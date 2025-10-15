using Accounting.Application.CreditMemos.Approve;

namespace Accounting.Infrastructure.Endpoints.CreditMemos.v1;

public static class CreditMemoApproveEndpoint
{
    internal static RouteHandlerBuilder MapCreditMemoApproveEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveCreditMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(CreditMemoApproveEndpoint))
            .WithSummary("Approve a credit memo")
            .WithDescription("Approve a draft credit memo for application or refund")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Accounting.Approve")
            .MapToApiVersion(1);
    }
}
