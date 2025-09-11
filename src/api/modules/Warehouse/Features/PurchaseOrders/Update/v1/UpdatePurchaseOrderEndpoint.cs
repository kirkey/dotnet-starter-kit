using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.PurchaseOrders.Update.v1;

public static class UpdatePurchaseOrderEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseOrderUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdatePurchaseOrderCommand request, ISender mediator) =>
        {
            if (id != request.Id) return Results.BadRequest();
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdatePurchaseOrderEndpoint))
        .WithSummary("Updates a purchase order")
        .WithDescription("Updates a purchase order")
        .Produces<UpdatePurchaseOrderResponse>(StatusCodes.Status200OK)
        .RequirePermission("Permissions.Warehouse.Update")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}

