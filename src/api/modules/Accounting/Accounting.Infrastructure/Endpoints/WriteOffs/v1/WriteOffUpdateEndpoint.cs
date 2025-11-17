using Accounting.Application.WriteOffs.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.WriteOffs.v1;

public static class WriteOffUpdateEndpoint
{
    internal static RouteHandlerBuilder MapWriteOffUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateWriteOffCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var writeOffId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = writeOffId });
            })
            .WithName(nameof(WriteOffUpdateEndpoint))
            .WithSummary("Update write-off")
            .WithDescription("Updates a pending write-off details")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

