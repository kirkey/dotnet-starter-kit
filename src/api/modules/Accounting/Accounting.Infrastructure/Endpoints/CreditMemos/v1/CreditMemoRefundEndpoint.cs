using Accounting.Application.CreditMemos.Refund;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.CreditMemos.v1;

public static class CreditMemoRefundEndpoint
{
    internal static RouteHandlerBuilder MapCreditMemoRefundEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/refund", async (DefaultIdType id, RefundCreditMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(CreditMemoRefundEndpoint))
            .WithSummary("Issue refund for credit memo")
            .WithDescription("Issue a direct refund for an approved credit memo")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
