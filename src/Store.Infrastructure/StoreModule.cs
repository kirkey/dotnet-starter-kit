using Store.Infrastructure.Endpoints.Customers.v1;
using Store.Infrastructure.Endpoints.GroceryItems.v1;
using Store.Infrastructure.Endpoints.InventoryTransfers.v1;
using Store.Infrastructure.Endpoints.SalesOrders.v1;
using Store.Infrastructure.Endpoints.StockAdjustments.v1;
using Store.Infrastructure.Endpoints.WarehouseLocations.v1;
using Store.Infrastructure.Endpoints.Warehouses.v1;
using Store.Infrastructure.Persistence;

namespace Store.Infrastructure;

public static class StoreModule
{
    // Optional Carter endpoints holder (not required if endpoints are registered via IEndpoint implementations)
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("store") { }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            // Register endpoints using Catalog-style grouped mapping for clarity
            var groceryGroup = app.MapGroup("grocery-items").WithTags("Grocery Items");
            groceryGroup.MapCreateGroceryItemEndpoint();
            groceryGroup.MapGetGroceryItemEndpoint();
            groceryGroup.MapUpdateGroceryItemEndpoint();
            groceryGroup.MapDeleteGroceryItemEndpoint();

            var customerGroup = app.MapGroup("customers").WithTags("Customers");
            customerGroup.MapCreateCustomerEndpoint();
            customerGroup.MapGetCustomerEndpoint();
            customerGroup.MapUpdateCustomerEndpoint();
            customerGroup.MapDeleteCustomerEndpoint();

            var inventoryGroup = app.MapGroup("inventory-transfers").WithTags("Inventory Transfers");
            inventoryGroup.MapCreateInventoryTransferEndpoint();
            inventoryGroup.MapGetInventoryTransferEndpoint();
            inventoryGroup.MapUpdateInventoryTransferEndpoint();
            inventoryGroup.MapSearchInventoryTransfersEndpoint();

            var stockGroup = app.MapGroup("stock-adjustments").WithTags("Stock Adjustments");
            stockGroup.MapCreateStockAdjustmentEndpoint();
            stockGroup.MapGetStockAdjustmentEndpoint();
            stockGroup.MapApproveStockAdjustmentEndpoint();
            // Note: search endpoint removed until a SearchStockAdjustmentsQuery is implemented

            var warehouses = app.MapGroup("warehouses").WithTags("Warehouses");
            warehouses.MapCreateWarehouseEndpoint();
            warehouses.MapGetWarehouseEndpoint();
            warehouses.MapUpdateWarehouseEndpoint();
            warehouses.MapDeleteWarehouseEndpoint();

            var whLocations = app.MapGroup("warehouse-locations").WithTags("Warehouse Locations");
            whLocations.MapCreateWarehouseLocationEndpoint();
            whLocations.MapGetWarehouseLocationEndpoint();
            whLocations.MapSearchWarehouseLocationsEndpoint();
            whLocations.MapUpdateWarehouseLocationEndpoint();

            var sales = app.MapGroup("sales-orders").WithTags("Sales Orders");
            sales.MapCreateSalesOrderEndpoint();
            sales.MapGetSalesOrderEndpoint();
            sales.MapUpdateSalesOrderEndpoint();
            sales.MapDeleteSalesOrderEndpoint();
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
