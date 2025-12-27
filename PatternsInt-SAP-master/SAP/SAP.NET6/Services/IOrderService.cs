using SAP.NET6.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.NET6.Services
{
    public interface IOrderService
    {
        Task<OrderListViewModel> GetUserOrdersAsync();

        Task CreateOrderFromCartAsync();

        Task CreateOrderFromCartWithValidationAsync();

        Task<OrderViewModel> GetOrderAsync(Guid id);
    }
}
