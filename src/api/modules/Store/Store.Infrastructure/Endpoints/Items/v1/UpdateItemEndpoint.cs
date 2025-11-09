using FSH.Starter.WebApi.Store.Application.Items.Update.v1;

namespace Store.Infrastructure.Endpoints.Items.v1;

/// <summary>
/// Endpoint for updating an item.
/// </summary>
public static class UpdateItemEndpoint
{
    /// <summary>
    /// Maps the update item endpoint.
    /// </summary>
    internal static RouteHandlerBuilder MapUpdateItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateItemCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateItemEndpoint))
            .WithSummary("Update an existing item")
            .WithDescription("Updates an existing inventory item")
            .Produces<UpdateItemResponse>()
            .RequirePermission("Permissions.Store.Update")
            .MapToApiVersion(1);
    }
}
