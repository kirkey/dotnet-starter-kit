using Accounting.Application.Payees.Export.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Payees.v1;

/// <summary>
/// Endpoint for exporting Payees to Excel files.
/// Supports filtering by expense account, search criteria, and TIN presence.
/// </summary>
public static class PayeeExportEndpoint
{
    internal static RouteHandlerBuilder MapPayeeExportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/export", async (ExportPayeesQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.File(response.Data, response.ContentType, response.FileName);
            })
            .WithName(nameof(PayeeExportEndpoint))
            .WithSummary("Export payees to Excel file")
            .WithDescription("Exports payees to Excel (.xlsx) file with optional filtering by expense account, search criteria, TIN presence, and active status")
            .Produces<FileResult>()
            .ProducesValidationProblem()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
