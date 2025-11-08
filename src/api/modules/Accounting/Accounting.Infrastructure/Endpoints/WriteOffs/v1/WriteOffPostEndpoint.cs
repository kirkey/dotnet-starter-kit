using Accounting.Application.WriteOffs.Post.v1;

namespace Accounting.Infrastructure.Endpoints.WriteOffs.v1;

public static class WriteOffPostEndpoint
{
    internal static RouteHandlerBuilder MapWriteOffPostEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/post", async (DefaultIdType id, PostWriteOffCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var writeOffId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = writeOffId, Message = "Write-off posted to general ledger successfully" });
            })
            .WithName(nameof(WriteOffPostEndpoint))
            .WithSummary("Post write-off to GL")
            .WithDescription("Posts an approved write-off to the general ledger")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

