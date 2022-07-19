using RetailInMotionObjects.Models;
using System.Collections.Generic;

namespace RetailInMotionContracts.DAL
{
    public interface IOrderItemsRepository
    {
        int Add(List<OrderItem> orderItems);
        int Delete(List<int> orderItemIds);
        int Edit(List<OrderItem> orderItems);
    }
}
