using System;
using System.Collections.Generic;
using System.Text;

namespace RetailInMotionObjects.ApiModels
{
    public class OrderItemApiModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Guid ProductGuid { get; set; }
        public int Quantity { get; set; }
    }
}
