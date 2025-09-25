using FSH.Starter.WebApi.Store.Application.GroceryItems.Export.v1;
using FSH.Starter.WebApi.Store.Application.GroceryItems.Import.v1;

namespace Store.Infrastructure.Endpoints.GroceryItems;

/// <summary>
/// Endpoints for importing and exporting grocery items data.
/// Supports Excel file import/export with comprehensive filtering options.
/// </summary>
public static class GroceryItemsImportExportEndpoints
{
    /// <summary>
    /// Maps grocery items import and export endpoints to the application.
    /// </summary>
    /// <param name="app">The web application builder</param>
    /// <returns>The configured route group builder</returns>
    public static RouteGroupBuilder MapGroceryItemsImportExportEndpoints(this RouteGroupBuilder app)
    {
        app.MapPost("/import", ImportGroceryItemsAsync)
            .WithName("ImportGroceryItems")
            .WithSummary("Import grocery items from Excel file")
            .WithDescription("Imports grocery items from an uploaded Excel (.xlsx) file. Returns detailed results including success/failure counts and error messages.")
            .Accepts<ImportGroceryItemsCommand>("multipart/form-data")
            .Produces<ImportGroceryItemsResponse>()
            .ProducesValidationProblem()
            .RequireAuthorization("Permissions.Store.Import");

        app.MapGet("/export", ExportGroceryItemsAsync)
            .WithName("ExportGroceryItems")
            .WithSummary("Export grocery items to Excel file")
            .WithDescription("Exports grocery items to Excel format with optional filtering by category, supplier, search terms, and various criteria.")
            .Produces<FileResult>(contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            .ProducesValidationProblem()
            .RequireAuthorization("Permissions.Store.Export");

        return app;
    }

    /// <summary>
    /// Handles grocery items import from uploaded Excel file.
    /// </summary>
    /// <param name="request">Import command with file upload</param>
    /// <param name="mediator">MediatR mediator for command handling</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Import results with detailed success/failure information</returns>
    private static async Task<IResult> ImportGroceryItemsAsync(
        ImportGroceryItemsCommand request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        
        if (result.IsFullySuccessful)
        {
            return Results.Ok(result);
        }
        
        if (result.HasPartialSuccess)
        {
            // Return 207 Multi-Status for partial success
            return Results.Json(result, statusCode: 207);
        }
        
        // Complete failure
        return Results.BadRequest(result);
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
