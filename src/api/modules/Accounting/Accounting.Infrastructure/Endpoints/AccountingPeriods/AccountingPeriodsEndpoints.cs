using Accounting.Infrastructure.Endpoints.AccountingPeriods.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.AccountingPeriods;

/// <summary>
/// Endpoint configuration for AccountingPeriods module.
/// Provides comprehensive REST API endpoints for managing accounting-periods.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class AccountingPeriodsEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all AccountingPeriods endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/accounting-periods").WithTags("accounting-period");

        group.MapAccountingPeriodCloseEndpoint();
        group.MapAccountingPeriodCreateEndpoint();
        group.MapAccountingPeriodDeleteEndpoint();
        group.MapAccountingPeriodGetEndpoint();
        group.MapAccountingPeriodReopenEndpoint();
        group.MapAccountingPeriodSearchEndpoint();
        group.MapAccountingPeriodUpdateEndpoint();
    }
}
