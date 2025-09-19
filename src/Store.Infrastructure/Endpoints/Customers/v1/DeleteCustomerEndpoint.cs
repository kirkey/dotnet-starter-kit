using FSH.Starter.WebApi.Store.Application.Customers.Delete.v1;

namespace Store.Infrastructure.Endpoints.Customers.v1;

public static class DeleteCustomerEndpoint
{
    internal static RouteHandlerBuilder MapDeleteCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            await mediator.Send(new DeleteCustomerCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName(nameof(DeleteCustomerEndpoint))
        .WithSummary("Delete customer")
        .WithDescription("Deletes a customer by their unique identifier")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission("Permissions.Customers.Delete")
        .MapToApiVersion(1);
    }
}
