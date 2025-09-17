using Accounting.Application.FixedAssets.Dtos;
using Accounting.Application.FixedAssets.Search;

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
            .Produces<PagedList<FixedAssetDto>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


