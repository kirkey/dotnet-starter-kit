using Accounting.Application.WriteOffs.RecordRecovery.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.WriteOffs.v1;

public static class WriteOffRecordRecoveryEndpoint
{
    internal static RouteHandlerBuilder MapWriteOffRecordRecoveryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/recovery", async (DefaultIdType id, RecordRecoveryCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var writeOffId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = writeOffId, Message = "Recovery recorded successfully" });
            })
            .WithName(nameof(WriteOffRecordRecoveryEndpoint))
            .WithSummary("Record recovery")
            .WithDescription("Records recovery of a previously written-off amount")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

