using Accounting.Infrastructure.Endpoints.AccountingPeriods.v1;

namespace Accounting.Infrastructure.Endpoints.AccountingPeriods;

/// <summary>
/// Endpoint configuration for Accounting Periods module.
/// </summary>
public static class AccountingPeriodsEndpoints
{
    /// <summary>
    /// Maps all Accounting Periods endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapAccountingPeriodsEndpoints(this IEndpointRouteBuilder app)
    {
        var accountingPeriodsGroup = app.MapGroup("/accounting-periods")
            .WithTags("Accounting-Periods")
            .WithName(nameof(AccountingPeriodsEndpoints))
            .WithDescription("Endpoints for managing accounting periods");

        // Version 1 endpoints will be added here when implemented
        accountingPeriodsGroup.MapAccountingPeriodCreateEndpoint();
        accountingPeriodsGroup.MapAccountingPeriodUpdateEndpoint();
        accountingPeriodsGroup.MapAccountingPeriodDeleteEndpoint();
        accountingPeriodsGroup.MapAccountingPeriodGetEndpoint();
        accountingPeriodsGroup.MapAccountingPeriodSearchEndpoint();
        // Added workflow endpoints
        accountingPeriodsGroup.MapAccountingPeriodCloseEndpoint();
        accountingPeriodsGroup.MapAccountingPeriodReopenEndpoint();

        return app;
    }
}
