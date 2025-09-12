namespace FSH.Starter.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class DeleteProductEndpoint
{
    internal static RouteHandlerBuilder MapProductDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteProductCommand(id)).ConfigureAwait(false);
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteProductEndpoint))
            .WithSummary("deletes product by id")
            .WithDescription("deletes product by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Products.Delete")
            .MapToApiVersion(1);
    }
}
