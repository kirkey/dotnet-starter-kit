using FSH.Starter.WebApi.Store.Application.GroceryItems.Import;
using Store.Infrastructure.Endpoints.Categories;
using Store.Infrastructure.Endpoints.Customers;
using Store.Infrastructure.Endpoints.CycleCounts;
using Store.Infrastructure.Endpoints.GroceryItems;
using Store.Infrastructure.Endpoints.InventoryTransfers;
using Store.Infrastructure.Endpoints.PriceLists;
using Store.Infrastructure.Endpoints.PurchaseOrders;
using Store.Infrastructure.Endpoints.SalesOrders;
using Store.Infrastructure.Endpoints.StockAdjustments;
using Store.Infrastructure.Endpoints.Suppliers;
using Store.Infrastructure.Endpoints.WarehouseLocations;
using Store.Infrastructure.Endpoints.Warehouses;
using Store.Infrastructure.Endpoints.WholesaleContracts;
using Store.Infrastructure.Endpoints.WholesalePricings;
using Store.Infrastructure.Import;
using Store.Infrastructure.Persistence;

namespace Store.Infrastructure;

/// <summary>
/// Main module configuration for the Store system.
/// Registers all store endpoints and services.
/// </summary>
public static class StoreModule
{
    /// <summary>
    /// Registers all store endpoints with the application.
    /// </summary>
    /// <param name="app">The endpoint route builder to configure.</param>
    /// <returns>The configured endpoint route builder.</returns>
    public static IEndpointRouteBuilder MapStoreEndpoints(this IEndpointRouteBuilder app)
    {
        var storeGroup = app.MapGroup("/store")
            .WithTags("Store")
            .WithDescription("Comprehensive store management module endpoints");

        // Map all functional area endpoints
        storeGroup.MapCategoriesEndpoints();
        storeGroup.MapCustomersEndpoints();
        storeGroup.MapCycleCountsEndpoints();
        storeGroup.MapGroceryItemsEndpoints();
        storeGroup.MapInventoryTransfersEndpoints();
        storeGroup.MapPriceListsEndpoints();
        storeGroup.MapPurchaseOrdersEndpoints();
        storeGroup.MapSalesOrdersEndpoints();
        storeGroup.MapStockAdjustmentsEndpoints();
        storeGroup.MapSuppliersEndpoints();
        storeGroup.MapWarehouseLocationsEndpoints();
        storeGroup.MapWarehousesEndpoints();
        storeGroup.MapWholesaleContractsEndpoints();
        storeGroup.MapWholesalePricingsEndpoints();

        return app;
    }

    /// <summary>
    /// Registers store services with dependency injection container.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The configured web application builder.</returns>
    public static WebApplicationBuilder RegisterStoreServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<StoreDbContext>();
        builder.Services.AddScoped<IDbInitializer, StoreDbInitializer>();

        // Register repositories without keys (for MediatR handlers that don't use keyed services)
        builder.Services.AddScoped<IRepository<Category>, StoreRepository<Category>>();
        builder.Services.AddScoped<IReadRepository<Category>, StoreRepository<Category>>();
        builder.Services.AddScoped<IRepository<Customer>, StoreRepository<Customer>>();
        builder.Services.AddScoped<IReadRepository<Customer>, StoreRepository<Customer>>();
        builder.Services.AddScoped<IRepository<CycleCount>, StoreRepository<CycleCount>>();
        builder.Services.AddScoped<IReadRepository<CycleCount>, StoreRepository<CycleCount>>();
        builder.Services.AddScoped<IRepository<CycleCountItem>, StoreRepository<CycleCountItem>>();
        builder.Services.AddScoped<IReadRepository<CycleCountItem>, StoreRepository<CycleCountItem>>();
        builder.Services.AddScoped<IRepository<GroceryItem>, StoreRepository<GroceryItem>>();
        builder.Services.AddScoped<IReadRepository<GroceryItem>, StoreRepository<GroceryItem>>();
        builder.Services.AddScoped<IRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>();
        builder.Services.AddScoped<IReadRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>();
        builder.Services.AddScoped<IRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>();
        builder.Services.AddScoped<IReadRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>();
        builder.Services.AddScoped<IRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>();
        builder.Services.AddScoped<IReadRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>();
        builder.Services.AddScoped<IRepository<PriceList>, StoreRepository<PriceList>>();
        builder.Services.AddScoped<IReadRepository<PriceList>, StoreRepository<PriceList>>();
        builder.Services.AddScoped<IRepository<PriceListItem>, StoreRepository<PriceListItem>>();
        builder.Services.AddScoped<IReadRepository<PriceListItem>, StoreRepository<PriceListItem>>();
        builder.Services.AddScoped<IRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>();
        builder.Services.AddScoped<IReadRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>();
        builder.Services.AddScoped<IRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>();
        builder.Services.AddScoped<IReadRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>();
        builder.Services.AddScoped<IRepository<SalesOrder>, StoreRepository<SalesOrder>>();
        builder.Services.AddScoped<IReadRepository<SalesOrder>, StoreRepository<SalesOrder>>();
        builder.Services.AddScoped<IRepository<SalesOrderItem>, StoreRepository<SalesOrderItem>>();
        builder.Services.AddScoped<IReadRepository<SalesOrderItem>, StoreRepository<SalesOrderItem>>();
        builder.Services.AddScoped<IRepository<StockAdjustment>, StoreRepository<StockAdjustment>>();
        builder.Services.AddScoped<IReadRepository<StockAdjustment>, StoreRepository<StockAdjustment>>();
        builder.Services.AddScoped<IRepository<Supplier>, StoreRepository<Supplier>>();
        builder.Services.AddScoped<IReadRepository<Supplier>, StoreRepository<Supplier>>();
        builder.Services.AddScoped<IRepository<Warehouse>, StoreRepository<Warehouse>>();
        builder.Services.AddScoped<IReadRepository<Warehouse>, StoreRepository<Warehouse>>();
        builder.Services.AddScoped<IRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>();
        builder.Services.AddScoped<IReadRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>();
        builder.Services.AddScoped<IRepository<WholesaleContract>, StoreRepository<WholesaleContract>>();
        builder.Services.AddScoped<IReadRepository<WholesaleContract>, StoreRepository<WholesaleContract>>();
        builder.Services.AddScoped<IRepository<WholesalePricing>, StoreRepository<WholesalePricing>>();
        builder.Services.AddScoped<IReadRepository<WholesalePricing>, StoreRepository<WholesalePricing>>();
        
        // Register with keyed services (for handlers that use specific keys)
        builder.Services.AddKeyedScoped<IRepository<Category>, StoreRepository<Category>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<Category>, StoreRepository<Category>>("store");
        builder.Services.AddKeyedScoped<IRepository<Customer>, StoreRepository<Customer>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<Customer>, StoreRepository<Customer>>("store");
        builder.Services.AddKeyedScoped<IRepository<CycleCount>, StoreRepository<CycleCount>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<CycleCount>, StoreRepository<CycleCount>>("store");
        builder.Services.AddKeyedScoped<IRepository<CycleCountItem>, StoreRepository<CycleCountItem>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<CycleCountItem>, StoreRepository<CycleCountItem>>("store");
        builder.Services.AddKeyedScoped<IRepository<GroceryItem>, StoreRepository<GroceryItem>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<GroceryItem>, StoreRepository<GroceryItem>>("store");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>("store");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>("store");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store");
        builder.Services.AddKeyedScoped<IRepository<PriceList>, StoreRepository<PriceList>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<PriceList>, StoreRepository<PriceList>>("store");
        builder.Services.AddKeyedScoped<IRepository<PriceListItem>, StoreRepository<PriceListItem>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<PriceListItem>, StoreRepository<PriceListItem>>("store");
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store");
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>("store");
        builder.Services.AddKeyedScoped<IRepository<SalesOrder>, StoreRepository<SalesOrder>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<SalesOrder>, StoreRepository<SalesOrder>>("store");
        builder.Services.AddKeyedScoped<IRepository<SalesOrderItem>, StoreRepository<SalesOrderItem>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<SalesOrderItem>, StoreRepository<SalesOrderItem>>("store");
        builder.Services.AddKeyedScoped<IRepository<StockAdjustment>, StoreRepository<StockAdjustment>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<StockAdjustment>, StoreRepository<StockAdjustment>>("store");
        builder.Services.AddKeyedScoped<IRepository<Supplier>, StoreRepository<Supplier>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<Supplier>, StoreRepository<Supplier>>("store");
        builder.Services.AddKeyedScoped<IRepository<Warehouse>, StoreRepository<Warehouse>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<Warehouse>, StoreRepository<Warehouse>>("store");
        builder.Services.AddKeyedScoped<IRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>("store");
        builder.Services.AddKeyedScoped<IRepository<WholesaleContract>, StoreRepository<WholesaleContract>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<WholesaleContract>, StoreRepository<WholesaleContract>>("store");
        builder.Services.AddKeyedScoped<IRepository<WholesalePricing>, StoreRepository<WholesalePricing>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<WholesalePricing>, StoreRepository<WholesalePricing>>("store");

        // Register with specific keys for different functional areas
        builder.Services.AddKeyedScoped<IRepository<Category>, StoreRepository<Category>>("store:categories");
        builder.Services.AddKeyedScoped<IReadRepository<Category>, StoreRepository<Category>>("store:categories");
        builder.Services.AddKeyedScoped<IRepository<Customer>, StoreRepository<Customer>>("store:customers");
        builder.Services.AddKeyedScoped<IReadRepository<Customer>, StoreRepository<Customer>>("store:customers");
        builder.Services.AddKeyedScoped<IRepository<CycleCount>, StoreRepository<CycleCount>>("store:cycle-counts");
        builder.Services.AddKeyedScoped<IReadRepository<CycleCount>, StoreRepository<CycleCount>>("store:cycle-counts");
        builder.Services.AddKeyedScoped<IRepository<CycleCountItem>, StoreRepository<CycleCountItem>>("store:cycle-count-items");
        builder.Services.AddKeyedScoped<IReadRepository<CycleCountItem>, StoreRepository<CycleCountItem>>("store:cycle-count-items");
        builder.Services.AddKeyedScoped<IRepository<GroceryItem>, StoreRepository<GroceryItem>>("store:grocery-items");
        builder.Services.AddKeyedScoped<IReadRepository<GroceryItem>, StoreRepository<GroceryItem>>("store:grocery-items");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>("store:inventory-transfers");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>("store:inventory-transfers");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>("store:inventory-transfer-items");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>("store:inventory-transfer-items");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store:inventory-transactions");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store:inventory-transactions");
        builder.Services.AddKeyedScoped<IRepository<PriceList>, StoreRepository<PriceList>>("store:price-lists");
        builder.Services.AddKeyedScoped<IReadRepository<PriceList>, StoreRepository<PriceList>>("store:price-lists");
        builder.Services.AddKeyedScoped<IRepository<PriceListItem>, StoreRepository<PriceListItem>>("store:price-list-items");
        builder.Services.AddKeyedScoped<IReadRepository<PriceListItem>, StoreRepository<PriceListItem>>("store:price-list-items");
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store:purchase-orders");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store:purchase-orders");
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>("store:purchase-order-items");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>("store:purchase-order-items");
        builder.Services.AddKeyedScoped<IRepository<SalesOrder>, StoreRepository<SalesOrder>>("store:sales-orders");
        builder.Services.AddKeyedScoped<IReadRepository<SalesOrder>, StoreRepository<SalesOrder>>("store:sales-orders");
        builder.Services.AddKeyedScoped<IRepository<SalesOrderItem>, StoreRepository<SalesOrderItem>>("store:sales-order-items");
        builder.Services.AddKeyedScoped<IReadRepository<SalesOrderItem>, StoreRepository<SalesOrderItem>>("store:sales-order-items");
        builder.Services.AddKeyedScoped<IRepository<StockAdjustment>, StoreRepository<StockAdjustment>>("store:stock-adjustments");
        builder.Services.AddKeyedScoped<IReadRepository<StockAdjustment>, StoreRepository<StockAdjustment>>("store:stock-adjustments");
        builder.Services.AddKeyedScoped<IRepository<Supplier>, StoreRepository<Supplier>>("store:suppliers");
        builder.Services.AddKeyedScoped<IReadRepository<Supplier>, StoreRepository<Supplier>>("store:suppliers");
        builder.Services.AddKeyedScoped<IRepository<Warehouse>, StoreRepository<Warehouse>>("store:warehouses");
        builder.Services.AddKeyedScoped<IReadRepository<Warehouse>, StoreRepository<Warehouse>>("store:warehouses");
        builder.Services.AddKeyedScoped<IRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>("store:warehouse-locations");
        builder.Services.AddKeyedScoped<IReadRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>("store:warehouse-locations");
        builder.Services.AddKeyedScoped<IRepository<WholesaleContract>, StoreRepository<WholesaleContract>>("store:wholesale-contracts");
        builder.Services.AddKeyedScoped<IReadRepository<WholesaleContract>, StoreRepository<WholesaleContract>>("store:wholesale-contracts");
        builder.Services.AddKeyedScoped<IRepository<WholesalePricing>, StoreRepository<WholesalePricing>>("store:wholesale-pricings");
        builder.Services.AddKeyedScoped<IReadRepository<WholesalePricing>, StoreRepository<WholesalePricing>>("store:wholesale-pricings");

        // Register import services
        builder.Services.AddScoped<IGroceryItemImportParser, GroceryItemImportParser>();

        return builder;
    }

    public static WebApplication UseStoreModule(this WebApplication app)
    {
        return app;
    }
}
