using Shared.Authorization;

namespace FSH.Starter.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreateBrandEndpoint
{
    internal static RouteHandlerBuilder MapBrandCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateBrandCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateBrandEndpoint))
            .WithSummary("creates a brand")
            .WithDescription("creates a brand")
            .Produces<CreateBrandResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Brands))
            .MapToApiVersion(1);
    }
}
