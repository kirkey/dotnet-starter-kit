using Carter;

namespace Accounting.Infrastructure.Endpoints.FuelConsumption;

/// <summary>
/// Endpoint configuration for FuelConsumption module.
/// Provides comprehensive REST API endpoints for managing fuel-consumption.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class FuelConsumptionEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all FuelConsumption endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        // TODO: Implement FuelConsumption endpoints
        // var group = app.MapGroup("accounting/fuel-consumption").WithTags("fuel-consumption");
        // group.MapFuelConsumptionCreateEndpoint();
        // group.MapFuelConsumptionDeleteEndpoint();
        // group.MapFuelConsumptionGetEndpoint();
        // group.MapFuelConsumptionSearchEndpoint();
        // group.MapFuelConsumptionUpdateEndpoint();
    }
}
