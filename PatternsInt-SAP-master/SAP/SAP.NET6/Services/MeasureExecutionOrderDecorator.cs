using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SAP.NET6.Services
{
    public class MeasureExecutionOrderDecorator : BaseOrderDecorator
    {
        public MeasureExecutionOrderDecorator(IOrderService orderService) : base(orderService)
        {
        }

        public override async Task CreateOrderFromCartWithValidationAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _orderService.CreateOrderFromCartWithValidationAsync();
            }
            finally
            {
                stopwatch.Stop();
                // In a real implementation, this would be logged to a monitoring system
                Console.WriteLine($"CreateOrderFromCartWithValidationAsync execution time: {stopwatch.ElapsedMilliseconds} ms");
            }
        }

        public override async Task CreateOrderFromCartAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _orderService.CreateOrderFromCartAsync();
            }
            finally
            {
                stopwatch.Stop();
                // In a real implementation, this would be logged to a monitoring system
                Console.WriteLine($"CreateOrderFromCartAsync execution time: {stopwatch.ElapsedMilliseconds} ms");
            }
        }
    }
}