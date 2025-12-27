using SAP.NET6.Data;
using SAP.NET6.Data.Models.Catalog;
using System;
using System.Linq;

namespace SAP.NET6.Services
{
    public class StockValidationHandler : OrderValidationHandler
    {
        private readonly ApplicationDbContext _dbContext;

        public StockValidationHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override void Validate(OrderValidationRequest request)
        {
            foreach (var cartItem in request.Cart.Items)
            {
                var item = _dbContext.Items.FirstOrDefault(i => i.Id == cartItem.ItemId);
                if (item == null)
                {
                    throw new InvalidOperationException($"Item with ID {cartItem.ItemId} does not exist in the catalog.");
                }

                if (item.QuantityInStock < cartItem.Quantity)
                {
                    throw new InvalidOperationException($"Item '{item.Name}' is out of stock. Requested: {cartItem.Quantity}, Available: {item.QuantityInStock}");
                }
            }

            // If all validations pass, continue to the next handler
            base.Validate(request);
        }
    }
}