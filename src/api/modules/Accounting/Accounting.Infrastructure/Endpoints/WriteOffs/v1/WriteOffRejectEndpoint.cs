using Accounting.Application.WriteOffs.Reject.v1;

namespace Accounting.Infrastructure.Endpoints.WriteOffs.v1;

public static class WriteOffRejectEndpoint
{
    internal static RouteHandlerBuilder MapWriteOffRejectEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectWriteOffCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var writeOffId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = writeOffId, Message = "Write-off rejected successfully" });
            })
            .WithName(nameof(WriteOffRejectEndpoint))
            .WithSummary("Reject write-off")
            .WithDescription("Rejects a pending write-off")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

