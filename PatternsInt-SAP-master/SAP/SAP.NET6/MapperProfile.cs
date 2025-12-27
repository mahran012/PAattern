using AutoMapper;
using SAP.NET6.Data.Models;
using SAP.NET6.Data.Models.Catalog;
using SAP.NET6.ViewModels.Cart;
using SAP.NET6.ViewModels.Catalog;
using SAP.NET6.ViewModels.Catalog.Admin;
using SAP.NET6.ViewModels.Orders;

namespace SAP.NET6
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // From db to view entities
            CreateMap<Item, ItemViewModel>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Attributes, AttributesViewModel>();
            CreateMap<ItemToCart, CartItemViewModel>();
            CreateMap<Cart, CartViewModel>().ForMember(x => x.Items, opt => opt.MapFrom(c => c.ItemToCarts));
            CreateMap<ItemToOrder, OrderItemViewModel>();
            CreateMap<Order, OrderViewModel>().ForMember(x => x.OrderItems, opt => opt.MapFrom(o => o.ItemToOrders));

            // From view to db entities
            CreateMap<CreateCategoryViewModel, Category>();
            CreateMap<CreateItemViewModel, Item>();
        }
    }
}
