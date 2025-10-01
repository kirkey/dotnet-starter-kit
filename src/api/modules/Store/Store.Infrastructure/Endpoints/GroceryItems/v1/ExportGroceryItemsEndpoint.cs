using FSH.Starter.WebApi.Store.Application.GroceryItems.Export.v1;

namespace Store.Infrastructure.Endpoints.GroceryItems.v1;

/// <summary>
/// Endpoint to export grocery items to an Excel file (.xlsx) with filtering capabilities.
/// Returns an Excel file containing the filtered grocery items data.
/// </summary>
public static class ExportGroceryItemsEndpoint
{
    /// <summary>
    /// Maps the export grocery items endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for export grocery items endpoint</returns>
    internal static RouteHandlerBuilder MapExportGroceryItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/export", async (ISender mediator, [FromBody] ExportGroceryItemsQuery request) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.File(
                    response.Data,
                    response.ContentType,
                    response.FileName);
            })
            .WithName(nameof(ExportGroceryItemsEndpoint))
            .WithSummary("Export grocery items to Excel")
            .WithDescription("Export grocery items to Excel file with optional filtering by category, supplier, search term, stock status, and other criteria.")
            .Produces<FileResult>(StatusCodes.Status200OK, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            .RequirePermission("Permissions.Store.Export")
            .MapToApiVersion(1);
    }
}
