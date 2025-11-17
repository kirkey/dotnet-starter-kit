using FSH.Starter.WebApi.Catalog.Application.Brands.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class UpdateBrandEndpoint
{
    internal static RouteHandlerBuilder MapBrandUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateBrandCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateBrandEndpoint))
            .WithSummary("update a brand")
            .WithDescription("update a brand")
            .Produces<UpdateBrandResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Brands))
            .MapToApiVersion(1);
    }
}
