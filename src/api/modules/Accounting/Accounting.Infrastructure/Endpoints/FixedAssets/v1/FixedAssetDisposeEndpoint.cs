using Accounting.Application.FixedAssets.Dispose.v1;

namespace Accounting.Infrastructure.Endpoints.FixedAssets.v1;

public static class FixedAssetDisposeEndpoint
{
    internal static RouteHandlerBuilder MapFixedAssetDisposeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/dispose", async (DefaultIdType id, DisposeFixedAssetCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var assetId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = assetId, Message = "Asset disposed successfully" });
            })
            .WithName(nameof(FixedAssetDisposeEndpoint))
            .WithSummary("Dispose asset")
            .WithDescription("Marks an asset as disposed and records disposal details")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

