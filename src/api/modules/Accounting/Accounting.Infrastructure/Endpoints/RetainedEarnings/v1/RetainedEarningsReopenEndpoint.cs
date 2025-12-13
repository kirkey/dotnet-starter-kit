using Accounting.Application.RetainedEarnings.Reopen.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RetainedEarnings.v1;

public static class RetainedEarningsReopenEndpoint
{
    internal static RouteHandlerBuilder MapRetainedEarningsReopenEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/reopen", async (DefaultIdType id, ReopenRetainedEarningsCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var reId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = reId, Message = "Fiscal year reopened successfully" });
            })
            .WithName(nameof(RetainedEarningsReopenEndpoint))
            .WithSummary("Reopen fiscal year")
            .WithDescription("Reopens a closed fiscal year for retained earnings")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

