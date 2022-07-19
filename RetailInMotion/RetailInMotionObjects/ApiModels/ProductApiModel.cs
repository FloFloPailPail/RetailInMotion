using System;
using System.Collections.Generic;
using System.Text;

namespace RetailInMotionObjects.ApiModels
{
    public class ProductApiModel
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public int StockQuantity { get; set; }
    }
}
