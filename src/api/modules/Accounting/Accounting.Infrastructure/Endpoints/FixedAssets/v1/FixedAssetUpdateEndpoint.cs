using Accounting.Application.FixedAssets.Update;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FixedAssets.v1;

public static class FixedAssetUpdateEndpoint
{
    internal static RouteHandlerBuilder MapFixedAssetUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateFixedAssetCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(FixedAssetUpdateEndpoint))
            .WithSummary("update a fixed asset")
            .WithDescription("update a fixed asset")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

