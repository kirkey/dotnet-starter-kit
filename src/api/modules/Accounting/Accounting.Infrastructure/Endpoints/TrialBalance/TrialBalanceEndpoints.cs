namespace Accounting.Infrastructure.Endpoints.TrialBalance;

/// <summary>
/// Endpoint configuration for Trial Balance module.
/// </summary>
public static class TrialBalanceEndpoints
{
    /// <summary>
    /// Maps all Trial Balance endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapTrialBalanceEndpoints(this IEndpointRouteBuilder app)
    {
        var trialBalanceGroup = app.MapGroup("/trial-balance")
            .WithTags("Trial-Balance")
            .WithDescription("Endpoints for managing trial balance reports");

        // Version 1 endpoints will be added here when implemented
        // trialBalanceGroup.MapTrialBalanceCreateEndpoint();
        // trialBalanceGroup.MapTrialBalanceUpdateEndpoint();
        // trialBalanceGroup.MapTrialBalanceDeleteEndpoint();
        // trialBalanceGroup.MapTrialBalanceGetEndpoint();
        // trialBalanceGroup.MapTrialBalanceSearchEndpoint();

        return app;
    }
}
