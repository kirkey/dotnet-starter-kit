using Accounting.Application.CreditMemos.Delete;

namespace Accounting.Infrastructure.Endpoints.CreditMemos.v1;

public static class CreditMemoDeleteEndpoint
{
    internal static RouteHandlerBuilder MapCreditMemoDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeleteCreditMemoCommand(id);
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(CreditMemoDeleteEndpoint))
            .WithSummary("Delete a credit memo")
            .WithDescription("Delete a credit memo (draft status only)")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
