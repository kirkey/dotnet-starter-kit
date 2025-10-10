using FSH.Framework.Core.Storage.Commands;
using FSH.Starter.WebApi.Store.Application.Items.Import.v1;

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

                if (result.IsSuccess)
                {
                    return Results.Ok(new
                    {
                        Message = $"Successfully imported {result.ImportedCount} items",
                        Data = result
                    });
                }

                return Results.BadRequest(new
                {
                    Message = result.FailedCount > 0
                        ? $"Import completed with {result.FailedCount} errors. {result.ImportedCount} items imported successfully."
                        : "Import validation failed",
                    Data = result,
                    Errors = result.Errors
                });
            })
            .WithName(nameof(ImportItemsEndpoint))
            .WithSummary("Import items from Excel file")
            .WithDescription("Imports items from an Excel file with validation. Returns count of successful and failed imports with detailed error messages.")
            .Produces<ImportResponse>(200)
            .Produces<ImportResponse>(400)
            .RequirePermission("Permissions.Store.Import")
            .MapToApiVersion(1);
    }
}

