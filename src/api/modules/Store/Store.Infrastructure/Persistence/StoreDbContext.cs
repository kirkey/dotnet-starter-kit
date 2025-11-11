using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Infrastructure.Tenant;
using Microsoft.Extensions.Options;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence;

public sealed class StoreDbContext(
    IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor,
    DbContextOptions<StoreDbContext> options,
    IPublisher publisher,
    IOptions<DatabaseOptions> settings) : FshDbContext(multiTenantContextAccessor, options, publisher, settings)
{
    // Core entities
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Supplier> Suppliers { get; set; } = null!;
    
    // Warehouse entities
    public DbSet<Warehouse> Warehouses { get; set; } = null!;
    public DbSet<WarehouseLocation> WarehouseLocations { get; set; } = null!;
    public DbSet<Bin> Bins { get; set; } = null!;
    
    // Inventory tracking
    public DbSet<StockLevel> StockLevels { get; set; } = null!;
    public DbSet<LotNumber> LotNumbers { get; set; } = null!;
    public DbSet<SerialNumber> SerialNumbers { get; set; } = null!;
    public DbSet<InventoryReservation> InventoryReservations { get; set; } = null!;
    
    // Purchasing
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
    public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; } = null!;
    public DbSet<ItemSupplier> ItemSuppliers { get; set; } = null!;
    
    // Warehouse operations
    public DbSet<PickList> PickLists { get; set; } = null!;
    public DbSet<PickListItem> PickListItems { get; set; } = null!;
    public DbSet<PutAwayTask> PutAwayTasks { get; set; } = null!;
    public DbSet<PutAwayTaskItem> PutAwayTaskItems { get; set; } = null!;
    public DbSet<GoodsReceipt> GoodsReceipts { get; set; } = null!;
    public DbSet<GoodsReceiptItem> GoodsReceiptItems { get; set; } = null!;
    
    // Inventory management
    public DbSet<InventoryTransaction> InventoryTransactions { get; set; } = null!;
    public DbSet<InventoryTransfer> InventoryTransfers { get; set; } = null!;
    public DbSet<InventoryTransferItem> InventoryTransferItems { get; set; } = null!;
    public DbSet<StockAdjustment> StockAdjustments { get; set; } = null!;
    public DbSet<CycleCount> CycleCounts { get; set; } = null!;
    public DbSet<CycleCountItem> CycleCountItems { get; set; } = null!;
    
    // Sales imports
    public DbSet<SalesImport> SalesImports { get; set; } = null!;
    public DbSet<SalesImportItem> SalesImportItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly);

        // Configure schema using a shared constant
        modelBuilder.HasDefaultSchema(SchemaNames.Store);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<decimal>().HavePrecision(16, 2);
    }
}
