using FSH.Starter.WebApi.Store.Application.GroceryItems.Import.v1;

namespace Store.Infrastructure.Endpoints.GroceryItems.v1;

/// <summary>
/// Endpoint to import grocery items from an Excel file (.xlsx). Returns the total imported count.
/// </summary>
public static class ImportGroceryItemsEndpoint
{
    internal static RouteHandlerBuilder MapImportGroceryItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/import", async (ImportGroceryItemsCommand request, ISender mediator) =>
            {
                var total = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(total);
            })
            .WithName(nameof(ImportGroceryItemsEndpoint))
            .WithSummary("Import grocery items from Excel")
            .WithDescription("Accepts a FileUploadCommand payload inside ImportGroceryItemsCommand and returns the imported count.")
            .Produces<int>()
            .RequirePermission("Permissions.Store.Create")
            .MapToApiVersion(1);
    }
}

