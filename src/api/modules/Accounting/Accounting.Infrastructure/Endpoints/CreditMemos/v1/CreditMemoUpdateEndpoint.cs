using Accounting.Application.CreditMemos.Update;

namespace Accounting.Infrastructure.Endpoints.CreditMemos.v1;

public static class CreditMemoUpdateEndpoint
{
    internal static RouteHandlerBuilder MapCreditMemoUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateCreditMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(CreditMemoUpdateEndpoint))
            .WithSummary("Update a credit memo")
            .WithDescription("Update an existing credit memo (draft only)")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
