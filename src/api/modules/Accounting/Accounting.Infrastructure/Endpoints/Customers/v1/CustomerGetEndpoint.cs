using Accounting.Application.Customers.Dtos;
using Accounting.Application.Customers.Get;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.Customers.v1;

public static class CustomerGetEndpoint
{
    internal static RouteHandlerBuilder MapCustomerGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetCustomerRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CustomerGetEndpoint))
            .WithSummary("get a customer by id")
            .WithDescription("get a customer by id")
            .Produces<CustomerDto>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


