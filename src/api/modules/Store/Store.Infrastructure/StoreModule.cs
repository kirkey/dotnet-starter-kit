using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Report.v1.Services;
using Store.Domain.Entities;
using Store.Infrastructure.Services;
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
    /// All endpoints are auto-discovered by Carter via ICarterModule implementations.
    /// This method is kept for backward compatibility but can be empty.
    /// </summary>
    /// <param name="app">The endpoint route builder to configure.</param>
    /// <returns>The configured endpoint route builder.</returns>
    public static IEndpointRouteBuilder MapStoreEndpoints(this IEndpointRouteBuilder app)
    {
        // All store endpoints are now auto-discovered by Carter via ICarterModule implementations.
        // No manual endpoint mapping is required.
        // Individual endpoint classes implement ICarterModule and are automatically registered.
        return app;
    }

    /// <summary>
    /// Registers store services with dependency injection container.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The configured web application builder.</returns>
    /// <remarks>
    /// This method registers all store repositories and services in a highly organized structure:
    /// 
    /// 1. CORE SERVICES - DbContext, initializers, and business services
    /// 
    /// 2. NON-KEYED REPOSITORIES - Standard DI registrations (25 aggregate root entities)
    ///    Used by MediatR handlers that don't use [FromKeyedServices] attribute
    /// 
    /// 3. KEYED REPOSITORIES - Keyed DI registrations with various keys:
    ///    - "store" - Generic key for basic handlers  
    ///    - "store:{entity}" - Specific keys for specialized handlers
    ///    
    /// Entity Groups:
    /// - Master Data: Category, Item, ItemSupplier, Supplier
    /// - Warehouse Management: Bin, Warehouse, WarehouseLocation
    /// - Inventory Control: StockLevel, StockAdjustment, InventoryTransaction, InventoryReservation
    /// - Traceability: LotNumber, SerialNumber
    /// - Receiving: GoodsReceipt, PutAwayTask
    /// - Picking/Shipping: PickList, PickListItem
    /// - Inventory Counting: CycleCount, CycleCountItem
    /// - Transfers: InventoryTransfer, InventoryTransferItem
    /// - Purchasing: PurchaseOrder, PurchaseOrderItem
    /// - Sales Integration: SalesImport, SalesImportItem
    /// 
    /// Total: ~200+ repository registrations supporting 25 aggregate root entities
    /// </remarks>
    public static WebApplicationBuilder RegisterStoreServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        
        // ============================================================================
        // CORE SERVICES
        // ============================================================================
        builder.Services.BindDbContext<StoreDbContext>();
        builder.Services.AddScoped<IDbInitializer, StoreDbInitializer>();
        builder.Services.AddScoped<IPurchaseOrderPdfService, PurchaseOrderPdfService>();
    
        // ============================================================================
        // NON-KEYED REPOSITORY REGISTRATIONS (for MediatR handlers without keyed services)
        // Organized by functional area - 25 aggregate root entities total
        // ============================================================================
        
        // --- Master Data ---
        builder.Services.AddScoped<IRepository<Category>, StoreRepository<Category>>();
        builder.Services.AddScoped<IReadRepository<Category>, StoreRepository<Category>>();
        builder.Services.AddScoped<IRepository<Item>, StoreRepository<Item>>();
        builder.Services.AddScoped<IReadRepository<Item>, StoreRepository<Item>>();
        builder.Services.AddScoped<IRepository<ItemSupplier>, StoreRepository<ItemSupplier>>();
        builder.Services.AddScoped<IReadRepository<ItemSupplier>, StoreRepository<ItemSupplier>>();
        builder.Services.AddScoped<IRepository<Supplier>, StoreRepository<Supplier>>();
        builder.Services.AddScoped<IReadRepository<Supplier>, StoreRepository<Supplier>>();
        
        // --- Warehouse Management ---
        builder.Services.AddScoped<IRepository<Bin>, StoreRepository<Bin>>();
        builder.Services.AddScoped<IReadRepository<Bin>, StoreRepository<Bin>>();
        builder.Services.AddScoped<IRepository<Warehouse>, StoreRepository<Warehouse>>();
        builder.Services.AddScoped<IReadRepository<Warehouse>, StoreRepository<Warehouse>>();
        builder.Services.AddScoped<IRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>();
        builder.Services.AddScoped<IReadRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>();
        
        // --- Inventory Control ---
        builder.Services.AddScoped<IRepository<InventoryReservation>, StoreRepository<InventoryReservation>>();
        builder.Services.AddScoped<IReadRepository<InventoryReservation>, StoreRepository<InventoryReservation>>();
        builder.Services.AddScoped<IRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>();
        builder.Services.AddScoped<IReadRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>();
        builder.Services.AddScoped<IRepository<StockAdjustment>, StoreRepository<StockAdjustment>>();
        builder.Services.AddScoped<IReadRepository<StockAdjustment>, StoreRepository<StockAdjustment>>();
        builder.Services.AddScoped<IRepository<StockLevel>, StoreRepository<StockLevel>>();
        builder.Services.AddScoped<IReadRepository<StockLevel>, StoreRepository<StockLevel>>();
        
        // --- Traceability ---
        builder.Services.AddScoped<IRepository<LotNumber>, StoreRepository<LotNumber>>();
        builder.Services.AddScoped<IReadRepository<LotNumber>, StoreRepository<LotNumber>>();
        builder.Services.AddScoped<IRepository<SerialNumber>, StoreRepository<SerialNumber>>();
        builder.Services.AddScoped<IReadRepository<SerialNumber>, StoreRepository<SerialNumber>>();
        
        // --- Receiving ---
        builder.Services.AddScoped<IRepository<GoodsReceipt>, StoreRepository<GoodsReceipt>>();
        builder.Services.AddScoped<IReadRepository<GoodsReceipt>, StoreRepository<GoodsReceipt>>();
        builder.Services.AddScoped<IRepository<PutAwayTask>, StoreRepository<PutAwayTask>>();
        builder.Services.AddScoped<IReadRepository<PutAwayTask>, StoreRepository<PutAwayTask>>();
        
        // --- Picking/Shipping ---
        builder.Services.AddScoped<IRepository<PickList>, StoreRepository<PickList>>();
        builder.Services.AddScoped<IReadRepository<PickList>, StoreRepository<PickList>>();
        builder.Services.AddScoped<IRepository<PickListItem>, StoreRepository<PickListItem>>();
        builder.Services.AddScoped<IReadRepository<PickListItem>, StoreRepository<PickListItem>>();
        
        // --- Inventory Counting ---
        builder.Services.AddScoped<IRepository<CycleCount>, StoreRepository<CycleCount>>();
        builder.Services.AddScoped<IReadRepository<CycleCount>, StoreRepository<CycleCount>>();
        builder.Services.AddScoped<IRepository<CycleCountItem>, StoreRepository<CycleCountItem>>();
        builder.Services.AddScoped<IReadRepository<CycleCountItem>, StoreRepository<CycleCountItem>>();
        
        // --- Inventory Transfers ---
        builder.Services.AddScoped<IRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>();
        builder.Services.AddScoped<IReadRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>();
        builder.Services.AddScoped<IRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>();
        builder.Services.AddScoped<IReadRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>();
        
        // --- Purchasing ---
        builder.Services.AddScoped<IRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>();
        builder.Services.AddScoped<IReadRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>();
        builder.Services.AddScoped<IRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>();
        builder.Services.AddScoped<IReadRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>();
        
        // --- Sales Integration ---
        builder.Services.AddScoped<IRepository<SalesImport>, StoreRepository<SalesImport>>();
        builder.Services.AddScoped<IReadRepository<SalesImport>, StoreRepository<SalesImport>>();
        builder.Services.AddScoped<IRepository<SalesImportItem>, StoreRepository<SalesImportItem>>();
        builder.Services.AddScoped<IReadRepository<SalesImportItem>, StoreRepository<SalesImportItem>>();
        
        // ============================================================================
        // KEYED REPOSITORY REGISTRATIONS - GENERIC "store" KEY
        // For handlers that use [FromKeyedServices("store")]
        // Organized by functional area - 25 aggregate root entities total
        // ============================================================================
        
        // --- Master Data ---
        builder.Services.AddKeyedScoped<IRepository<Category>, StoreRepository<Category>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<Category>, StoreRepository<Category>>("store");
        builder.Services.AddKeyedScoped<IRepository<Item>, StoreRepository<Item>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<Item>, StoreRepository<Item>>("store");
        builder.Services.AddKeyedScoped<IRepository<ItemSupplier>, StoreRepository<ItemSupplier>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<ItemSupplier>, StoreRepository<ItemSupplier>>("store");
        builder.Services.AddKeyedScoped<IRepository<Supplier>, StoreRepository<Supplier>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<Supplier>, StoreRepository<Supplier>>("store");
        
        // --- Warehouse Management ---
        builder.Services.AddKeyedScoped<IRepository<Bin>, StoreRepository<Bin>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<Bin>, StoreRepository<Bin>>("store");
        builder.Services.AddKeyedScoped<IRepository<Warehouse>, StoreRepository<Warehouse>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<Warehouse>, StoreRepository<Warehouse>>("store");
        builder.Services.AddKeyedScoped<IRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>("store");
        
        // --- Inventory Control ---
        builder.Services.AddKeyedScoped<IRepository<InventoryReservation>, StoreRepository<InventoryReservation>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryReservation>, StoreRepository<InventoryReservation>>("store");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store");
        builder.Services.AddKeyedScoped<IRepository<StockAdjustment>, StoreRepository<StockAdjustment>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<StockAdjustment>, StoreRepository<StockAdjustment>>("store");
        builder.Services.AddKeyedScoped<IRepository<StockLevel>, StoreRepository<StockLevel>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<StockLevel>, StoreRepository<StockLevel>>("store");
        
        // --- Traceability ---
        builder.Services.AddKeyedScoped<IRepository<LotNumber>, StoreRepository<LotNumber>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<LotNumber>, StoreRepository<LotNumber>>("store");
        builder.Services.AddKeyedScoped<IRepository<SerialNumber>, StoreRepository<SerialNumber>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<SerialNumber>, StoreRepository<SerialNumber>>("store");
        
        // --- Receiving ---
        builder.Services.AddKeyedScoped<IRepository<GoodsReceipt>, StoreRepository<GoodsReceipt>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<GoodsReceipt>, StoreRepository<GoodsReceipt>>("store");
        builder.Services.AddKeyedScoped<IRepository<PutAwayTask>, StoreRepository<PutAwayTask>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<PutAwayTask>, StoreRepository<PutAwayTask>>("store");
        
        // --- Picking/Shipping ---
        builder.Services.AddKeyedScoped<IRepository<PickList>, StoreRepository<PickList>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<PickList>, StoreRepository<PickList>>("store");
        builder.Services.AddKeyedScoped<IRepository<PickListItem>, StoreRepository<PickListItem>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<PickListItem>, StoreRepository<PickListItem>>("store");
        
        // --- Inventory Counting ---
        builder.Services.AddKeyedScoped<IRepository<CycleCount>, StoreRepository<CycleCount>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<CycleCount>, StoreRepository<CycleCount>>("store");
        builder.Services.AddKeyedScoped<IRepository<CycleCountItem>, StoreRepository<CycleCountItem>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<CycleCountItem>, StoreRepository<CycleCountItem>>("store");
        
        // --- Inventory Transfers ---
        builder.Services.AddKeyedScoped<IRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>("store");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>("store");
        
        // --- Purchasing ---
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store");
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>("store");
        
        // --- Sales Integration ---
        builder.Services.AddKeyedScoped<IRepository<SalesImport>, StoreRepository<SalesImport>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<SalesImport>, StoreRepository<SalesImport>>("store");
        builder.Services.AddKeyedScoped<IRepository<SalesImportItem>, StoreRepository<SalesImportItem>>("store");
        builder.Services.AddKeyedScoped<IReadRepository<SalesImportItem>, StoreRepository<SalesImportItem>>("store");

        // ============================================================================
        // KEYED REPOSITORY REGISTRATIONS - SPECIFIC "store:{entity}" KEYS
        // For handlers that use specific keyed services
        // Organized by functional area
        // ============================================================================
        
        // --- Master Data ---
        builder.Services.AddKeyedScoped<IRepository<Category>, StoreRepository<Category>>("store:categories");
        builder.Services.AddKeyedScoped<IReadRepository<Category>, StoreRepository<Category>>("store:categories");
        builder.Services.AddKeyedScoped<IRepository<Item>, StoreRepository<Item>>("store:items");
        builder.Services.AddKeyedScoped<IReadRepository<Item>, StoreRepository<Item>>("store:items");
        builder.Services.AddKeyedScoped<IRepository<ItemSupplier>, StoreRepository<ItemSupplier>>("store:item-suppliers");
        builder.Services.AddKeyedScoped<IReadRepository<ItemSupplier>, StoreRepository<ItemSupplier>>("store:item-suppliers");
        builder.Services.AddKeyedScoped<IRepository<Supplier>, StoreRepository<Supplier>>("store:suppliers");
        builder.Services.AddKeyedScoped<IReadRepository<Supplier>, StoreRepository<Supplier>>("store:suppliers");
        
        // --- Warehouse Management ---
        builder.Services.AddKeyedScoped<IRepository<Bin>, StoreRepository<Bin>>("store:bins");
        builder.Services.AddKeyedScoped<IReadRepository<Bin>, StoreRepository<Bin>>("store:bins");
        builder.Services.AddKeyedScoped<IRepository<Warehouse>, StoreRepository<Warehouse>>("store:warehouses");
        builder.Services.AddKeyedScoped<IReadRepository<Warehouse>, StoreRepository<Warehouse>>("store:warehouses");
        builder.Services.AddKeyedScoped<IRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>("store:warehouse-locations");
        builder.Services.AddKeyedScoped<IReadRepository<WarehouseLocation>, StoreRepository<WarehouseLocation>>("store:warehouse-locations");
        
        // --- Inventory Control ---
        builder.Services.AddKeyedScoped<IRepository<InventoryReservation>, StoreRepository<InventoryReservation>>("store:inventory-reservations");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryReservation>, StoreRepository<InventoryReservation>>("store:inventory-reservations");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store:inventory-transactions");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransaction>, StoreRepository<InventoryTransaction>>("store:inventory-transactions");
        builder.Services.AddKeyedScoped<IRepository<StockAdjustment>, StoreRepository<StockAdjustment>>("store:stock-adjustments");
        builder.Services.AddKeyedScoped<IReadRepository<StockAdjustment>, StoreRepository<StockAdjustment>>("store:stock-adjustments");
        builder.Services.AddKeyedScoped<IRepository<StockLevel>, StoreRepository<StockLevel>>("store:stock-levels");
        builder.Services.AddKeyedScoped<IReadRepository<StockLevel>, StoreRepository<StockLevel>>("store:stock-levels");
        
        // --- Traceability ---
        builder.Services.AddKeyedScoped<IRepository<LotNumber>, StoreRepository<LotNumber>>("store:lot-numbers");
        builder.Services.AddKeyedScoped<IReadRepository<LotNumber>, StoreRepository<LotNumber>>("store:lot-numbers");
        builder.Services.AddKeyedScoped<IRepository<SerialNumber>, StoreRepository<SerialNumber>>("store:serial-numbers");
        builder.Services.AddKeyedScoped<IReadRepository<SerialNumber>, StoreRepository<SerialNumber>>("store:serial-numbers");
        
        // --- Receiving ---
        builder.Services.AddKeyedScoped<IRepository<GoodsReceipt>, StoreRepository<GoodsReceipt>>("store:goods-receipts");
        builder.Services.AddKeyedScoped<IReadRepository<GoodsReceipt>, StoreRepository<GoodsReceipt>>("store:goods-receipts");
        builder.Services.AddKeyedScoped<IRepository<PutAwayTask>, StoreRepository<PutAwayTask>>("store:put-away-tasks");
        builder.Services.AddKeyedScoped<IReadRepository<PutAwayTask>, StoreRepository<PutAwayTask>>("store:put-away-tasks");
        
        // --- Picking/Shipping ---
        builder.Services.AddKeyedScoped<IRepository<PickList>, StoreRepository<PickList>>("store:pick-lists");
        builder.Services.AddKeyedScoped<IReadRepository<PickList>, StoreRepository<PickList>>("store:pick-lists");
        builder.Services.AddKeyedScoped<IRepository<PickListItem>, StoreRepository<PickListItem>>("store:pick-list-items");
        builder.Services.AddKeyedScoped<IReadRepository<PickListItem>, StoreRepository<PickListItem>>("store:pick-list-items");
        
        // --- Inventory Counting ---
        builder.Services.AddKeyedScoped<IRepository<CycleCount>, StoreRepository<CycleCount>>("store:cycle-counts");
        builder.Services.AddKeyedScoped<IReadRepository<CycleCount>, StoreRepository<CycleCount>>("store:cycle-counts");
        builder.Services.AddKeyedScoped<IRepository<CycleCountItem>, StoreRepository<CycleCountItem>>("store:cycle-count-items");
        builder.Services.AddKeyedScoped<IReadRepository<CycleCountItem>, StoreRepository<CycleCountItem>>("store:cycle-count-items");
        
        // --- Inventory Transfers ---
        builder.Services.AddKeyedScoped<IRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>("store:inventory-transfers");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransfer>, StoreRepository<InventoryTransfer>>("store:inventory-transfers");
        builder.Services.AddKeyedScoped<IRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>("store:inventory-transfer-items");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransferItem>, StoreRepository<InventoryTransferItem>>("store:inventory-transfer-items");
        
        // --- Purchasing ---
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store:purchase-orders");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrder>, StoreRepository<PurchaseOrder>>("store:purchase-orders");
        builder.Services.AddKeyedScoped<IRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>("store:purchase-order-items");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrderItem>, StoreRepository<PurchaseOrderItem>>("store:purchase-order-items");
        
        // --- Sales Integration ---
        builder.Services.AddKeyedScoped<IRepository<SalesImport>, StoreRepository<SalesImport>>("store:sales-imports");
        builder.Services.AddKeyedScoped<IReadRepository<SalesImport>, StoreRepository<SalesImport>>("store:sales-imports");
        builder.Services.AddKeyedScoped<IRepository<SalesImportItem>, StoreRepository<SalesImportItem>>("store:sales-import-items");
        builder.Services.AddKeyedScoped<IReadRepository<SalesImportItem>, StoreRepository<SalesImportItem>>("store:sales-import-items");

        return builder;
    }

    public static WebApplication UseStoreModule(this WebApplication app)
    {
        return app;
    }
}
