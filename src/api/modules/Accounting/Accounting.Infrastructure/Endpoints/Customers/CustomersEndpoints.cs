using Accounting.Infrastructure.Endpoints.Customers.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Customers;

/// <summary>
/// Endpoint configuration for Customers module.
/// Provides comprehensive REST API endpoints for managing customers.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class CustomersEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Customers endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/customers").WithTags("customer");

        group.MapCustomerCreateEndpoint();
        group.MapCustomerGetEndpoint();
        group.MapCustomerSearchEndpoint();
        group.MapCustomerUpdateEndpoint();
    }
}
