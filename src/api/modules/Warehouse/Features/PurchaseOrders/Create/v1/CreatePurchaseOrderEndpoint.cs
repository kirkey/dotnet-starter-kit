using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.PurchaseOrders.Create.v1;

public static class CreatePurchaseOrderEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseOrderCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreatePurchaseOrderCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(CreatePurchaseOrderEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreatePurchaseOrderEndpoint))
        .WithSummary("Creates a purchase order")
        .WithDescription("Creates a purchase order")
        .Produces<CreatePurchaseOrderResponse>(StatusCodes.Status201Created)
        .RequirePermission("Permissions.Warehouse.Create")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}

