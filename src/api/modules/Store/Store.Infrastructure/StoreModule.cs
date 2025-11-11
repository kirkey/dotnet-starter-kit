using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Report.v1.Services;
using Store.Domain.Entities;
using Store.Infrastructure.Services;
using Store.Infrastructure.Endpoints.Bins;
using Store.Infrastructure.Endpoints.Categories;
using Store.Infrastructure.Endpoints.CycleCounts;
using Store.Infrastructure.Endpoints.GoodsReceipts;
using Store.Infrastructure.Endpoints.InventoryReservations;
using Store.Infrastructure.Endpoints.InventoryTransactions;
using Store.Infrastructure.Endpoints.InventoryTransfers;
using Store.Infrastructure.Endpoints.Items;
using Store.Infrastructure.Endpoints.ItemSuppliers;
using Store.Infrastructure.Endpoints.LotNumbers;
using Store.Infrastructure.Endpoints.PickLists;
using Store.Infrastructure.Endpoints.PurchaseOrders;
using Store.Infrastructure.Endpoints.PutAwayTasks;
using Store.Infrastructure.Endpoints.SerialNumbers;
using Store.Infrastructure.Endpoints.StockAdjustments;
using Store.Infrastructure.Endpoints.StockLevels;
using Store.Infrastructure.Endpoints.Suppliers;
using Store.Infrastructure.Endpoints.WarehouseLocations;
using Store.Infrastructure.Endpoints.Warehouses;
using Store.Infrastructure.Endpoints.SalesImports;
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
        storeGroup.MapBinsEndpoints();
        storeGroup.MapCategoriesEndpoints();
        storeGroup.MapCycleCountsEndpoints();
        storeGroup.MapGoodsReceiptsEndpoints();
        storeGroup.MapInventoryReservationsEndpoints();
        storeGroup.MapInventoryTransactionsEndpoints();
        storeGroup.MapInventoryTransfersEndpoints();
        storeGroup.MapItemsEndpoints();
        storeGroup.MapItemSuppliersEndpoints();
        storeGroup.MapLotNumbersEndpoints();
        storeGroup.MapPickListsEndpoints();
        storeGroup.MapPutAwayTasksEndpoints();
        storeGroup.MapSerialNumbersEndpoints();
        storeGroup.MapPurchaseOrdersEndpoints();
        storeGroup.MapStockAdjustmentsEndpoints();
        storeGroup.MapStockLevelsEndpoints();
        storeGroup.MapSuppliersEndpoints();
        storeGroup.MapWarehouseLocationsEndpoints();
        storeGroup.MapWarehousesEndpoints();
        storeGroup.MapSalesImportsEndpoints();

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
        builder.Services.AddScoped<IRepository<Bin>, StoreRepository<Bin>>();
        builder.Services.AddScoped<IReadRepository<Bin>, StoreRepository<Bin>>();
        builder.Services.AddScoped<IRepository<Category>, StoreRepository<Category>>();
        builder.Services.AddScoped<IReadRepository<Category>, StoreRepository<Category>>();
        builder.Services.AddScoped<IRepository<Item>, StoreRepository<Item>>();
        builder.Services.AddScoped<IReadRepository<Item>, StoreRepository<Item>>();
        builder.Services.AddScoped<IRepository<ItemSupplier>, StoreRepository<ItemSupplier>>();
        builder.Services.AddScoped<IReadRepository<ItemSupplier>, StoreRepository<ItemSupplier>>();
        builder.Services.AddScoped<IRepository<LotNumber>, StoreRepository<LotNumber>>();
        builder.Services.AddScoped<IReadRepository<LotNumber>, StoreRepository<LotNumber>>();
        builder.Services.AddScoped<IRepository<SerialNumber>, StoreRepository<SerialNumber>>();
        builder.Services.AddScoped<IReadRepository<SerialNumber>, StoreRepository<SerialNumber>>();
        builder.Services.AddScoped<IRepository<CycleCount>, StoreRepository<CycleCount>>();
        builder.Services.AddScoped<IReadRepository<CycleCount>, StoreRepository<CycleCount>>();
        builder.Services.AddScoped<IRepository<CycleCountItem>, StoreRepository<CycleCountItem>>();
        builder.Services.AddScoped<IReadRepository<CycleCountItem>, StoreRepository<CycleCountItem>>();
        builder.Services.AddScoped<IRepository<GoodsReceipt>, StoreRepository<GoodsReceipt>>();
        builder.Services.AddScoped<IReadRepository<GoodsReceipt>, StoreRepository<GoodsReceipt>>();
        builder.Services.AddScoped<IRepository<PickList>, StoreRepository<PickList>>();
        builder.Services.AddScoped<IReadRepository<PickList>, StoreRepository<PickList>>();
        builder.Services.AddScoped<IRepository<PutAwayTask>, StoreRepository<PutAwayTask>>();
        builder.Services.AddScoped<IReadRepository<PutAwayTask>, StoreRepository<PutAwayTask>>();
        builder.Services.AddScoped<IRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>();
        builder.Services.AddScoped<IReadRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>();
        builder.Services.AddScoped<IRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>();
        builder.Services.AddScoped<IReadRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>();
        builder.Services.AddScoped<IRepository<InventoryReservation>, StoreRepository<InventoryReservation>>();
        builder.Services.AddScoped<IReadRepository<InventoryReservation>, StoreRepository<InventoryReservation>>();
        builder.Services.AddScoped<IRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>();
        builder.Services.AddScoped<IReadRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>();
        builder.Services.AddScoped<IRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>();
        builder.Services.AddScoped<IReadRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>();
        builder.Services.AddScoped<IRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>();
        builder.Services.AddScoped<IReadRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>();
        builder.Services.AddScoped<IRepository<StockAdjustment>, StoreRepository<StockAdjustment>>();
        builder.Services.AddScoped<IReadRepository<StockAdjustment>, StoreRepository<StockAdjustment>>();
        builder.Services.AddScoped<IRepository<StockLevel>, StoreRepository<StockLevel>>();
        builder.Services.AddScoped<IReadRepository<StockLevel>, StoreRepository<StockLevel>>();
        builder.Services.AddScoped<IRepository<Supplier>, StoreRepository<Supplier>>();
        builder.Services.AddScoped<IReadRepository<Supplier>, StoreRepository<Supplier>>();
        builder.Services.AddScoped<IRepository<Warehouse>, StoreRepository<Warehouse>>();
        builder.Services.AddScoped<IReadRepository<Warehouse>, StoreRepository<Warehouse>>();
        builder.Services.AddScoped<IRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>();
        builder.Services.AddScoped<IReadRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>();
        builder.Services.AddScoped<IRepository<SalesImport>, StoreRepository<SalesImport>>();
        builder.Services.AddScoped<IReadRepository<SalesImport>, StoreRepository<SalesImport>>();
        builder.Services.AddScoped<IRepository<SalesImportItem>, StoreRepository<SalesImportItem>>();
        builder.Services.AddScoped<IReadRepository<SalesImportItem>, StoreRepository<SalesImportItem>>();
        
        // Register with keyed services (for handlers that use specific keys)
        builder.Services.AddKeyedScoped<IRepository<Bin>, StoreRepository<Bin>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<Bin>, StoreRepository<Bin>>("store");
        builder.Services.AddKeyedScoped<IRepository<Category>, StoreRepository<Category>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<Category>, StoreRepository<Category>>("store");
        builder.Services.AddKeyedScoped<IRepository<Item>, StoreRepository<Item>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<Item>, StoreRepository<Item>>("store");
        builder.Services.AddKeyedScoped<IRepository<ItemSupplier>, StoreRepository<ItemSupplier>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<ItemSupplier>, StoreRepository<ItemSupplier>>("store");
        builder.Services.AddKeyedScoped<IRepository<LotNumber>, StoreRepository<LotNumber>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<LotNumber>, StoreRepository<LotNumber>>("store");
        builder.Services.AddKeyedScoped<IRepository<SerialNumber>, StoreRepository<SerialNumber>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<SerialNumber>, StoreRepository<SerialNumber>>("store");
        builder.Services.AddKeyedScoped<IRepository<CycleCount>, StoreRepository<CycleCount>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<CycleCount>, StoreRepository<CycleCount>>("store");
        builder.Services.AddKeyedScoped<IRepository<CycleCountItem>, StoreRepository<CycleCountItem>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<CycleCountItem>, StoreRepository<CycleCountItem>>("store");
        builder.Services.AddKeyedScoped<IRepository<GoodsReceipt>, StoreRepository<GoodsReceipt>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<GoodsReceipt>, StoreRepository<GoodsReceipt>>("store");
        builder.Services.AddKeyedScoped<IRepository<PickList>, StoreRepository<PickList>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<PickList>, StoreRepository<PickList>>("store");
        builder.Services.AddKeyedScoped<IRepository<PutAwayTask>, StoreRepository<PutAwayTask>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<PutAwayTask>, StoreRepository<PutAwayTask>>("store");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>("store");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>("store");
        builder.Services.AddKeyedScoped<IRepository<InventoryReservation>, StoreRepository<InventoryReservation>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryReservation>, StoreRepository<InventoryReservation>>("store");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store");
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store");
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>("store");
        builder.Services.AddKeyedScoped<IRepository<StockAdjustment>, StoreRepository<StockAdjustment>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<StockAdjustment>, StoreRepository<StockAdjustment>>("store");
        builder.Services.AddKeyedScoped<IRepository<StockLevel>, StoreRepository<StockLevel>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<StockLevel>, StoreRepository<StockLevel>>("store");
        builder.Services.AddKeyedScoped<IRepository<Supplier>, StoreRepository<Supplier>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<Supplier>, StoreRepository<Supplier>>("store");
        builder.Services.AddKeyedScoped<IRepository<Warehouse>, StoreRepository<Warehouse>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<Warehouse>, StoreRepository<Warehouse>>("store");
        builder.Services.AddKeyedScoped<IRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>("store");

        // Register with specific keys for different functional areas
        builder.Services.AddKeyedScoped<IRepository<Bin>, StoreRepository<Bin>>("store:bins");
        builder.Services.AddKeyedScoped<IReadRepository<Bin>, StoreRepository<Bin>>("store:bins");
        builder.Services.AddKeyedScoped<IRepository<Category>, StoreRepository<Category>>("store:categories");
        builder.Services.AddKeyedScoped<IReadRepository<Category>, StoreRepository<Category>>("store:categories");
        builder.Services.AddKeyedScoped<IRepository<Item>, StoreRepository<Item>>("store:items");
        builder.Services.AddKeyedScoped<IReadRepository<Item>, StoreRepository<Item>>("store:items");
        builder.Services.AddKeyedScoped<IRepository<ItemSupplier>, StoreRepository<ItemSupplier>>("store:itemsuppliers");
        builder.Services.AddKeyedScoped<IReadRepository<ItemSupplier>, StoreRepository<ItemSupplier>>("store:itemsuppliers");
        builder.Services.AddKeyedScoped<IRepository<StockLevel>, StoreRepository<StockLevel>>("store:stocklevels");
        builder.Services.AddKeyedScoped<IReadRepository<StockLevel>, StoreRepository<StockLevel>>("store:stocklevels");
        builder.Services.AddKeyedScoped<IRepository<StockLevel>, StoreRepository<StockLevel>>("store:stock-levels");
        builder.Services.AddKeyedScoped<IReadRepository<StockLevel>, StoreRepository<StockLevel>>("store:stock-levels");
        builder.Services.AddKeyedScoped<IRepository<LotNumber>, StoreRepository<LotNumber>>("store:lotnumbers");
        builder.Services.AddKeyedScoped<IReadRepository<LotNumber>, StoreRepository<LotNumber>>("store:lotnumbers");
        builder.Services.AddKeyedScoped<IRepository<SerialNumber>, StoreRepository<SerialNumber>>("store:serialnumbers");
        builder.Services.AddKeyedScoped<IReadRepository<SerialNumber>, StoreRepository<SerialNumber>>("store:serialnumbers");
        builder.Services.AddKeyedScoped<IRepository<CycleCount>, StoreRepository<CycleCount>>("store:cycle-counts");
        builder.Services.AddKeyedScoped<IReadRepository<CycleCount>, StoreRepository<CycleCount>>("store:cycle-counts");
        builder.Services.AddKeyedScoped<IRepository<CycleCountItem>, StoreRepository<CycleCountItem>>("store:cycle-count-items");
        builder.Services.AddKeyedScoped<IReadRepository<CycleCountItem>, StoreRepository<CycleCountItem>>("store:cycle-count-items");
        builder.Services.AddKeyedScoped<IRepository<GoodsReceipt>, StoreRepository<GoodsReceipt>>("store:goodsreceipts");
        builder.Services.AddKeyedScoped<IReadRepository<GoodsReceipt>, StoreRepository<GoodsReceipt>>("store:goodsreceipts");
        builder.Services.AddKeyedScoped<IRepository<PickList>, StoreRepository<PickList>>("store:picklists");
        builder.Services.AddKeyedScoped<IReadRepository<PickList>, StoreRepository<PickList>>("store:picklists");
        builder.Services.AddKeyedScoped<IRepository<PickListItem>, StoreRepository<PickListItem>>("store:picklistitems");
        builder.Services.AddKeyedScoped<IReadRepository<PickListItem>, StoreRepository<PickListItem>>("store:picklistitems");
        builder.Services.AddKeyedScoped<IRepository<PutAwayTask>, StoreRepository<PutAwayTask>>("store:putawaytasks");
        builder.Services.AddKeyedScoped<IReadRepository<PutAwayTask>, StoreRepository<PutAwayTask>>("store:putawaytasks");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>("store:inventory-transfers");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>("store:inventory-transfers");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>("store:inventory-transfer-items");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>("store:inventory-transfer-items");
        builder.Services.AddKeyedScoped<IRepository<InventoryReservation>, StoreRepository<InventoryReservation>>("store:inventoryreservations");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryReservation>, StoreRepository<InventoryReservation>>("store:inventoryreservations");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store:inventory-transactions");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store:inventory-transactions");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store:inventorytransactions");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store:inventorytransactions");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store:inventory-transactions");
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store:purchase-orders");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store:purchase-orders");
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>("store:purchase-order-items");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>("store:purchase-order-items");
        builder.Services.AddKeyedScoped<IRepository<StockAdjustment>, StoreRepository<StockAdjustment>>("store:stock-adjustments");
        builder.Services.AddKeyedScoped<IReadRepository<StockAdjustment>, StoreRepository<StockAdjustment>>("store:stock-adjustments");
        builder.Services.AddKeyedScoped<IRepository<Supplier>, StoreRepository<Supplier>>("store:suppliers");
        builder.Services.AddKeyedScoped<IReadRepository<Supplier>, StoreRepository<Supplier>>("store:suppliers");
        builder.Services.AddKeyedScoped<IRepository<Warehouse>, StoreRepository<Warehouse>>("store:warehouses");
        builder.Services.AddKeyedScoped<IReadRepository<Warehouse>, StoreRepository<Warehouse>>("store:warehouses");
        builder.Services.AddKeyedScoped<IRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>("store:warehouse-locations");
        builder.Services.AddKeyedScoped<IReadRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>("store:warehouse-locations");
        builder.Services.AddKeyedScoped<IRepository<SalesImport>, StoreRepository<SalesImport>>("store:sales-imports");
        builder.Services.AddKeyedScoped<IReadRepository<SalesImport>, StoreRepository<SalesImport>>("store:sales-imports");
        builder.Services.AddKeyedScoped<IRepository<SalesImportItem>, StoreRepository<SalesImportItem>>("store:sales-import-items");
        builder.Services.AddKeyedScoped<IReadRepository<SalesImportItem>, StoreRepository<SalesImportItem>>("store:sales-import-items");

        // Register PDF service for Purchase Orders
        builder.Services.AddScoped<IPurchaseOrderPdfService, PurchaseOrderPdfService>();

        return builder;
    }

    public static WebApplication UseStoreModule(this WebApplication app)
    {
        return app;
    }
}
