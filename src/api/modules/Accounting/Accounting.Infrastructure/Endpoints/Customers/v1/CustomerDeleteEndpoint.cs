using Accounting.Application.Customers.Delete;

namespace Accounting.Infrastructure.Endpoints.Customers.v1;

public static class CustomerDeleteEndpoint
{
    internal static RouteHandlerBuilder MapCustomerDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteCustomerCommand(id)).ConfigureAwait(false);
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
