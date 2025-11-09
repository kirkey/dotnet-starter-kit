using Accounting.Application.Accruals.Reject;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class AccrualRejectEndpoint
{
    internal static RouteHandlerBuilder MapAccrualRejectEndpoint(this IEndpointRouteBuilder endpoints)
        => endpoints
            .MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectAccrualCommand command, ISender mediator) =>
            {
                if (id != command.AccrualId) return Results.BadRequest("ID in URL does not match ID in request body.");
                var result = await mediator.Send(command).ConfigureAwait(false);
                return TypedResults.Ok(result);
            })
            .WithName(nameof(AccrualRejectEndpoint))
            .WithSummary("Reject accrual")
            .WithDescription("Rejects accrual entry")
            .Produces<DefaultIdType>();
}

