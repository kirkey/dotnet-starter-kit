using Accounting.Application.DebitMemos.Delete;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.DebitMemos.v1;

public static class DebitMemoDeleteEndpoint
{
    internal static RouteHandlerBuilder MapDebitMemoDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeleteDebitMemoCommand(id);
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(DebitMemoDeleteEndpoint))
            .WithSummary("Delete a debit memo")
            .WithDescription("Delete a debit memo (draft status only)")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
