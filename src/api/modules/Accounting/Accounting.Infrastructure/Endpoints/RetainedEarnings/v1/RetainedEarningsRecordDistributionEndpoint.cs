using Accounting.Application.RetainedEarnings.RecordDistribution.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RetainedEarnings.v1;

public static class RetainedEarningsRecordDistributionEndpoint
{
    internal static RouteHandlerBuilder MapRetainedEarningsRecordDistributionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/distributions", async (DefaultIdType id, RecordDistributionCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var reId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = reId, Message = "Distribution recorded successfully" });
            })
            .WithName(nameof(RetainedEarningsRecordDistributionEndpoint))
            .WithSummary("Record distribution")
            .WithDescription("Records a distribution to members or shareholders")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

