using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAP.NET6;
using SAP.NET6.Data;
using SAP.NET6.Services;
using SAP.NET6.Services.Catalog;
using SAP.NET6.Services.Catalog.FileProcessing;
using SAP.NET6.Services.Catalog.Implementations;
using SAP.NET6.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
builder.Services.AddSingleton(mapConfig.CreateMapper());
builder.Services.AddScoped<ICatalogDataProvider, CatalogDataProvider>();
builder.Services.AddScoped<ICatalogAdministration, CatalogAdministration>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<CatalogFileProcessor>();
// Register the actual API client without the interface first
builder.Services.AddHttpClient<DistributorApiClient>(client =>
{
    client.BaseAddress = new Uri("https://sap.thevvp.ru/");
    client.DefaultRequestHeaders.Add("User-Agent", "SAP-WebShop/1.0");
});

// Register the caching proxy as the IDistributorApiClient
builder.Services.AddScoped<IDistributorApiClient, CachingDistributorApiClient>();

builder.Services.AddScoped<OrderService>();

// Configuration for loyalty and promotions
builder.Services.Configure<LoyaltyConfiguration>(builder.Configuration.GetSection("Loyalty"));
var promotionItems = builder.Configuration.GetSection("PromotionItems").Get<PromotionItem[]>();
builder.Services.Configure<PromotionItem[]>(options => options = promotionItems);

// Configuration for notifications
builder.Services.Configure<NotificationsConfiguration>(builder.Configuration.GetSection("Notifications"));

// Register validation handlers
builder.Services.AddScoped<StockValidationHandler>();
builder.Services.AddScoped<PromotionDiscountHandler>();
builder.Services.AddScoped<LoyaltyDiscountHandler>();

// Register the validation pipeline
builder.Services.AddScoped<IOrderValidationHandler>(provider =>
{
    var stockHandler = provider.GetRequiredService<StockValidationHandler>();
    var promotionHandler = provider.GetRequiredService<PromotionDiscountHandler>();
    var loyaltyHandler = provider.GetRequiredService<LoyaltyDiscountHandler>();
    
    // Build the chain: Stock -> Promotion -> Loyalty
    stockHandler.SetNext(promotionHandler).SetNext(loyaltyHandler);
    
    return stockHandler;
});

// Register decorator services
builder.Services.AddScoped<INotifierService, NotifierService>(); // We'll create this later
// Manually register the decorator chain
builder.Services.AddScoped<IOrderService>(provider =>
{
    var orderService = provider.GetRequiredService<OrderService>();
    var notifierService = provider.GetRequiredService<INotifierService>();
    
    // Wrap with decorators: NotifierDecorator -> MeasureExecutionDecorator -> OrderService
    var measureDecorator = new MeasureExecutionOrderDecorator(orderService);
    var notifierDecorator = new NotifierOrderDecorator(measureDecorator, notifierService);
    
    return notifierDecorator;
});

builder.Services.AddScoped<ApplicationDbInitializer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    //Automigration of database
    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    //Seed database
    var initializer = scope.ServiceProvider.GetService<ApplicationDbInitializer>();
    initializer.SeedUsers();
    initializer.SeedCatalog();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
