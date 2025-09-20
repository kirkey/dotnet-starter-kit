using FSH.Starter.WebApi.Store.Application.PriceLists.Update.v1;

namespace Store.Infrastructure.Endpoints.PriceLists.v1;

/// <summary>
/// Endpoint for updating a price list.
/// </summary>
public static class UpdatePriceListEndpoint
{
    /// <summary>
    /// Maps the update price list endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for update price list endpoint</returns>
    internal static RouteHandlerBuilder MapUpdatePriceListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdatePriceListCommand command, ISender sender) =>
        {
            var updateCommand = command with { Id = id };
            var result = await sender.Send(updateCommand).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(UpdatePriceListEndpoint))
        .WithSummary("Update a price list")
        .WithDescription("Updates an existing price list")
        .Produces<UpdatePriceListResponse>()
        .RequirePermission("Permissions.PriceLists.Update")
        .MapToApiVersion(1);
    }
}
