using Carter;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using Store.Domain;
using Store.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Store.Infrastructure;

public static class StoreModule
{
    // Optional Carter endpoints holder (not required if endpoints are registered via IEndpoint implementations)
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("store") { }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            // endpoints are implemented as IEndpoint classes under Endpoints/v1, so module routes don't need to add routes here.
        }
    }

    public static WebApplicationBuilder RegisterStoreServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Bind DB context using the framework helper which reads DatabaseOptions
        builder.Services.BindDbContext<StoreDbContext>();

        // If a DB initializer is present, register it (we add one in Persistence)
        builder.Services.AddScoped<IDbInitializer, StoreDbInitializer>();

        // Register keyed repositories expected by Store application handlers
        builder.Services.AddKeyedScoped<IRepository<GroceryItem>, StoreRepository<GroceryItem>>("store:grocery-items");
        builder.Services.AddKeyedScoped<IReadRepository<GroceryItem>, StoreRepository<GroceryItem>>("store:grocery-items");

        builder.Services.AddKeyedScoped<IRepository<Customer>, StoreRepository<Customer>>("store:customers");
        builder.Services.AddKeyedScoped<IReadRepository<Customer>, StoreRepository<Customer>>("store:customers");

        builder.Services.AddKeyedScoped<IRepository<Warehouse>, StoreRepository<Warehouse>>("store:warehouses");
        builder.Services.AddKeyedScoped<IReadRepository<Warehouse>, StoreRepository<Warehouse>>("store:warehouses");

        builder.Services.AddKeyedScoped<IRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>("store:warehouse-locations");
        builder.Services.AddKeyedScoped<IReadRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>("store:warehouse-locations");

        builder.Services.AddKeyedScoped<IRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>("store:inventory-transfers");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>("store:inventory-transfers");

        builder.Services.AddKeyedScoped<IRepository<StockAdjustment>, StoreRepository<StockAdjustment>>("store:stock-adjustments");
        builder.Services.AddKeyedScoped<IReadRepository<StockAdjustment>, StoreRepository<StockAdjustment>>("store:stock-adjustments");

        // Additional common entity registrations (purchase orders, sales orders, price lists)
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store:purchase-orders");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store:purchase-orders");

        builder.Services.AddKeyedScoped<IRepository<SalesOrder>, StoreRepository<SalesOrder>>("store:sales-orders");
        builder.Services.AddKeyedScoped<IReadRepository<SalesOrder>, StoreRepository<SalesOrder>>("store:sales-orders");

        builder.Services.AddKeyedScoped<IRepository<PriceList>, StoreRepository<PriceList>>("store:price-lists");
        builder.Services.AddKeyedScoped<IReadRepository<PriceList>, StoreRepository<PriceList>>("store:price-lists");

        return builder;
    }

    public static WebApplication UseStoreModule(this WebApplication app)
    {
        return app;
    }
}
