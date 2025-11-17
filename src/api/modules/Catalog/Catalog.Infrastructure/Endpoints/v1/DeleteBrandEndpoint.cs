using Shared.Authorization;

namespace FSH.Starter.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class DeleteBrandEndpoint
{
    internal static RouteHandlerBuilder MapBrandDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteBrandCommand(id)).ConfigureAwait(false);
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteBrandEndpoint))
            .WithSummary("deletes brand by id")
            .WithDescription("deletes brand by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Brands))
            .MapToApiVersion(1);
    }
}
