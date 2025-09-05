using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Framework.Infrastructure.Tenant;
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
    public DbSet<Domain.Warehouse> Warehouses { get; set; } = null!;
    public DbSet<Domain.InventoryItem> InventoryItems { get; set; } = null!;
    public DbSet<Domain.StockMovement> StockMovements { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WarehouseDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Warehouse);
    }
}
