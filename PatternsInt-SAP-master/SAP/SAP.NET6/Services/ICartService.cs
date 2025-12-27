using SAP.NET6.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SAP.NET6.Services
{
    public interface ICartService
    {
        Task<CartViewModel> GetCurrentUserCartAsync();

        Task AddToCartAsync(AddItemToCartViewModel item);

        Task<bool> IsItemInCartAsync(Guid id);

        Task ChangeQuantityAsync(Guid itemId, int dif);

        Task RemoveItemAsync(Guid itemId);

        Task ClearAsync();
    }
}
