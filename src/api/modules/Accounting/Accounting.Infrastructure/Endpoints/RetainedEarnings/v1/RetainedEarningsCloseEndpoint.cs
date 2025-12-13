using Accounting.Application.RetainedEarnings.Close.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RetainedEarnings.v1;

public static class RetainedEarningsCloseEndpoint
{
    internal static RouteHandlerBuilder MapRetainedEarningsCloseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/close", async (DefaultIdType id, CloseRetainedEarningsCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var reId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = reId, Message = "Fiscal year closed successfully" });
            })
            .WithName(nameof(RetainedEarningsCloseEndpoint))
            .WithSummary("Close fiscal year")
            .WithDescription("Closes the fiscal year for retained earnings")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

