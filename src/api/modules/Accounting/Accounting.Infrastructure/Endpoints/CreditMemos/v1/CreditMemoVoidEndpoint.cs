using Accounting.Application.CreditMemos.Void;

namespace Accounting.Infrastructure.Endpoints.CreditMemos.v1;

public static class CreditMemoVoidEndpoint
{
    internal static RouteHandlerBuilder MapCreditMemoVoidEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/void", async (DefaultIdType id, VoidCreditMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(CreditMemoVoidEndpoint))
            .WithSummary("Void a credit memo")
            .WithDescription("Void a credit memo and reverse any applications or refunds")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
