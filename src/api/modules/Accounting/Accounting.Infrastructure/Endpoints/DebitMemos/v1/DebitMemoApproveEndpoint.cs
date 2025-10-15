using Accounting.Application.DebitMemos.Approve;

namespace Accounting.Infrastructure.Endpoints.DebitMemos.v1;

public static class DebitMemoApproveEndpoint
{
    internal static RouteHandlerBuilder MapDebitMemoApproveEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveDebitMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(DebitMemoApproveEndpoint))
            .WithSummary("Approve a debit memo")
            .WithDescription("Approve a draft debit memo for application")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Accounting.Approve")
            .MapToApiVersion(1);
    }
}
