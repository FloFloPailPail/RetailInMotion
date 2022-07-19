using System;
using System.Collections.Generic;
using System.Text;

namespace RetailInMotionObjects.Models
{
    public class Product
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public int StockQuantity { get; set; }
    }
}
