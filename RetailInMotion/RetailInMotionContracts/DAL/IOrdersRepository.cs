using RetailInMotionObjects.DataTransferObjects;
using RetailInMotionObjects.Models;
using System.Collections.Generic;

namespace RetailInMotionContracts.DAL
{
    public interface IOrdersRepository
    {
        int Add();
        int Add(Order order);
        int DeleteSoft(int id);
        Order Get(int id);
        OrderDTO GetWithItems(int id);
        IEnumerable<OrderDTO> List(int page, int size);
        int UpdateDeliveryAddress(Order order);
    }
}
