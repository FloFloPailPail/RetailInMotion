using RetailInMotionObjects.ApiModels;
using System.Collections.Generic;

namespace RetailInMotionContracts.BLL
{
    public interface IOrdersService
    {
        bool Cancel(int id);
        int Create();
        OrderApiModel Create(OrderApiModel order);
        OrderApiModel Get(int id);
        List<OrderApiModel> List(int page, int size);
        OrderApiModel UpdateDeliveryAddress(OrderApiModel order);
        OrderApiModel UpdateItems(int id, IEnumerable<OrderItemApiModel> orderItems);
    }
}
