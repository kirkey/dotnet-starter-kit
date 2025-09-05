using Carter;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using FSH.Starter.WebApi.Warehouse.Features.Create.v1;
using FSH.Starter.WebApi.Warehouse.Features.Get.v1;
using FSH.Starter.WebApi.Warehouse.Features.GetList.v1;
using FSH.Starter.WebApi.Warehouse.Features.Update.v1;
using FSH.Starter.WebApi.Warehouse.Features.Delete.v1;
using FSH.Starter.WebApi.Warehouse.Features.StockMovement.v1;
using FSH.Starter.WebApi.Warehouse.Features.Inventory.v1;
using FSH.Starter.WebApi.Warehouse.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Warehouse;

public static class WarehouseModule
{
    public class Endpoints : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var warehouseGroup = app.MapGroup("warehouses").WithTags("warehouses");
            
            // Warehouse management endpoints
            warehouseGroup.MapWarehouseCreateEndpoint();
            warehouseGroup.MapGetWarehouseEndpoint();
            warehouseGroup.MapGetWarehouseListEndpoint();
            warehouseGroup.MapWarehouseUpdateEndpoint();
            warehouseGroup.MapWarehouseDeleteEndpoint();
            
            // Stock movement endpoints
            warehouseGroup.MapStockMovementCreateEndpoint();
            warehouseGroup.MapConfirmStockMovementEndpoint();
            warehouseGroup.MapGetStockMovementListEndpoint();
            
            // Inventory endpoints
            warehouseGroup.MapGetInventoryListEndpoint();
        }
    }

    public static WebApplicationBuilder RegisterWarehouseServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Register DbContext
        builder.Services.BindDbContext<WarehouseDbContext>();
        
        // Register database initializer
        builder.Services.AddScoped<IDbInitializer, WarehouseDbInitializer>();
        
        // Register repositories
        builder.Services.AddKeyedScoped<IRepository<Domain.Warehouse>, WarehouseRepository<Domain.Warehouse>>("warehouse");
        builder.Services.AddKeyedScoped<IReadRepository<Domain.Warehouse>, WarehouseRepository<Domain.Warehouse>>("warehouse");
        
        builder.Services.AddKeyedScoped<IRepository<StockMovement>, WarehouseRepository<StockMovement>>("warehouse");
        builder.Services.AddKeyedScoped<IReadRepository<StockMovement>, WarehouseRepository<StockMovement>>("warehouse");
        
        builder.Services.AddKeyedScoped<IRepository<InventoryItem>, WarehouseRepository<InventoryItem>>("warehouse");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryItem>, WarehouseRepository<InventoryItem>>("warehouse");

        // Register handlers
        builder.Services.AddScoped<CreateWarehouseHandler>();
        builder.Services.AddScoped<GetWarehouseHandler>();
        builder.Services.AddScoped<GetWarehouseListHandler>();
        builder.Services.AddScoped<UpdateWarehouseHandler>();
        builder.Services.AddScoped<DeleteWarehouseHandler>();
        builder.Services.AddScoped<CreateStockMovementHandler>();
        builder.Services.AddScoped<ConfirmStockMovementHandler>();
        builder.Services.AddScoped<GetStockMovementListHandler>();
        builder.Services.AddScoped<GetInventoryListHandler>();

        return builder;
    }

    public static WebApplication UseWarehouseModule(this WebApplication app)
    {
        return app;
    }
}
