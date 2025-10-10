using FSH.Framework.Core.Storage.Queries;
using FSH.Starter.WebApi.Store.Application.Items.Export.v1;

namespace Store.Infrastructure.Endpoints.Items.v1;

/// <summary>
/// Endpoint for exporting Items to Excel format.
/// </summary>
public static class ExportItemsEndpoint
{
    internal static RouteHandlerBuilder MapExportItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/export", async (
                ExportItemsQuery query,
                ISender mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(query, cancellationToken);
                return Results.Ok(result);
            })
            .WithName(nameof(ExportItemsEndpoint))
            .WithSummary("Export items to Excel file")
            .WithDescription(
                "Exports items to Excel format with optional filtering by category, supplier, price range, and other criteria. Returns an ExportResponse with file data.")
            .Produces<ExportResponse>(200)
            .Produces<ExportResponse>(400)
            .RequirePermission("Permissions.Store.Export")
            .MapToApiVersion(1);
    }
}
