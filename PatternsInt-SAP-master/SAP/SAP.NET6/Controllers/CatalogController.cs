using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SAP.NET6.Services;
using SAP.NET6.Services.Catalog;

namespace SAP.NET6.Controllers
{
    [Route("catalog")]
    public class CatalogController : Controller
    {
        ICatalogDataProvider CatalogDataProvider { get; }

        ICartService CartService { get; }

        public CatalogController(ICatalogDataProvider catalogDataProvider,
            ICartService cartService)
        {
            CatalogDataProvider = catalogDataProvider;
            CartService = cartService;
        }

        [Route("")]
        public async Task<ActionResult> Index()
        {
            var catalog = await CatalogDataProvider.GetCatalogAsync();

            return View(catalog);
        }

        [Route("node/{id}")]
        public async Task<ActionResult> Node(Guid id)
        {
            var catalog = await CatalogDataProvider.GetCatalogAsync(id);
            return View("Index", catalog);
        }

        [Route("item/{id}")]
        public async Task<ActionResult> Item(Guid id)
        {
            var item = await CatalogDataProvider.GetItemAsync(id);
            item.IsInCart = await CartService.IsItemInCartAsync(id);
            return View(item);
        }
    }
}