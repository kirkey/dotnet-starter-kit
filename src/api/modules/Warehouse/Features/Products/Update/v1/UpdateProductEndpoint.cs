using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Products.Update.v1;

public static class UpdateProductEndpoint
{
    public static RouteHandlerBuilder MapProductUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateProductCommand request, ISender mediator) =>
        {
            if (id != request.Id) return Results.BadRequest();
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateProductEndpoint))
        .WithSummary("Updates a product")
        .WithDescription("Updates a product")
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .RequirePermission("Permissions.Warehouse.Update")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}
