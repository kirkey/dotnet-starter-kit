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
                return Results.Created($"/accounting/fixed-assets/{response.Id}", response);
            })
            .WithName(nameof(FixedAssetCreateEndpoint))
            .WithSummary("create a fixed asset")
            .WithDescription("create a fixed asset")
            .Produces<CreateFixedAssetResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
