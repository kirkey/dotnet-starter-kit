using Accounting.Application.Accruals.Approve;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class AccrualApproveEndpoint
{
    internal static RouteHandlerBuilder MapAccrualApproveEndpoint(this IEndpointRouteBuilder endpoints)
        => endpoints
            .MapPost("/{id}/approve", async (DefaultIdType id, ApproveAccrualCommand command, ISender mediator) =>
            {
                if (id != command.AccrualId) return Results.BadRequest("ID in URL does not match ID in request body.");
                var result = await mediator.Send(command).ConfigureAwait(false);
                return TypedResults.Ok(result);
            })
            .WithName(nameof(AccrualApproveEndpoint))
            .WithSummary("Approve accrual")
            .WithDescription("Approves accrual entry")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Accounting))
            .MapToApiVersion(1);
}

