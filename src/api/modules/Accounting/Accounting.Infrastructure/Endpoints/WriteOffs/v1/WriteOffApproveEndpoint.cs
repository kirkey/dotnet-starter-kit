using Accounting.Application.WriteOffs.Approve.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.WriteOffs.v1;

public static class WriteOffApproveEndpoint
{
    internal static RouteHandlerBuilder MapWriteOffApproveEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/approve", async (DefaultIdType id, ApproveWriteOffCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var writeOffId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = writeOffId, Message = "Write-off approved successfully" });
            })
            .WithName(nameof(WriteOffApproveEndpoint))
            .WithSummary("Approve write-off")
            .WithDescription("Approves a pending write-off for posting")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

