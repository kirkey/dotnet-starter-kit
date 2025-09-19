using FSH.Starter.WebApi.Store.Application.PriceLists.Create.v1;

namespace Store.Infrastructure.Endpoints.PriceLists.v1;

/// <summary>
/// Endpoint for creating a new price list.
/// </summary>
public static class CreatePriceListEndpoint
{
    /// <summary>
    /// Maps the create price list endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for create price list endpoint</returns>
    internal static RouteHandlerBuilder MapCreatePriceListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreatePriceListCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/price-lists/{result.Id}", result);
        })
        .WithName("CreatePriceList")
        .WithSummary("Create a new price list")
        .WithDescription("Creates a new price list")
        .Produces<CreatePriceListResponse>()
        .MapToApiVersion(1);
    }
}
