using FSH.Starter.WebApi.Store.Application.Bins.Delete.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Bins.v1;

public static class DeleteBinEndpoint
{
    internal static RouteHandlerBuilder MapDeleteBinEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var deletedId = await mediator.Send(new DeleteBinCommand(id)).ConfigureAwait(false);
                return Results.Ok(deletedId);
            })
            .WithName(nameof(DeleteBinEndpoint))
            .WithSummary("Delete a bin")
            .WithDescription("Deletes a storage bin")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Warehouse))
            .MapToApiVersion(1);
    }
}
