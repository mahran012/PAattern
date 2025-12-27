using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAP.NET6.Data;
using SAP.NET6.Data.Models;
using SAP.NET6.ViewModels.Orders;

namespace SAP.NET6.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private ApplicationDbContext DbContext { get; }

        private UserManager<IdentityUser> UserManager { get; }

        private IHttpContextAccessor HttpContextAccessor { get; }

        private IMapper Mapper { get; }

        private ICartService CartService { get; }

        private IOrderValidationHandler ValidationHandler { get; }

        public OrderService(ApplicationDbContext dbContext,
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ICartService cartService,
            IOrderValidationHandler validationHandler)
        {
            DbContext = dbContext;
            UserManager = userManager;
            HttpContextAccessor = httpContextAccessor;
            Mapper = mapper;
            CartService = cartService;
            ValidationHandler = validationHandler;
        }

        public async Task<OrderListViewModel> GetUserOrdersAsync()
        {
            var userId = UserManager.GetUserId(HttpContextAccessor.HttpContext.User);
            var orders = await DbContext.Orders
                .Where(x => x.User.Id == userId)
                .ToListAsync();

            var result = new OrderListViewModel();
            if (orders?.Any() == true)
            {
                result.Orders = orders.Select(Mapper.Map<OrderViewModel>).ToList();
            }

            return result;
        }

        public async Task CreateOrderFromCartAsync()
        {
            var user = await UserManager.GetUserAsync(HttpContextAccessor.HttpContext.User);
            var cart = await CartService.GetCurrentUserCartAsync();

            DbContext.Orders.Add(new Order
            {
                CreateDate = DateTime.Now,
                DiscountCoef = 1,
                TotalPrice = cart.TotalPrice,
                PriceWithDiscount = cart.TotalPrice,
                User = user,
                ItemToOrders = cart.Items.Select(x => new ItemToOrder
                {
                    AttributesId = x.Attributes.Id,
                    ItemId = x.ItemId,
                    Quantity = x.Quantity,
                    Price = x.Attributes.Price
                }).ToList()
            });

            await DbContext.SaveChangesAsync();
            await CartService.ClearAsync();
        }

        public async Task CreateOrderFromCartWithValidationAsync()
        {
            var user = await UserManager.GetUserAsync(HttpContextAccessor.HttpContext.User);
            var cart = await CartService.GetCurrentUserCartAsync();

            // Create validation request
            var validationRequest = new OrderValidationRequest
            {
                Cart = cart,
                User = user,
                TotalPrice = cart.TotalPrice,
                FinalPrice = cart.TotalPrice
            };

            // Add item IDs and quantities to the request
            foreach (var item in cart.Items)
            {
                validationRequest.ItemIds.Add(item.ItemId);
                validationRequest.ItemQuantities[item.ItemId] = item.Quantity;
            }

            // Run the validation pipeline
            ValidationHandler.Validate(validationRequest);

            // Calculate final price after applying discounts
            var finalPrice = validationRequest.TotalPrice;
            
            // Apply item discounts
            foreach (var item in cart.Items)
            {
                if (validationRequest.ItemDiscounts.ContainsKey(item.ItemId))
                {
                    var discount = validationRequest.ItemDiscounts[item.ItemId];
                    finalPrice -= discount * item.Quantity; // Apply discount per item multiplied by quantity
                }
            }
            
            // Apply order discount
            finalPrice = finalPrice * (1 - validationRequest.OrderDiscount);

            // Create the order
            DbContext.Orders.Add(new Order
            {
                CreateDate = DateTime.Now,
                DiscountCoef = 1 - validationRequest.OrderDiscount,
                TotalPrice = validationRequest.TotalPrice,
                PriceWithDiscount = finalPrice,
                User = user,
                ItemToOrders = cart.Items.Select(x => new ItemToOrder
                {
                    AttributesId = x.Attributes.Id,
                    ItemId = x.ItemId,
                    Quantity = x.Quantity,
                    Price = x.Attributes.Price - (validationRequest.ItemDiscounts.ContainsKey(x.ItemId) ? validationRequest.ItemDiscounts[x.ItemId] : 0)
                }).ToList()
            });

            await DbContext.SaveChangesAsync();
            await CartService.ClearAsync();
        }

        public async Task<OrderViewModel> GetOrderAsync(Guid id)
        {
            var userId = UserManager.GetUserId(HttpContextAccessor.HttpContext.User);
            var order = await DbContext.Orders
                .Include(o => o.ItemToOrders)
                    .ThenInclude(i => i.Item)
                .Include(o => o.ItemToOrders)
                    .ThenInclude(i => i.Attributes)
                .FirstOrDefaultAsync(x => x.Id == id && x.User.Id == userId);

            if (order == null)
            {
                throw new KeyNotFoundException();
            }

            return Mapper.Map<OrderViewModel>(order);
        }
    }
}
