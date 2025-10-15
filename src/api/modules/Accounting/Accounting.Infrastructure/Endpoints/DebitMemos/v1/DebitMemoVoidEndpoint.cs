using Accounting.Application.DebitMemos.Void;

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
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
