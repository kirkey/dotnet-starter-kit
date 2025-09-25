using FSH.Starter.WebApi.Store.Application.GroceryItems.Export.v1;

namespace Store.Infrastructure.Endpoints.GroceryItems;

/// <summary>
/// Endpoint for exporting grocery items to Excel files.
/// Uses the framework's DataExport service for Excel generation.
/// </summary>
public static class ExportGroceryItemsEndpoint
{
    /// <summary>
    /// Maps the export grocery items endpoint.
    /// </summary>
    /// <param name="app">The route group builder</param>
    /// <returns>The configured route group builder</returns>
    public static RouteGroupBuilder MapExportGroceryItemsEndpoint(this RouteGroupBuilder app)
    {
        app.MapGet("/export", ExportGroceryItemsAsync)
            .WithName("ExportGroceryItems")
            .WithSummary("Export grocery items to Excel file")
            .WithDescription("Exports grocery items to Excel format with optional filtering by category, supplier, search terms, and various criteria.")
            .Produces<FileResult>(contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            .ProducesValidationProblem();

        return app;
    }

    /// <summary>
    /// Handles grocery items export to Excel file with optional filtering.
    /// </summary>
    /// <param name="categoryId">Optional category filter</param>
    /// <param name="supplierId">Optional supplier filter</param>
    /// <param name="searchTerm">Optional search term filter</param>
    /// <param name="includeInactive">Include inactive items</param>
    /// <param name="onlyLowStock">Export only low stock items</param>
    /// <param name="onlyPerishable">Export only perishable items</param>
    /// <param name="mediator">MediatR mediator for query handling</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Excel file as download</returns>
    private static async Task<IResult> ExportGroceryItemsAsync(
        DefaultIdType? categoryId,
        DefaultIdType? supplierId,
        string? searchTerm,
        bool includeInactive,
        bool onlyLowStock,
        bool onlyPerishable,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new ExportGroceryItemsQuery(
            categoryId,
            supplierId,
            searchTerm,
            includeInactive,
            onlyLowStock,
            onlyPerishable);

        var result = await mediator.Send(query, cancellationToken);
        
        return Results.File(
            result.Data,
            result.ContentType,
            result.FileName);
    }
}
