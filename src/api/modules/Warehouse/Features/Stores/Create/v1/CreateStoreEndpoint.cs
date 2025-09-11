using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Stores.Create.v1;

public static class CreateStoreEndpoint
{
    internal static RouteHandlerBuilder MapStoreCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateStoreCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(CreateStoreEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreateStoreEndpoint))
        .WithSummary("Creates a store")
        .WithDescription("Creates a store")
        .Produces<CreateStoreResponse>(StatusCodes.Status201Created)
        .RequirePermission("Permissions.Warehouse.Create")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}

