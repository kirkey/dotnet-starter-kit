using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Sales.Create.v1;

public static class CreateSaleEndpoint
{
    internal static RouteHandlerBuilder MapSaleCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateSaleCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(CreateSaleEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreateSaleEndpoint))
        .WithSummary("Creates a sale")
        .WithDescription("Creates a sale")
        .Produces<CreateSaleResponse>(StatusCodes.Status201Created)
        .RequirePermission("Permissions.Warehouse.Create")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}

