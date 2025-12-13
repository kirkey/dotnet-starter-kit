using Accounting.Application.FixedAssets.Depreciate;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FixedAssets.v1;

public static class FixedAssetDepreciateEndpoint
{
    internal static RouteHandlerBuilder MapFixedAssetDepreciateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/depreciate", async (DefaultIdType id, DepreciateFixedAssetCommand command, ISender mediator) =>
            {
                if (id != command.FixedAssetId) return Results.BadRequest("ID in URL does not match ID in request body.");
                var assetId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = assetId, Message = "Depreciation recorded successfully" });
            })
            .WithName(nameof(FixedAssetDepreciateEndpoint))
            .WithSummary("Record depreciation")
            .WithDescription("Records depreciation expense and reduces book value")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

