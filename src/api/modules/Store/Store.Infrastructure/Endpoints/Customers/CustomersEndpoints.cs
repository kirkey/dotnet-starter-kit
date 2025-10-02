namespace Store.Infrastructure.Endpoints.Customers;

/// <summary>
/// Endpoint configuration for Customers module.
/// </summary>
public static class CustomersEndpoints
{
    /// <summary>
    /// Maps all Customers endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapCustomersEndpoints(this IEndpointRouteBuilder app)
    {
        var customersGroup = app.MapGroup("/customers")
            .WithTags("Customers")
            .WithDescription("Endpoints for managing customers");

        // Version 1 endpoints will be added here when implemented
        // customersGroup.MapCreateCustomerEndpoint();
        // customersGroup.MapUpdateCustomerEndpoint();
        // customersGroup.MapDeleteCustomerEndpoint();
        // customersGroup.MapGetCustomerEndpoint();
        // customersGroup.MapSearchCustomersEndpoint();

        return app;
    }
}
