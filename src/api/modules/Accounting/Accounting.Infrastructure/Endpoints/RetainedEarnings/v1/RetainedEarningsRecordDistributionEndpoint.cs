using Accounting.Application.RetainedEarnings.RecordDistribution.v1;

namespace Accounting.Infrastructure.Endpoints.RetainedEarnings.v1;

public static class RetainedEarningsRecordDistributionEndpoint
{
    internal static RouteHandlerBuilder MapRetainedEarningsRecordDistributionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/distributions", async (DefaultIdType id, RecordDistributionCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var reId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = reId, Message = "Distribution recorded successfully" });
            })
            .WithName(nameof(RetainedEarningsRecordDistributionEndpoint))
            .WithSummary("Record distribution")
            .WithDescription("Records a distribution to members or shareholders")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

