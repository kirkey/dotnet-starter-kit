namespace FSH.Starter.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class GetProductEndpoint
{
    internal static RouteHandlerBuilder MapProductGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetProductRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetProductEndpoint))
            .WithSummary("gets product by id")
            .WithDescription("gets prodct by id")
            .Produces<ProductResponse>()
            .RequirePermission("Permissions.Products.View")
            .MapToApiVersion(1);
    }
}
