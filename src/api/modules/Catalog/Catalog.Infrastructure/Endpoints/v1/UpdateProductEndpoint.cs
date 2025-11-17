using FSH.Starter.WebApi.Catalog.Application.Products.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class UpdateProductEndpoint
{
    internal static RouteHandlerBuilder MapProductUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateProductCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateProductEndpoint))
            .WithSummary("update a product")
            .WithDescription("update a product")
            .Produces<UpdateProductResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Products))
            .MapToApiVersion(1);
    }
}
