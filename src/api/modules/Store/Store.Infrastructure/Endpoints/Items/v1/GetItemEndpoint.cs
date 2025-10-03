using FSH.Starter.WebApi.Store.Application.Items.Get.v1;

namespace Store.Infrastructure.Endpoints.Items.v1;

public static class GetItemEndpoint
{
    internal static RouteHandlerBuilder MapGetItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetItemCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetItemEndpoint))
            .WithSummary("Get item by ID")
            .WithDescription("Retrieves a specific inventory item by its ID")
            .Produces<ItemResponse>()
            .RequirePermission("Permissions.Store.View")
            .MapToApiVersion(1);
    }
}
