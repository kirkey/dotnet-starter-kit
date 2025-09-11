using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Customers.Create.v1;

public static class CreateCustomerEndpoint
{
    internal static RouteHandlerBuilder MapCustomerCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateCustomerCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(CreateCustomerEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreateCustomerEndpoint))
        .WithSummary("Creates a customer")
        .WithDescription("Creates a customer")
        .Produces<CreateCustomerResponse>(StatusCodes.Status201Created)
        .RequirePermission("Permissions.Warehouse.Create")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}
