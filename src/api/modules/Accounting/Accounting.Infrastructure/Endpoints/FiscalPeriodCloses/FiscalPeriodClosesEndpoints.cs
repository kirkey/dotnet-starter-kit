using Accounting.Infrastructure.Endpoints.FiscalPeriodCloses.v1;

namespace Accounting.Infrastructure.Endpoints.FiscalPeriodCloses;

/// <summary>
/// Endpoint configuration for Fiscal Period Closes module.
/// </summary>
public static class FiscalPeriodClosesEndpoints
{
    internal static IEndpointRouteBuilder MapFiscalPeriodClosesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/fiscal-period-closes")
            .WithTags("Fiscal Period Closes")
            .WithDescription("Endpoints for managing fiscal period close processes")
            .MapToApiVersion(1);

        // CRUD operations
        group.MapFiscalPeriodCloseCreateEndpoint();
        group.MapFiscalPeriodCloseGetEndpoint();
        group.MapFiscalPeriodCloseSearchEndpoint();
        
        // Workflow operations
        group.MapCompleteTaskEndpoint();
        group.MapAddValidationIssueEndpoint();
        group.MapResolveValidationIssueEndpoint();
        group.MapCompleteFiscalPeriodCloseEndpoint();
        group.MapReopenFiscalPeriodCloseEndpoint();

        return app;
    }
}

