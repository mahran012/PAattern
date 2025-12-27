using Microsoft.Extensions.Options;
using SAP.NET6.Data;
using System;
using System.Linq;

namespace SAP.NET6.Services
{
    public class PromotionDiscountHandler : OrderValidationHandler
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IOptionsMonitor<PromotionItem[]> _promotionOptions;

        public PromotionDiscountHandler(ApplicationDbContext dbContext, IOptionsMonitor<PromotionItem[]> promotionOptions)
        {
            _dbContext = dbContext;
            _promotionOptions = promotionOptions;
        }

        public override void Validate(OrderValidationRequest request)
        {
            var promotions = _promotionOptions.CurrentValue;
            
            foreach (var cartItem in request.Cart.Items)
            {
                var applicablePromotion = promotions.FirstOrDefault(p => p.ItemId == cartItem.ItemId);
                if (applicablePromotion != null)
                {
                    // Apply discount to this item
                    var originalPrice = cartItem.Item.Attributes.FirstOrDefault(a => a.Id == cartItem.Attributes.Id)?.Price ?? 0;
                    var discountAmount = originalPrice * (applicablePromotion.Discount / 100);
                    request.ItemDiscounts[cartItem.ItemId] = discountAmount;
                }
            }

            // Continue to the next handler
            base.Validate(request);
        }
    }
}