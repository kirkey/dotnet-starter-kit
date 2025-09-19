using Accounting.Application.FixedAssets.Create;

namespace Accounting.Infrastructure.Endpoints.FixedAssets.v1;

public static class FixedAssetCreateEndpoint
{
    internal static RouteHandlerBuilder MapFixedAssetCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateFixedAssetCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(FixedAssetCreateEndpoint))
            .WithSummary("create a fixed asset")
            .WithDescription("create a fixed asset")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

