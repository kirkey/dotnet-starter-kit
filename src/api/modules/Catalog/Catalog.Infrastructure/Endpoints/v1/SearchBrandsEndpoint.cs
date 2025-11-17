using Shared.Authorization;

namespace FSH.Starter.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchBrandsEndpoint
{
    internal static RouteHandlerBuilder MapGetBrandListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchBrandsCommand command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchBrandsEndpoint))
            .WithSummary("Gets a list of brands")
            .WithDescription("Gets a list of brands with pagination and filtering support")
            .Produces<PagedList<BrandResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Brands))
            .MapToApiVersion(1);
    }
}
