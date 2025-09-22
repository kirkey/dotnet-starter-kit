using FSH.Starter.WebApi.Store.Application.Customers.CreditLimit.v1;

namespace Store.Infrastructure.Endpoints.Customers.v1;

public static class ChangeCustomerCreditLimitEndpoint
{
    internal static RouteHandlerBuilder MapChangeCustomerCreditLimitEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}/credit-limit", async (DefaultIdType id, ChangeCustomerCreditLimitCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(ChangeCustomerCreditLimitEndpoint))
        .WithSummary("Change customer credit limit")
        .WithDescription("Updates the credit limit for a customer")
        .Produces<ChangeCustomerCreditLimitResponse>()
        .RequirePermission("Permissions.Store.Update")
        .MapToApiVersion(1);
    }
}

