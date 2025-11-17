using FSH.Framework.Core.Storage.Commands;
using FSH.Starter.WebApi.Store.Application.Items.Import.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Items.v1;

/// <summary>
/// Endpoint for importing Items from Excel files.
/// </summary>
public static class ImportItemsEndpoint
{
    internal static RouteHandlerBuilder MapImportItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/import", async (
                ImportItemsCommand command,
                ISender mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);

                // Always return 200 OK with the ImportResponse
                // The client will check IsSuccess, ImportedCount, and FailedCount to determine the outcome
                return Results.Ok(result);
            })
            .WithName(nameof(ImportItemsEndpoint))
            .WithSummary("Import items from Excel file")
            .WithDescription("Imports items from an Excel file with validation. Returns ImportResponse with successful/failed counts and detailed error messages.")
            .Produces<ImportResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Import, FshResources.Store))
            .MapToApiVersion(1);
    }
}

