using Shared.Authorization;

namespace FSH.Starter.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreateProductEndpoint
{
    internal static RouteHandlerBuilder MapProductCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateProductCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateProductEndpoint))
            .WithSummary("creates a product")
            .WithDescription("creates a product")
            .Produces<CreateProductResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Products))
            .MapToApiVersion(1);
    }
}
