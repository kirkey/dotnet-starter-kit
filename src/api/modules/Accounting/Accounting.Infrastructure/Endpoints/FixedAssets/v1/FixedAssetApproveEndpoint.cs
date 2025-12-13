using Accounting.Application.FixedAssets.Approve.v1;

namespace Accounting.Infrastructure.Endpoints.FixedAssets.v1;

public static class FixedAssetApproveEndpoint
{
    internal static RouteHandlerBuilder MapFixedAssetApproveEndpoint(this IEndpointRouteBuilder endpoints)
        => endpoints
            .MapPost("/{id}/approve", async (DefaultIdType id, ApproveFixedAssetCommand command, ISender mediator) =>
            {
                if (id != command.FixedAssetId) return Results.BadRequest("ID in URL does not match ID in request body.");
                var result = await mediator.Send(command).ConfigureAwait(false);
                return TypedResults.Ok(result);
            })
            .WithName(nameof(FixedAssetApproveEndpoint))
            .WithSummary("Approve fixed asset")
            .WithDescription("Approves a fixed asset for activation")
            .Produces<DefaultIdType>();
}

