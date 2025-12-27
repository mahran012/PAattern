using System;
using System.Threading.Tasks;

namespace SAP.NET6.Services
{
    public abstract class BaseOrderDecorator : IOrderService
    {
        protected readonly IOrderService _orderService;

        public BaseOrderDecorator(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public virtual Task<OrderListViewModel> GetUserOrdersAsync()
        {
            return _orderService.GetUserOrdersAsync();
        }

        public virtual Task CreateOrderFromCartAsync()
        {
            return _orderService.CreateOrderFromCartAsync();
        }

        public virtual Task CreateOrderFromCartWithValidationAsync()
        {
            return _orderService.CreateOrderFromCartWithValidationAsync();
        }

        public virtual Task<OrderViewModel> GetOrderAsync(Guid id)
        {
            return _orderService.GetOrderAsync(id);
        }
    }
}