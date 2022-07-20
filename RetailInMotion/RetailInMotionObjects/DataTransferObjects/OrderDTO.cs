using RetailInMotionObjects.Models;
using System.Collections.Generic;

namespace RetailInMotionObjects.DataTransferObjects
{
    public class OrderDTO : Order
    {
        public OrderDTO()
        {
            OrderItems = new List<OrderItem>();
        }
        public List<OrderItem> OrderItems { get; set; }
    }
}
