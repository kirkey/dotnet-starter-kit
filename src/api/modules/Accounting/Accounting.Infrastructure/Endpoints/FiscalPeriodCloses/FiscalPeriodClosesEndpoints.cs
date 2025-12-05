using Accounting.Infrastructure.Endpoints.FiscalPeriodCloses.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.FiscalPeriodCloses;

/// <summary>
/// Endpoint configuration for FiscalPeriodCloses module.
/// Provides comprehensive REST API endpoints for managing fiscal-period-closes.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class FiscalPeriodClosesEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all FiscalPeriodCloses endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/fiscal-period-closes").WithTags("fiscal-period-close");

        group.MapAddValidationIssueEndpoint();
        group.MapCompleteFiscalPeriodCloseEndpoint();
        group.MapCompleteFiscalPeriodCloseTaskEndpoint();
        group.MapFiscalPeriodCloseCreateEndpoint();
        group.MapFiscalPeriodCloseGetEndpoint();
        group.MapFiscalPeriodCloseSearchEndpoint();
        group.MapReopenFiscalPeriodCloseEndpoint();
        group.MapResolveValidationIssueEndpoint();
    }
}
