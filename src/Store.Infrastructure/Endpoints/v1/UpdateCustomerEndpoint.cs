namespace Store.Infrastructure.Endpoints.v1;

public static class UpdateCustomerEndpoint
{
    internal static RouteHandlerBuilder MapUpdateCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/customers/{id:guid}", async (Guid id, FSH.Starter.WebApi.Store.Application.Customers.Update.v1.UpdateCustomerCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateCustomer")
        .WithSummary("Update customer")
        .WithDescription("Updates an existing customer")
        .MapToApiVersion(1);
    }
}

