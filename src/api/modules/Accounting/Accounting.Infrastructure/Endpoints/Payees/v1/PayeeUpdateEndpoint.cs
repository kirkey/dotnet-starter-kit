using Accounting.Application.Payees.Update.v1;

namespace Accounting.Infrastructure.Endpoints.Payees.v1;

public static class PayeeUpdateEndpoint
{
    internal static RouteHandlerBuilder MapPayeeUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, PayeeUpdateCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest();
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PayeeUpdateEndpoint))
            .WithSummary("update a payee")
            .WithDescription("update a payee")
            .Produces<PayeeUpdateResponse>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
