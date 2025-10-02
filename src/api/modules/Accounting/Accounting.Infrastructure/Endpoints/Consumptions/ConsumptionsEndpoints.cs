namespace Accounting.Infrastructure.Endpoints.Consumptions;

/// <summary>
/// Endpoint configuration for Consumptions module.
/// </summary>
public static class ConsumptionsEndpoints
{
    /// <summary>
    /// Maps all Consumptions endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapConsumptionsEndpoints(this IEndpointRouteBuilder app)
    {
        var consumptionsGroup = app.MapGroup("/consumptions")
            .WithTags("Consumptions")
            .WithDescription("Endpoints for managing consumption tracking");

        // Version 1 endpoints will be added here when implemented
        // consumptionsGroup.MapConsumptionCreateEndpoint();
        // consumptionsGroup.MapConsumptionUpdateEndpoint();
        // consumptionsGroup.MapConsumptionDeleteEndpoint();
        // consumptionsGroup.MapConsumptionGetEndpoint();
        // consumptionsGroup.MapConsumptionSearchEndpoint();

        return app;
    }
}
