using System;
using System.Collections.Generic;

namespace RetailInMotionObjects.ApiModels
{
    public class OrderApiModel
    {
        public OrderApiModel()
        {
            OrderItems = new List<OrderItemApiModel>();
        }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string CountryCode { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeDeleted { get; set; }
        public DateTime DateTimeUpdated { get; set; }
        public int Id { get; set; }
        public string PostalCode { get; set; }
        public List<OrderItemApiModel> OrderItems { get; set; }
    }
}
