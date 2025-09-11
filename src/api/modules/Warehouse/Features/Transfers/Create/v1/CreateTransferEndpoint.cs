using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Transfers.Create.v1;

public static class CreateTransferEndpoint
{
    internal static RouteHandlerBuilder MapTransferCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateTransferCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(CreateTransferEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreateTransferEndpoint))
        .WithSummary("Creates a transfer")
        .WithDescription("Creates a transfer")
        .Produces<CreateTransferResponse>(StatusCodes.Status201Created)
        .RequirePermission("Permissions.Warehouse.Create")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}

