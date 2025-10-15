using Accounting.Application.DebitMemos.Apply;

namespace Accounting.Infrastructure.Endpoints.DebitMemos.v1;

public static class DebitMemoApplyEndpoint
{
    internal static RouteHandlerBuilder MapDebitMemoApplyEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/apply", async (DefaultIdType id, ApplyDebitMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(DebitMemoApplyEndpoint))
            .WithSummary("Apply a debit memo")
            .WithDescription("Apply an approved debit memo to an invoice or bill")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
