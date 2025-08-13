using Accounting.Application.Customers.Delete;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.Customers.v1;

public static class CustomerDeleteEndpoint
{
    internal static RouteHandlerBuilder MapCustomerDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteCustomerRequest(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(CustomerDeleteEndpoint))
            .WithSummary("delete customer by id")
            .WithDescription("delete customer by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}


