using FSH.Starter.WebApi.Store.Application.Items.Delete.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Items.v1;

public static class DeleteItemEndpoint
{
    internal static RouteHandlerBuilder MapDeleteItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var deletedId = await mediator.Send(new DeleteItemCommand(id)).ConfigureAwait(false);
                return Results.Ok(deletedId);
            })
            .WithName(nameof(DeleteItemEndpoint))
            .WithSummary("Delete an item")
            .WithDescription("Deletes an inventory item")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
            .MapToApiVersion(1);
    }
}
