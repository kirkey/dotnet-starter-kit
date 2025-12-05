using Accounting.Infrastructure.Endpoints.FuelConsumption.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.FuelConsumption;

/// <summary>
/// Endpoint configuration for FuelConsumption module.
/// Provides comprehensive REST API endpoints for managing fuel-consumption.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class FuelConsumptionEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all FuelConsumption endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/fuel-consumption").WithTags("fuel-consumption");

        group.MapFuelConsumptionCreateEndpoint();
        group.MapFuelConsumptionDeleteEndpoint();
        group.MapFuelConsumptionGetEndpoint();
        group.MapFuelConsumptionSearchEndpoint();
        group.MapFuelConsumptionUpdateEndpoint();
    }
}
