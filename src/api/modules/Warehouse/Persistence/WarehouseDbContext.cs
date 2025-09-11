using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Framework.Infrastructure.Tenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Constants;

namespace FSH.Starter.WebApi.Warehouse.Persistence;

public sealed class WarehouseDbContext(
    IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor,
    DbContextOptions<WarehouseDbContext> options,
    IPublisher publisher,
    IOptions<DatabaseOptions> settings) : FshDbContext(multiTenantContextAccessor, options, publisher, settings)
{
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Store> Stores { get; set; } = null!;
    public DbSet<Domain.Warehouse> Warehouses { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Supplier> Suppliers { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductBatch> ProductBatches { get; set; } = null!;
    public DbSet<SupplierProduct> SupplierProducts { get; set; } = null!;
    public DbSet<WarehouseInventory> WarehouseInventories { get; set; } = null!;
    public DbSet<StoreInventory> StoreInventories { get; set; } = null!;
    public DbSet<InventoryTransaction> InventoryTransactions { get; set; } = null!;
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
    public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = null!;
    public DbSet<StoreTransfer> StoreTransfers { get; set; } = null!;
    public DbSet<StoreTransferDetail> StoreTransferDetails { get; set; } = null!;
    public DbSet<Sale> Sales { get; set; } = null!;
    public DbSet<SaleDetail> SaleDetails { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<ProductMovementSummary> ProductMovementSummaries { get; set; } = null!;
    public DbSet<ExpiryAlert> ExpiryAlerts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WarehouseDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Warehouse);
    }
}
