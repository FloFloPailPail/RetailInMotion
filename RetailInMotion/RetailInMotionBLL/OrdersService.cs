using RetailInMotionContracts.BLL;
using RetailInMotionContracts.DAL;
using RetailInMotionObjects;
using RetailInMotionObjects.ApiModels;
using RetailInMotionObjects.DataTransferObjects;
using RetailInMotionObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetailInMotionBLL
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IOrderItemsRepository _orderItemsRepository;

        public OrdersService(IOrdersRepository ordersRepository, IOrderItemsRepository orderItemsRepository)
        {
            _ordersRepository = ordersRepository;
            _orderItemsRepository = orderItemsRepository;
        }

        public bool Cancel(int id)
        {
            var result = _ordersRepository.DeleteSoft(id);
            return result > 0;
        }

        public int Create()
        {
            int result = _ordersRepository.Add();
            return result;
        }

        public OrderApiModel Create(OrderApiModel order)
        {
            if (order == null)
            {
                //Bad request
                return null;
            }

            var utcNow = DateTime.UtcNow;
            int result = _ordersRepository.Add(
                new Order()
                {
                    AddressLine1 = order.AddressLine1,
                    AddressLine2 = order.AddressLine2,
                    AddressLine3 = order.AddressLine3,
                    AddressLine4 = order.AddressLine4,
                    PostalCode = order.PostalCode,
                    CountryCode = order.CountryCode,
                    DateTimeCreated = utcNow,
                    DateTimeUpdated = utcNow,
                });
            if (result == 0)
            {
                //ERROR
            }

            if ((order.OrderItems?.Count ?? 0) > 0)
            {
                int addItemsResult = _orderItemsRepository.Add(Map(order.OrderItems));
                if (addItemsResult == 0)
                {
                    //ERROR
                }
            }

            order.Id = result;
            return order;
        }

        public OrderApiModel Get(int id)
        {
            var order = _ordersRepository.GetWithItems(id);
            if (order == null)
            {
                //Error Not Found 404
                return null;
            }

            return Map(order);
        }

        public List<OrderApiModel> List(int page, int size)
        {
            var orders = _ordersRepository.List(page, size);

            return Map(orders);
        }

        public OrderApiModel UpdateDeliveryAddress(OrderApiModel order)
        {
            if (order == null)
            {
                //Bad Request
                throw new ArgumentException("Order object is null");
            }

            var orderToEdit = _ordersRepository.Get(order.Id);
            orderToEdit.AddressLine1 = order.AddressLine1;
            orderToEdit.AddressLine2 = order.AddressLine2;
            orderToEdit.AddressLine3 = order.AddressLine3;
            orderToEdit.AddressLine4 = order.AddressLine4;
            orderToEdit.CountryCode = order.CountryCode;
            orderToEdit.PostalCode = order.PostalCode;
            orderToEdit.DateTimeUpdated = DateTime.UtcNow;

            var result = _ordersRepository.UpdateDeliveryAddress(orderToEdit);
            if (result == 0)
            {
                //LogError
                throw new Exception("An error has occured while updating delivery address");
            }

            return order;
        }

        public OrderApiModel UpdateItems(int id, IEnumerable<OrderItemApiModel> orderItems)
        {
            var orderToEdit = _ordersRepository.GetWithItems(id);
            if (orderToEdit == null)
            {
                //Error Not Found 404
                return null;
            }

            var orderItemsToAdd = new List<OrderItem>();
            var orderItemIdsToDelete = new List<int>();
            var orderItemsToEdit = new List<OrderItem>();

            CompareOrderItems(orderToEdit, orderItems, orderItemsToAdd, orderItemIdsToDelete, orderItemsToEdit);

            UpdateOrderItems(orderItemsToAdd, orderItemsToEdit, orderItemIdsToDelete);

            var order = _ordersRepository.GetWithItems(id);
            if (order == null)
            {
                //LogError
                throw new Exception("An error has occured while retrieving order.");
            }

            return Map(order);
        }

        #region Helper Methods

        private void CompareOrderItems(
            OrderDTO orderToEdit,
            IEnumerable<OrderItemApiModel> orderItems,
            List<OrderItem> orderItemsToAdd,
            List<int> orderItemIdsToDelete,
            List<OrderItem> orderItemsToEdit)
        {
            var orderItemsDictionary = new Dictionary<int, OrderItem>();
            var orderItemsUpdatedDictionary = new Dictionary<int, OrderItemApiModel>();

            foreach (var item in orderToEdit.OrderItems ?? Enumerable.Empty<OrderItem>())
            {
                orderItemsDictionary.TryAdd(item.Id, item);
            }

            foreach (var item in orderItems ?? Enumerable.Empty<OrderItemApiModel>())
            {
                if (orderItemsDictionary.ContainsKey(item.Id) == false)
                {
                    orderItemsToAdd.Add(Map(item));
                }
                else
                {
                    orderItemsToEdit.Add(Map(item));
                }

                orderItemsUpdatedDictionary.TryAdd(item.Id, item);
            }

            foreach (var item in orderToEdit.OrderItems ?? Enumerable.Empty<OrderItem>())
            {
                if (orderItemsUpdatedDictionary.ContainsKey(item.Id) == false)
                {
                    orderItemIdsToDelete.Add(item.Id);
                }
            }
        }

        private void UpdateOrderItems(List<OrderItem> orderItemsToAdd, List<OrderItem> orderItemsToEdit, List<int> orderItemIdsToDelete)
        {
            if ((orderItemsToAdd?.Count ?? 0) > 0)
            {
                var itemsAddedResult = _orderItemsRepository.Add(orderItemsToAdd);
                if (itemsAddedResult == 0)
                {
                    //ERROR
                }
            }

            if ((orderItemsToEdit?.Count ?? 0) > 0)
            {
                var itemsEditedResult = _orderItemsRepository.Edit(orderItemsToEdit);
                if (itemsEditedResult == 0)
                {
                    //ERROR
                }
            }

            if ((orderItemIdsToDelete?.Count ?? 0) > 0)
            {
                var itemsDeletedResult = _orderItemsRepository.Delete(orderItemIdsToDelete);
                if (itemsDeletedResult == 0)
                {
                    //ERROR
                }
            }
        }

        private OrderApiModel Map(OrderDTO order)
        {
            return new OrderApiModel()
            {
                AddressLine1 = order.AddressLine1,
                AddressLine2 = order.AddressLine2,
                AddressLine3 = order.AddressLine3,
                AddressLine4 = order.AddressLine4,
                CountryCode = order.CountryCode,
                DateTimeCreated = order.DateTimeCreated,
                DateTimeUpdated = order.DateTimeUpdated,
                DateTimeDeleted = order.DateTimeDeleted,
                Id = order.Id,
                PostalCode = order.PostalCode,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemApiModel()
                {
                    Id = oi.Id,
                    OrderId = oi.OrderId,
                    ProductGuid = oi.ProductGuid,
                    Quantity = oi.Quantity
                })?.ToList()
            };
        }

        private List<OrderApiModel> Map(IEnumerable<OrderDTO> orders)
        {
            var ordersList = new List<OrderApiModel>();
            foreach (var order in orders ?? Enumerable.Empty<OrderDTO>())
            {
                ordersList.Add(Map(order));
            }

            return ordersList;
        }

        private OrderItem Map(OrderItemApiModel orderItem)
        {
            return new OrderItem()
            {
                Id = orderItem.Id,
                OrderId = orderItem.OrderId,
                ProductGuid = orderItem.ProductGuid,
                Quantity = orderItem.Quantity
            };
        }

        private List<OrderItem> Map(IEnumerable<OrderItemApiModel> orderItems)
        {
            var orderItemsList = new List<OrderItem>();
            foreach (var orderItem in orderItems ?? Enumerable.Empty<OrderItemApiModel>())
            {
                orderItemsList.Add(Map(orderItem));
            }

            return orderItemsList;
        }

        #endregion
    }
}
