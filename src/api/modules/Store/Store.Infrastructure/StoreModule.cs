using FSH.Starter.WebApi.Store.Application.GroceryItems.Import;
using Store.Infrastructure.Endpoints.Categories.v1;
using Store.Infrastructure.Endpoints.Customers.v1;
using Store.Infrastructure.Endpoints.CycleCounts.v1;
using Store.Infrastructure.Endpoints.GroceryItems.v1;
using Store.Infrastructure.Endpoints.InventoryTransfers.v1;
using Store.Infrastructure.Endpoints.PriceLists.v1;
using Store.Infrastructure.Endpoints.PurchaseOrders.v1;
using Store.Infrastructure.Endpoints.SalesOrders.v1;
using Store.Infrastructure.Endpoints.StockAdjustments.v1;
using Store.Infrastructure.Endpoints.Suppliers.v1;
using Store.Infrastructure.Endpoints.WarehouseLocations.v1;
using Store.Infrastructure.Endpoints.Warehouses.v1;
using Store.Infrastructure.Import;
using Store.Infrastructure.Persistence;

namespace Store.Infrastructure;

public static class StoreModule
{
    // Optional Carter endpoints holder (not required if endpoints are registered via IEndpoint implementations)
    public class Endpoints() : CarterModule("store")
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            // Register endpoints using Catalog-style grouped mapping for clarity
            var groceryGroup = app.MapGroup("grocery-items").WithTags("Grocery Items");
            groceryGroup.MapCreateGroceryItemEndpoint();
            groceryGroup.MapGetGroceryItemEndpoint();
            groceryGroup.MapUpdateGroceryItemEndpoint();
            groceryGroup.MapDeleteGroceryItemEndpoint();
            groceryGroup.MapSearchGroceryItemsEndpoint();
            groceryGroup.MapImportGroceryItemsEndpoint();
            groceryGroup.MapExportGroceryItemsEndpoint();

            var inventoryGroup = app.MapGroup("inventory-transfers").WithTags("Inventory Transfers");
            inventoryGroup.MapCreateInventoryTransferEndpoint();
            inventoryGroup.MapGetInventoryTransferEndpoint();
            inventoryGroup.MapUpdateInventoryTransferEndpoint();
            inventoryGroup.MapDeleteInventoryTransferEndpoint();
            inventoryGroup.MapSearchInventoryTransfersEndpoint();
            inventoryGroup.MapAddInventoryTransferItemEndpoint();
            inventoryGroup.MapRemoveInventoryTransferItemEndpoint();
            inventoryGroup.MapUpdateInventoryTransferItemEndpoint();
            // lifecycle endpoints
            inventoryGroup.MapApproveInventoryTransferEndpoint();
            inventoryGroup.MapMarkInTransitInventoryTransferEndpoint();
            inventoryGroup.MapCompleteInventoryTransferEndpoint();
            inventoryGroup.MapCancelInventoryTransferEndpoint();

            var stockGroup = app.MapGroup("stock-adjustments").WithTags("Stock Adjustments");
            stockGroup.MapCreateStockAdjustmentEndpoint();
            stockGroup.MapGetStockAdjustmentEndpoint();
            stockGroup.MapUpdateStockAdjustmentEndpoint();
            stockGroup.MapDeleteStockAdjustmentEndpoint();
            stockGroup.MapApproveStockAdjustmentEndpoint();
            stockGroup.MapSearchStockAdjustmentsEndpoint();

            var warehouses = app.MapGroup("warehouses").WithTags("Warehouses");
            warehouses.MapCreateWarehouseEndpoint();
            warehouses.MapGetWarehouseEndpoint();
            warehouses.MapUpdateWarehouseEndpoint();
            warehouses.MapDeleteWarehouseEndpoint();
            warehouses.MapSearchWarehousesEndpoint();

            var whLocations = app.MapGroup("warehouse-locations").WithTags("Warehouse Locations");
            whLocations.MapCreateWarehouseLocationEndpoint();
            whLocations.MapGetWarehouseLocationEndpoint();
            whLocations.MapUpdateWarehouseLocationEndpoint();
            whLocations.MapDeleteWarehouseLocationEndpoint();
            whLocations.MapSearchWarehouseLocationsEndpoint();

            var sales = app.MapGroup("sales-orders").WithTags("Sales Orders");
            sales.MapCreateSalesOrderEndpoint();
            sales.MapGetSalesOrderEndpoint();
            sales.MapUpdateSalesOrderEndpoint();
            sales.MapDeleteSalesOrderEndpoint();
            sales.MapSearchSalesOrdersEndpoint();

            // Purchase Orders endpoints
            var purchaseOrders = app.MapGroup("purchase-orders").WithTags("Purchase Orders");
            purchaseOrders.MapCreatePurchaseOrderEndpoint();
            purchaseOrders.MapGetPurchaseOrderEndpoint();
            purchaseOrders.MapUpdatePurchaseOrderEndpoint();
            purchaseOrders.MapDeletePurchaseOrderEndpoint();
            purchaseOrders.MapSearchPurchaseOrdersEndpoint();
            
            // Purchase Order Workflow endpoints
            purchaseOrders.MapSubmitPurchaseOrderEndpoint();
            purchaseOrders.MapApprovePurchaseOrderEndpoint();
            purchaseOrders.MapSendPurchaseOrderEndpoint();
            purchaseOrders.MapReceivePurchaseOrderEndpoint();
            purchaseOrders.MapCancelPurchaseOrderEndpoint();

            // Purchase Order Items endpoints
            purchaseOrders.MapGetPurchaseOrderItemsEndpoint();
            purchaseOrders.MapAddPurchaseOrderItemEndpoint();
            purchaseOrders.MapUpdatePurchaseOrderItemQuantityEndpoint();
            purchaseOrders.MapUpdatePurchaseOrderItemPriceEndpoint();
            purchaseOrders.MapReceivePurchaseOrderItemQuantityEndpoint();
            purchaseOrders.MapRemovePurchaseOrderItemEndpoint();

            // Price Lists endpoints
            var priceLists = app.MapGroup("price-lists").WithTags("Price Lists");
            priceLists.MapCreatePriceListEndpoint();
            priceLists.MapGetPriceListEndpoint();
            priceLists.MapUpdatePriceListEndpoint();
            priceLists.MapDeletePriceListEndpoint();
            priceLists.MapSearchPriceListsEndpoint();

            var categories = app.MapGroup("categories").WithTags("Categories");
            categories.MapCreateCategoryEndpoint();
            categories.MapGetCategoryEndpoint();
            categories.MapUpdateCategoryEndpoint();
            categories.MapDeleteCategoryEndpoint();
            categories.MapSearchCategoriesEndpoint();

            var cycleCounts = app.MapGroup("cycle-counts").WithTags("Cycle Counts");
            cycleCounts.MapCreateCycleCountEndpoint();
            cycleCounts.MapStartCycleCountEndpoint();
            cycleCounts.MapAddCycleCountItemEndpoint();
            cycleCounts.MapCompleteCycleCountEndpoint();
            cycleCounts.MapReconcileCycleCountEndpoint();

            // Supplier endpoints
            var suppliers = app.MapGroup("suppliers").WithTags("Suppliers");
            suppliers.MapCreateSupplierEndpoint();
            suppliers.MapGetSupplierEndpoint();
            suppliers.MapUpdateSupplierEndpoint();
            suppliers.MapDeleteSupplierEndpoint();
            suppliers.MapSearchSuppliersEndpoint();
            suppliers.MapActivateSupplierEndpoint();
            suppliers.MapDeactivateSupplierEndpoint();

            // Customer endpoints
            var customers = app.MapGroup("customers").WithTags("Customers");
            customers.MapCreateCustomerEndpoint();
            customers.MapGetCustomerEndpoint();
            customers.MapUpdateCustomerEndpoint();
            customers.MapDeleteCustomerEndpoint();
            customers.MapSearchCustomersEndpoint();
            customers.MapActivateCustomerEndpoint();
            customers.MapDeactivateCustomerEndpoint();
            customers.MapChangeCustomerCreditLimitEndpoint();
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

        builder.Services.AddKeyedScoped<IRepository<Category>, StoreRepository<Category>>("store:categories");
        builder.Services.AddKeyedScoped<IReadRepository<Category>, StoreRepository<Category>>("store:categories");

        builder.Services.AddKeyedScoped<IRepository<Supplier>, StoreRepository<Supplier>>("store:suppliers");
        builder.Services.AddKeyedScoped<IReadRepository<Supplier>, StoreRepository<Supplier>>("store:suppliers");

        // Register keyed repository for CycleCounts
        builder.Services.AddKeyedScoped<IRepository<CycleCount>, StoreRepository<CycleCount>>("store:cycle-counts");
        builder.Services.AddKeyedScoped<IReadRepository<CycleCount>, StoreRepository<CycleCount>>("store:cycle-counts");

        // Additional common entity registrations (purchase orders, sales orders, price lists)
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store:purchase-orders");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store:purchase-orders");
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>("store:purchase-order-items");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>("store:purchase-order-items");

        builder.Services.AddKeyedScoped<IRepository<SalesOrder>, StoreRepository<SalesOrder>>("store:sales-orders");
        builder.Services.AddKeyedScoped<IReadRepository<SalesOrder>, StoreRepository<SalesOrder>>("store:sales-orders");

        builder.Services.AddKeyedScoped<IRepository<PriceList>, StoreRepository<PriceList>>("store:price-lists");
        builder.Services.AddKeyedScoped<IReadRepository<PriceList>, StoreRepository<PriceList>>("store:price-lists");

        // Register wholesale contracts repository
        builder.Services.AddKeyedScoped<IRepository<WholesaleContract>, StoreRepository<WholesaleContract>>("store:wholesale-contracts");
        builder.Services.AddKeyedScoped<IReadRepository<WholesaleContract>, StoreRepository<WholesaleContract>>("store:wholesale-contracts");

        // Register wholesale pricings repository
        builder.Services.AddKeyedScoped<IRepository<WholesalePricing>, StoreRepository<WholesalePricing>>("store:wholesale-pricings");
        builder.Services.AddKeyedScoped<IReadRepository<WholesalePricing>, StoreRepository<WholesalePricing>>("store:wholesale-pricings");

        // New: inventory transactions repository for guards and reporting
        builder.Services.AddKeyedScoped<IRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store:inventory-transactions");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store:inventory-transactions");

        // Import/Export: register grocery items import parser implementation
        builder.Services.AddScoped<IGroceryItemImportParser, GroceryItemImportParser>();

        return builder;
    }

    public static WebApplication UseStoreModule(this WebApplication app)
    {
        return app;
    }
}
