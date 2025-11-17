using Accounting.Application.FixedAssets.Delete;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FixedAssets.v1;

public static class FixedAssetDeleteEndpoint
{
    internal static RouteHandlerBuilder MapFixedAssetDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteFixedAssetRequest(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(FixedAssetDeleteEndpoint))
            .WithSummary("delete fixed asset by id")
            .WithDescription("delete fixed asset by id")
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

