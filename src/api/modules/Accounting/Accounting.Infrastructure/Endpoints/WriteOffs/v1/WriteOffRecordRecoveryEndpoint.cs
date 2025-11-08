using Accounting.Application.WriteOffs.RecordRecovery.v1;

namespace Accounting.Infrastructure.Endpoints.WriteOffs.v1;

public static class WriteOffRecordRecoveryEndpoint
{
    internal static RouteHandlerBuilder MapWriteOffRecordRecoveryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/recovery", async (DefaultIdType id, RecordRecoveryCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var writeOffId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = writeOffId, Message = "Recovery recorded successfully" });
            })
            .WithName(nameof(WriteOffRecordRecoveryEndpoint))
            .WithSummary("Record recovery")
            .WithDescription("Records recovery of a previously written-off amount")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

