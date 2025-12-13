using Accounting.Application.DepreciationMethods.Deactivate.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.DepreciationMethods.v1;

public static class DepreciationMethodDeactivateEndpoint
{
    internal static RouteHandlerBuilder MapDepreciationMethodDeactivateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/deactivate", async (DefaultIdType id, ISender mediator) =>
            {
                var methodId = await mediator.Send(new DeactivateDepreciationMethodCommand(id)).ConfigureAwait(false);
                return Results.Ok(new { Id = methodId, Message = "Depreciation method deactivated successfully" });
            })
            .WithName(nameof(DepreciationMethodDeactivateEndpoint))
            .WithSummary("Deactivate depreciation method")
            .WithDescription("Deactivates a depreciation method")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

