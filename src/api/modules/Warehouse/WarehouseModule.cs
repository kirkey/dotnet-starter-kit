using Carter;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using FSH.Starter.WebApi.Warehouse.Features.Warehouses.Create.v1;
using FSH.Starter.WebApi.Warehouse.Features.Warehouses.Update.v1;
using FSH.Starter.WebApi.Warehouse.Features.Companies.Create.v1;
using FSH.Starter.WebApi.Warehouse.Features.Companies.Update.v1;
using FSH.Starter.WebApi.Warehouse.Features.Stores.Create.v1;
using FSH.Starter.WebApi.Warehouse.Features.Stores.Update.v1;
using FSH.Starter.WebApi.Warehouse.Features.Categories.Create.v1;
using FSH.Starter.WebApi.Warehouse.Features.Categories.Update.v1;
using FSH.Starter.WebApi.Warehouse.Features.Suppliers.Create.v1;
using FSH.Starter.WebApi.Warehouse.Features.Suppliers.Update.v1;
using FSH.Starter.WebApi.Warehouse.Features.Products.Create.v1;
using FSH.Starter.WebApi.Warehouse.Features.Products.Update.v1;
using FSH.Starter.WebApi.Warehouse.Features.Customers.Create.v1;
using FSH.Starter.WebApi.Warehouse.Features.Customers.Update.v1;
using FSH.Starter.WebApi.Warehouse.Features.PurchaseOrders.Create.v1;
using FSH.Starter.WebApi.Warehouse.Features.PurchaseOrders.Update.v1;
using FSH.Starter.WebApi.Warehouse.Features.Transfers.Create.v1;
using FSH.Starter.WebApi.Warehouse.Features.Transfers.Update.v1;
using FSH.Starter.WebApi.Warehouse.Features.Sales.Create.v1;
using FSH.Starter.WebApi.Warehouse.Features.Sales.Update.v1;
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
        public Endpoints() : base("warehouse") { }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var warehouses = app.MapGroup("warehouses").WithTags("warehouses");
            warehouses.MapWarehouseCreateEndpoint();
            warehouses.MapWarehouseUpdateEndpoint();

            var companies = app.MapGroup("companies").WithTags("companies");
            companies.MapCompanyCreateEndpoint();
            companies.MapCompanyUpdateEndpoint();

            var stores = app.MapGroup("stores").WithTags("stores");
            stores.MapStoreCreateEndpoint();
            stores.MapStoreUpdateEndpoint();

            var categories = app.MapGroup("categories").WithTags("categories");
            categories.MapCategoryCreateEndpoint();
            categories.MapCategoryUpdateEndpoint();

            var suppliers = app.MapGroup("suppliers").WithTags("suppliers");
            suppliers.MapSupplierCreateEndpoint();
            suppliers.MapSupplierUpdateEndpoint();

            var products = app.MapGroup("products").WithTags("products");
            products.MapProductCreateEndpoint();
            products.MapProductUpdateEndpoint();

            var customers = app.MapGroup("customers").WithTags("customers");
            customers.MapCustomerCreateEndpoint();
            customers.MapCustomerUpdateEndpoint();

            var purchaseOrders = app.MapGroup("purchaseorders").WithTags("purchaseorders");
            purchaseOrders.MapPurchaseOrderCreateEndpoint();
            purchaseOrders.MapPurchaseOrderUpdateEndpoint();

            var transfers = app.MapGroup("transfers").WithTags("transfers");
            transfers.MapTransferCreateEndpoint();
            transfers.MapTransferUpdateEndpoint();

            var sales = app.MapGroup("sales").WithTags("sales");
            sales.MapSaleCreateEndpoint();
            sales.MapSaleUpdateEndpoint();
        }
    }

    public static WebApplicationBuilder RegisterWarehouseServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<WarehouseDbContext>();
        builder.Services.AddScoped<IDbInitializer, WarehouseDbInitializer>();

        // keyed repositories for aggregates
        builder.Services.AddKeyedScoped<IRepository<Domain.Warehouse>, WarehouseRepository<Domain.Warehouse>>("warehouse");
        builder.Services.AddKeyedScoped<IReadRepository<Domain.Warehouse>, WarehouseRepository<Domain.Warehouse>>("warehouse");

        builder.Services.AddKeyedScoped<IRepository<Company>, WarehouseRepository<Company>>("warehouse");
        builder.Services.AddKeyedScoped<IReadRepository<Company>, WarehouseRepository<Company>>("warehouse");

        builder.Services.AddKeyedScoped<IRepository<Store>, WarehouseRepository<Store>>("warehouse");
        builder.Services.AddKeyedScoped<IReadRepository<Store>, WarehouseRepository<Store>>("warehouse");

        builder.Services.AddKeyedScoped<IRepository<Category>, WarehouseRepository<Category>>("warehouse");
        builder.Services.AddKeyedScoped<IReadRepository<Category>, WarehouseRepository<Category>>("warehouse");

        builder.Services.AddKeyedScoped<IRepository<Supplier>, WarehouseRepository<Supplier>>("warehouse");
        builder.Services.AddKeyedScoped<IReadRepository<Supplier>, WarehouseRepository<Supplier>>("warehouse");

        builder.Services.AddKeyedScoped<IRepository<Product>, WarehouseRepository<Product>>("warehouse");
        builder.Services.AddKeyedScoped<IReadRepository<Product>, WarehouseRepository<Product>>("warehouse");

        builder.Services.AddKeyedScoped<IRepository<Customer>, WarehouseRepository<Customer>>("warehouse");
        builder.Services.AddKeyedScoped<IReadRepository<Customer>, WarehouseRepository<Customer>>("warehouse");

        builder.Services.AddKeyedScoped<IRepository<PurchaseOrder>, WarehouseRepository<PurchaseOrder>>("warehouse");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseOrder>, WarehouseRepository<PurchaseOrder>>("warehouse");

        builder.Services.AddKeyedScoped<IRepository<StoreTransfer>, WarehouseRepository<StoreTransfer>>("warehouse");
        builder.Services.AddKeyedScoped<IReadRepository<StoreTransfer>, WarehouseRepository<StoreTransfer>>("warehouse");

        builder.Services.AddKeyedScoped<IRepository<Sale>, WarehouseRepository<Sale>>("warehouse");
        builder.Services.AddKeyedScoped<IReadRepository<Sale>, WarehouseRepository<Sale>>("warehouse");

        return builder;
    }

    public static WebApplication UseWarehouseModule(this WebApplication app)
    {
        return app;
    }
}
