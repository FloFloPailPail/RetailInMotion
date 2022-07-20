using System;

namespace RetailInMotionObjects.Models
{
    public class Product
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public int StockQuantity { get; set; }
    }
}
