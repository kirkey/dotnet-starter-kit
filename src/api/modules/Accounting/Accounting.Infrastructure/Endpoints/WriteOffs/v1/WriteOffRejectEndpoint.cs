using Accounting.Application.WriteOffs.Reject.v1;

namespace Accounting.Infrastructure.Endpoints.WriteOffs.v1;

public static class WriteOffRejectEndpoint
{
    internal static RouteHandlerBuilder MapWriteOffRejectEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectWriteOffCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
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

