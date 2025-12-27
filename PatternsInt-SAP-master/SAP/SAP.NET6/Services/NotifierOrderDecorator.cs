using System;
using System.Threading.Tasks;

namespace SAP.NET6.Services
{
    public class NotifierOrderDecorator : BaseOrderDecorator
    {
        private readonly INotifierService _notifierService;

        public NotifierOrderDecorator(IOrderService orderService, INotifierService notifierService) : base(orderService)
        {
            _notifierService = notifierService;
        }

        public override async Task CreateOrderFromCartWithValidationAsync()
        {
            try
            {
                await _orderService.CreateOrderFromCartWithValidationAsync();
                
                // Notify about the new order
                await _notifierService.Notify("New order has been created successfully.");
            }
            catch (Exception ex)
            {
                // In a real implementation, we would also notify about the error
                await _notifierService.Notify($"Error creating order: {ex.Message}");
                throw; // Re-throw the exception to maintain original behavior
            }
        }

        public override async Task CreateOrderFromCartAsync()
        {
            try
            {
                await _orderService.CreateOrderFromCartAsync();
                
                // Notify about the new order
                await _notifierService.Notify("New order has been created successfully.");
            }
            catch (Exception ex)
            {
                // In a real implementation, we would also notify about the error
                await _notifierService.Notify($"Error creating order: {ex.Message}");
                throw; // Re-throw the exception to maintain original behavior
            }
        }
    }
}