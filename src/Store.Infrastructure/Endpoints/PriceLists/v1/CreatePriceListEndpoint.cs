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
        return endpoints.MapPost("/", async (CreatePriceListCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(CreatePriceListEndpoint))
        .WithSummary("Create a new price list")
        .WithDescription("Creates a new price list")
        .Produces<CreatePriceListResponse>()
        .RequirePermission("Permissions.Store.Create")
        .MapToApiVersion(1);
    }
}
