using Accounting.Application.FixedAssets.Reject.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FixedAssets.v1;

public static class FixedAssetRejectEndpoint
{
    internal static RouteHandlerBuilder MapFixedAssetRejectEndpoint(this IEndpointRouteBuilder endpoints)
        => endpoints
            .MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectFixedAssetCommand command, ISender mediator) =>
            {
                if (id != command.FixedAssetId) return Results.BadRequest("ID in URL does not match ID in request body.");
                var result = await mediator.Send(command).ConfigureAwait(false);
                return TypedResults.Ok(result);
            })
            .WithName(nameof(FixedAssetRejectEndpoint))
            .WithSummary("Reject fixed asset")
            .WithDescription("Rejects a fixed asset")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Accounting))
            .MapToApiVersion(1);
}

