﻿using Carter;
using FSH.Starter.WebApi.Catalog.Infrastructure.Endpoints.v1;
using FSH.Starter.WebApi.Catalog.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Catalog.Infrastructure;

/// <summary>
/// Catalog module configuration and endpoint registration.
/// Handles all product and brand CRUD operations and service registration.
/// </summary>
public static class CatalogModule
{
    /// <summary>
    /// Endpoint routes for the Catalog module.
    /// Maps all product and brand endpoints with proper grouping and documentation.
    /// </summary>
    public class Endpoints() : CarterModule("catalog")
    {
        /// <summary>
        /// Adds all catalog routes (products and brands) to the application.
        /// Organizes endpoints by resource type with proper tagging for OpenAPI.
        /// </summary>
        /// <param name="app">The endpoint route builder.</param>
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var productGroup = app.MapGroup("products").WithTags("products");
            productGroup.MapProductCreateEndpoint();
            productGroup.MapProductGetEndpoint();
            productGroup.MapProductGetListEndpoint();
            productGroup.MapProductUpdateEndpoint();
            productGroup.MapProductDeleteEndpoint();

            var brandGroup = app.MapGroup("brands").WithTags("brands");
            brandGroup.MapBrandCreateEndpoint();
            brandGroup.MapGetBrandEndpoint();
            brandGroup.MapGetBrandListEndpoint();
            brandGroup.MapBrandUpdateEndpoint();
            brandGroup.MapBrandDeleteEndpoint();
        }
    }

    /// <summary>
    /// Registers all catalog services in the dependency injection container.
    /// Configures DbContext, repositories, and database initializers.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The web application builder for chaining.</returns>
    public static WebApplicationBuilder RegisterCatalogServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<CatalogDbContext>();
        builder.Services.AddScoped<IDbInitializer, CatalogDbInitializer>();
        builder.Services.AddKeyedScoped<IRepository<Product>, CatalogRepository<Product>>("catalog:products");
        builder.Services.AddKeyedScoped<IReadRepository<Product>, CatalogRepository<Product>>("catalog:products");
        builder.Services.AddKeyedScoped<IRepository<Brand>, CatalogRepository<Brand>>("catalog:brands");
        builder.Services.AddKeyedScoped<IReadRepository<Brand>, CatalogRepository<Brand>>("catalog:brands");
        return builder;
    }

    /// <summary>
    /// Applies the catalog module to the web application.
    /// Currently a no-op placeholder for future middleware or configuration.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>The web application for chaining.</returns>
    public static WebApplication UseCatalogModule(this WebApplication app)
    {
        return app;
    }
}
