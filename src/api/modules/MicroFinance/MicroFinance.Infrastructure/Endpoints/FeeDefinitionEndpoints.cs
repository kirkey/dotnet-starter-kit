namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Fee Definitions.
/// </summary>
public static class FeeDefinitionEndpoints
{
    /// <summary>
    /// Maps all Fee Definition endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapFeeDefinitionEndpoints(this IEndpointRouteBuilder app)
    {
        var feeDefinitionsGroup = app.MapGroup("fee-definitions").WithTags("fee-definitions");

        feeDefinitionsGroup.MapGet("/", () => Results.Ok("Fee Definitions endpoint - Coming soon"))
            .WithName("GetFeeDefinitions")
            .WithSummary("Gets all fee definitions");

        return app;
    }
}
