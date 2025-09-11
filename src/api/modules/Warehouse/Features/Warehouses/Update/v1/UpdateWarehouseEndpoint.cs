using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Warehouses.Update.v1;

public static class UpdateWarehouseEndpoint
{
    internal static RouteHandlerBuilder MapWarehouseUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateWarehouseCommand request, ISender mediator) =>
        {
            if (id != request.Id) return Results.BadRequest();
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateWarehouseEndpoint))
        .WithSummary("Updates a warehouse")
        .WithDescription("Updates a warehouse")
        .Produces<UpdateWarehouseResponse>(StatusCodes.Status200OK)
        .RequirePermission("Permissions.Warehouse.Update")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}

