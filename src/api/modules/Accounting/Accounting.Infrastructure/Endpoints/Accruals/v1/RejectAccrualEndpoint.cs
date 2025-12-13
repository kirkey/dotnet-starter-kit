using Accounting.Application.Accruals.Reject;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class RejectAccrualEndpoint
{
    internal static RouteHandlerBuilder MapRejectAccrualEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id}/reject", async (DefaultIdType id, RejectAccrualCommand command, ISender mediator) =>
        {
            if (id != command.AccrualId) return Results.BadRequest("ID in URL does not match ID in request body.");
            var result = await mediator.Send(command).ConfigureAwait(false);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(RejectAccrualEndpoint))
        .WithSummary("Reject accrual")
        .WithDescription("Rejects accrual entry")
        .Produces<DefaultIdType>()
        .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}
