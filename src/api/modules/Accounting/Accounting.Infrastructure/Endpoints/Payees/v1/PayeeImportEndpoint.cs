using Accounting.Application.Payees.Import.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Payees.v1;

/// <summary>
/// Endpoint for importing Payees from Excel files.
/// Handles file upload validation and processes payee creation in bulk with vendor master data management.
/// </summary>
public static class PayeeImportEndpoint
{
    internal static RouteHandlerBuilder MapPayeeImportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/import", async (ImportPayeesCommand command, ISender mediator) =>
            {
                var importedCount = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { ImportedCount = importedCount, Message = $"Successfully imported {importedCount} payees" });
            })
            .WithName(nameof(PayeeImportEndpoint))
            .WithSummary("Import payees from Excel file")
            .WithDescription("Imports payees from an Excel (.xlsx) file with validation, duplicate checking, and TIN validation")
            .Produces<object>()
            .ProducesValidationProblem()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .DisableAntiforgery()
            .MapToApiVersion(1);
    }
}
