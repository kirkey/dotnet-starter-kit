using Accounting.Infrastructure.Endpoints.Consumptions.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Consumptions;

/// <summary>
/// Endpoint configuration for Consumptions module.
/// Provides comprehensive REST API endpoints for managing consumptions.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class ConsumptionsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Consumptions endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/consumptions").WithTags("consumption");

        group.MapConsumptionCreateEndpoint();
        group.MapConsumptionDeleteEndpoint();
        group.MapConsumptionGetEndpoint();
        group.MapConsumptionSearchEndpoint();
        group.MapConsumptionUpdateEndpoint();
    }
}
