namespace Store.Infrastructure.Endpoints.GroceryItems;

using FSH.Framework.Core.Storage.File.Features;
using FSH.Starter.WebApi.Store.Application.GroceryItems.Import.v1;

/// <summary>
/// Endpoint for importing grocery items from Excel files.
/// Uses the framework's DataImport service for Excel processing.
/// </summary>
public static class ImportGroceryItemsEndpoint
{
    /// <summary>
    /// Maps the import grocery items endpoint.
    /// </summary>
    /// <param name="app">The route group builder</param>
    /// <returns>The configured route group builder</returns>
    public static RouteGroupBuilder MapImportGroceryItemsEndpoint(this RouteGroupBuilder app)
    {
        app.MapPost("/import", ImportGroceryItemsAsync)
            .WithName("ImportGroceryItems")
            .WithSummary("Import grocery items from Excel file")
            .WithDescription("Imports grocery items from an uploaded Excel (.xlsx) file. Returns detailed results including success/failure counts and error messages.")
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<ImportGroceryItemsResponse>()
            .Produces<ImportGroceryItemsResponse>(207) // Multi-status for partial success
            .ProducesValidationProblem();

        return app;
    }

    /// <summary>
    /// Handles grocery items import from uploaded Excel file.
    /// </summary>
    /// <param name="file">Uploaded Excel file</param>
    /// <param name="mediator">MediatR mediator for command handling</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Import results with detailed success/failure information</returns>
    private static async Task<IResult> ImportGroceryItemsAsync(
        IFormFile file,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        if (file?.Length == 0)
        {
            return Results.BadRequest("No file uploaded or file is empty.");
        }

        // Convert IFormFile to FileUploadCommand
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, cancellationToken);
        var base64Data = Convert.ToBase64String(memoryStream.ToArray());
        var mimeType = file.ContentType;
        var dataUrl = $"data:{mimeType};base64,{base64Data}";

        var fileUploadCommand = new FileUploadCommand
        {
            Name = file.FileName,
            Extension = Path.GetExtension(file.FileName),
            Data = dataUrl,
            Size = file.Length
        };

        var command = new ImportGroceryItemsCommand(fileUploadCommand);
        var result = await mediator.Send(command, cancellationToken);
        
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
}
