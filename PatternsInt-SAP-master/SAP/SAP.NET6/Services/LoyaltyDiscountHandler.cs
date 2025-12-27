using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SAP.NET6.Data;
using SAP.NET6.Data.Models;
using System;
using System.Linq;

namespace SAP.NET6.Services
{
    public class LoyaltyDiscountHandler : OrderValidationHandler
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOptionsMonitor<LoyaltyConfiguration> _loyaltyOptions;

        public LoyaltyDiscountHandler(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IOptionsMonitor<LoyaltyConfiguration> loyaltyOptions)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _loyaltyOptions = loyaltyOptions;
        }

        public override void Validate(OrderValidationRequest request)
        {
            var loyaltyConfig = _loyaltyOptions.CurrentValue;
            
            // Count the number of orders for the current user
            var userOrderCount = _dbContext.Orders.Count(o => o.User.Id == request.User.Id);
            
            // If user has more orders than the threshold, apply loyalty discount
            if (userOrderCount >= loyaltyConfig.UserOrdersAmount)
            {
                request.OrderDiscount = loyaltyConfig.Discount / 100; // Convert percentage to decimal
            }

            // Continue to the next handler
            base.Validate(request);
        }
    }
}