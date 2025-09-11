using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Suppliers.Update.v1;

public static class UpdateSupplierEndpoint
{
    internal static RouteHandlerBuilder MapSupplierUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateSupplierCommand request, ISender mediator) =>
        {
            if (id != request.Id) return Results.BadRequest();
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateSupplierEndpoint))
        .WithSummary("Updates a supplier")
        .WithDescription("Updates a supplier")
        .Produces<UpdateSupplierResponse>(StatusCodes.Status200OK)
        .RequirePermission("Permissions.Warehouse.Update")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}

