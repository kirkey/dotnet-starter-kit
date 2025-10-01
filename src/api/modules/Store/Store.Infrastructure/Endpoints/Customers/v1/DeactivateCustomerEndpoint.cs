using FSH.Starter.WebApi.Store.Application.Customers.Deactivate.v1;

namespace Store.Infrastructure.Endpoints.Customers.v1;

public static class DeactivateCustomerEndpoint
{
    internal static RouteHandlerBuilder MapDeactivateCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/deactivate", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new DeactivateCustomerCommand(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(DeactivateCustomerEndpoint))
        .WithSummary("Deactivate a customer")
        .WithDescription("Deactivates a customer to block transactions")
        .Produces<DeactivateCustomerResponse>()
        .RequirePermission("Permissions.Store.Update")
        .MapToApiVersion(1);
    }
}

