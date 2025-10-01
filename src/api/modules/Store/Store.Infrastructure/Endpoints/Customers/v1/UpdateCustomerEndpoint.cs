using FSH.Starter.WebApi.Store.Application.Customers.Update.v1;

namespace Store.Infrastructure.Endpoints.Customers.v1;

public static class UpdateCustomerEndpoint
{
    internal static RouteHandlerBuilder MapUpdateCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateCustomerCommand request, ISender mediator) =>
        {
            if (id != request.Id) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateCustomerEndpoint))
        .WithSummary("Update customer")
        .WithDescription("Updates an existing customer")
        .Produces<UpdateCustomerResponse>()
        .RequirePermission("Permissions.Store.Update")
        .MapToApiVersion(1);
    }
}
