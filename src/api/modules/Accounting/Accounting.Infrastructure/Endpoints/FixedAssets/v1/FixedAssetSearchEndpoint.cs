using Accounting.Application.FixedAssets.Responses;
using Accounting.Application.FixedAssets.Search;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FixedAssets.v1;

public static class FixedAssetSearchEndpoint
{
    internal static RouteHandlerBuilder MapFixedAssetSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchFixedAssetsRequest command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(FixedAssetSearchEndpoint))
            .WithSummary("Gets a list of fixed assets")
            .WithDescription("Gets a list of fixed assets with pagination and filtering support")
            .Produces<PagedList<FixedAssetResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


