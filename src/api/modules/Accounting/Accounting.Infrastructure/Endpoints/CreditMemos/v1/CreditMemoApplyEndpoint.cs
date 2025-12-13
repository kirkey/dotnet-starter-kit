using Accounting.Application.CreditMemos.Apply;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.CreditMemos.v1;

public static class CreditMemoApplyEndpoint
{
    internal static RouteHandlerBuilder MapCreditMemoApplyEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/apply", async (DefaultIdType id, ApplyCreditMemoCommand command, ISender mediator) =>
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
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
