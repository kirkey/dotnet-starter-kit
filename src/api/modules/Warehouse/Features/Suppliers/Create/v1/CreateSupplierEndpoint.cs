using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Suppliers.Create.v1;

public static class CreateSupplierEndpoint
{
    internal static RouteHandlerBuilder MapSupplierCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateSupplierCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(CreateSupplierEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreateSupplierEndpoint))
        .WithSummary("Creates a supplier")
        .WithDescription("Creates a supplier")
        .Produces<CreateSupplierResponse>(StatusCodes.Status201Created)
        .RequirePermission("Permissions.Warehouse.Create")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}
