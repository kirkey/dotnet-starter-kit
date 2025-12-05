using Accounting.Infrastructure.Endpoints.TrialBalance.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.TrialBalance;

/// <summary>
/// Endpoint configuration for TrialBalance module.
/// Provides comprehensive REST API endpoints for managing trial-balance.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class TrialBalanceEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all TrialBalance endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/trial-balance").WithTags("trial-balance");

        group.MapTrialBalanceCreateEndpoint();
        group.MapTrialBalanceFinalizeEndpoint();
        group.MapTrialBalanceGetEndpoint();
        group.MapTrialBalanceReopenEndpoint();
        group.MapTrialBalanceSearchEndpoint();
    }
}
