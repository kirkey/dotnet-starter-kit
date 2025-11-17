using Accounting.Application.ChartOfAccounts.Import.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

/// <summary>
/// Endpoint for importing Chart of Accounts from Excel files.
/// Handles file upload validation and processes account creation in bulk.
/// </summary>
public static class ChartOfAccountImportEndpoint
{
    internal static RouteHandlerBuilder MapChartOfAccountImportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/import", async (ImportChartOfAccountsCommand command, ISender mediator) =>
            {
                var importedCount = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { ImportedCount = importedCount, Message = $"Successfully imported {importedCount} chart of accounts" });
            })
            .WithName(nameof(ChartOfAccountImportEndpoint))
            .WithSummary("Import chart of accounts from Excel file")
            .WithDescription("Imports chart of accounts from an Excel (.xlsx) file with validation and duplicate checking")
            .Produces<object>()
            .ProducesValidationProblem()
            .RequirePermission(FshPermission.NameFor(FshActions.Import, FshResources.Accounting))
            .DisableAntiforgery()
            .MapToApiVersion(1);
    }
}
