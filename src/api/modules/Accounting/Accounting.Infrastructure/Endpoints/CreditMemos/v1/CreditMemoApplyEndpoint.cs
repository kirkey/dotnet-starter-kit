using Accounting.Application.CreditMemos.Apply;

namespace Accounting.Infrastructure.Endpoints.CreditMemos.v1;

public static class CreditMemoApplyEndpoint
{
    internal static RouteHandlerBuilder MapCreditMemoApplyEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/apply", async (DefaultIdType id, ApplyCreditMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(CreditMemoApplyEndpoint))
            .WithSummary("Apply a credit memo")
            .WithDescription("Apply an approved credit memo to an invoice or bill")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
