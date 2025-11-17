using Accounting.Application.FixedAssets.Get;
using Accounting.Application.FixedAssets.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FixedAssets.v1;

public static class FixedAssetGetEndpoint
{
    internal static RouteHandlerBuilder MapFixedAssetGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetFixedAssetRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(FixedAssetGetEndpoint))
            .WithSummary("get a fixed asset by id")
            .WithDescription("get a fixed asset by id")
            .Produces<FixedAssetResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


