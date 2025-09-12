using FSH.Starter.WebApi.Catalog.Application.Products.Search.v1;

namespace FSH.Starter.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchProductsEndpoint
{
    internal static RouteHandlerBuilder MapProductGetListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchProductsCommand command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchProductsEndpoint))
            .WithSummary("Gets a list of products")
            .WithDescription("Gets a list of products with pagination and filtering support")
            .Produces<PagedList<ProductResponse>>()
            .RequirePermission("Permissions.Products.View")
            .MapToApiVersion(1);
    }
}

