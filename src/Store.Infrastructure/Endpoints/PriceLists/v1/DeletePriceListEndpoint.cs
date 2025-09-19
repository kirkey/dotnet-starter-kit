using FSH.Starter.WebApi.Store.Application.PriceLists.Delete.v1;

namespace Store.Infrastructure.Endpoints.PriceLists.v1;

/// <summary>
/// Endpoint for deleting a price list.
/// </summary>
public static class DeletePriceListEndpoint
{
    /// <summary>
    /// Maps the delete price list endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for delete price list endpoint</returns>
    internal static RouteHandlerBuilder MapDeletePriceListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeletePriceListCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeletePriceList")
        .WithSummary("Delete a price list")
        .WithDescription("Deletes a price list by ID")
        .Produces(204)
        .MapToApiVersion(1);
    }
}
