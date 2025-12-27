using SAP.NET6.Data.Models;
using SAP.NET6.ViewModels.Cart;
using System;
using System.Collections.Generic;

namespace SAP.NET6.Services
{
    public class OrderValidationRequest
    {
        public CartViewModel Cart { get; set; }
        public IdentityUser User { get; set; }
        public List<Guid> ItemIds { get; set; }
        public Dictionary<Guid, int> ItemQuantities { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public Dictionary<Guid, decimal> ItemDiscounts { get; set; }
        public decimal OrderDiscount { get; set; }
        
        public OrderValidationRequest()
        {
            ItemIds = new List<Guid>();
            ItemQuantities = new Dictionary<Guid, int>();
            ItemDiscounts = new Dictionary<Guid, decimal>();
        }
    }
}