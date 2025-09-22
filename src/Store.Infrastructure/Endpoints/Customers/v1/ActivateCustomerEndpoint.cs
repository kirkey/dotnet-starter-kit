using FSH.Starter.WebApi.Store.Application.Customers.Activate.v1;

namespace Store.Infrastructure.Endpoints.Customers.v1;

public static class ActivateCustomerEndpoint
{
    internal static RouteHandlerBuilder MapActivateCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/activate", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new ActivateCustomerCommand(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(ActivateCustomerEndpoint))
        .WithSummary("Activate a customer")
        .WithDescription("Activates a customer to allow transactions")
        .Produces<ActivateCustomerResponse>()
        .RequirePermission("Permissions.Store.Update")
        .MapToApiVersion(1);
    }
}

