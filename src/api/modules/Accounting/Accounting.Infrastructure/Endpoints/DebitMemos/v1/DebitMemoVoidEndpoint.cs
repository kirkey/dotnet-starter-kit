using Accounting.Application.DebitMemos.Void;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.DebitMemos.v1;

public static class DebitMemoVoidEndpoint
{
    internal static RouteHandlerBuilder MapDebitMemoVoidEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/void", async (DefaultIdType id, VoidDebitMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(DebitMemoVoidEndpoint))
            .WithSummary("Void a debit memo")
            .WithDescription("Void a debit memo and reverse any applications")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
