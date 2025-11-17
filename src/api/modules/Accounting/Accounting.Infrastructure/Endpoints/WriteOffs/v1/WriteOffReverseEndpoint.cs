using Accounting.Application.WriteOffs.Reverse.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.WriteOffs.v1;

public static class WriteOffReverseEndpoint
{
    internal static RouteHandlerBuilder MapWriteOffReverseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reverse", async (DefaultIdType id, ReverseWriteOffCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var writeOffId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = writeOffId, Message = "Write-off reversed successfully" });
            })
            .WithName(nameof(WriteOffReverseEndpoint))
            .WithSummary("Reverse write-off")
            .WithDescription("Reverses a posted write-off")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

