using Accounting.Application.ChartOfAccounts.Export.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

/// <summary>
/// Endpoint for exporting Chart of Accounts to Excel files.
/// Supports filtering by account type, USOA category, and various criteria.
/// </summary>
public static class ChartOfAccountExportEndpoint
{
    internal static RouteHandlerBuilder MapChartOfAccountExportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/export", async (ExportChartOfAccountsQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.File(response.Data, response.ContentType, response.FileName);
            })
            .WithName(nameof(ChartOfAccountExportEndpoint))
            .WithSummary("Export chart of accounts to Excel file")
            .WithDescription("Exports chart of accounts to Excel (.xlsx) file with optional filtering by account type, USOA category, and search criteria")
            .Produces<FileResult>()
            .ProducesValidationProblem()
            .RequirePermission(FshPermission.NameFor(FshActions.Export, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
