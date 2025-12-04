namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Fixed Deposits.
/// </summary>
public static class FixedDepositEndpoints
{
    /// <summary>
    /// Maps all Fixed Deposit endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapFixedDepositEndpoints(this IEndpointRouteBuilder app)
    {
        var fixedDepositsGroup = app.MapGroup("fixed-deposits").WithTags("fixed-deposits");

        fixedDepositsGroup.MapGet("/", () => Results.Ok("Fixed Deposits endpoint - Coming soon"))
            .WithName("GetFixedDeposits")
            .WithSummary("Gets all fixed deposits");

        return app;
    }
}
