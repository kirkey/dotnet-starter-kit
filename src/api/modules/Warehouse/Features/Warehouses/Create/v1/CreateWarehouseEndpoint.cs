using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Warehouses.Create.v1;

public static class CreateWarehouseEndpoint
{
    internal static RouteHandlerBuilder MapWarehouseCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateWarehouseCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(CreateWarehouseEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreateWarehouseEndpoint))
        .WithSummary("Creates a warehouse")
        .WithDescription("Creates a warehouse")
        .Produces<CreateWarehouseResponse>(StatusCodes.Status201Created)
        .RequirePermission("Permissions.Warehouse.Create")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}

