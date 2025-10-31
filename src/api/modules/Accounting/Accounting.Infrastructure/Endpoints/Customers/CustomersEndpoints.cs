using Accounting.Infrastructure.Endpoints.Customers.v1;

namespace Accounting.Infrastructure.Endpoints.Customers;

/// <summary>
/// Endpoint configuration for Customers module.
/// Provides comprehensive REST API endpoints for managing customer accounts.
/// </summary>
public static class CustomersEndpoints
{
    /// <summary>
    /// Maps all Customers endpoints to the route builder.
    /// Includes Create, Read, Update, Delete, and Search operations for customers.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <returns>The configured endpoint route builder.</returns>
    internal static IEndpointRouteBuilder MapCustomersEndpoints(this IEndpointRouteBuilder app)
    {
        var customersGroup = app.MapGroup("/customers")
            .WithTags("Customers")
            .WithDescription("Endpoints for managing customers in the accounting system")
            .MapToApiVersion(1);

        // Version 1 endpoints
        customersGroup.MapCustomerCreateEndpoint();
        customersGroup.MapCustomerUpdateEndpoint();
        customersGroup.MapCustomerGetEndpoint();
        customersGroup.MapCustomerSearchEndpoint();

        return app;
    }
}

