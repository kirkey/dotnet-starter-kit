using Accounting.Application.DebitMemos.Update;

namespace Accounting.Infrastructure.Endpoints.DebitMemos.v1;

public static class DebitMemoUpdateEndpoint
{
    internal static RouteHandlerBuilder MapDebitMemoUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateDebitMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(DebitMemoUpdateEndpoint))
            .WithSummary("Update a debit memo")
            .WithDescription("Update an existing debit memo (draft only)")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
