using Accounting.Application.DepreciationMethods.Activate.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.DepreciationMethods.v1;

public static class DepreciationMethodActivateEndpoint
{
    internal static RouteHandlerBuilder MapDepreciationMethodActivateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/activate", async (DefaultIdType id, ISender mediator) =>
            {
                var methodId = await mediator.Send(new ActivateDepreciationMethodCommand(id)).ConfigureAwait(false);
                return Results.Ok(new { Id = methodId, Message = "Depreciation method activated successfully" });
            })
            .WithName(nameof(DepreciationMethodActivateEndpoint))
            .WithSummary("Activate depreciation method")
            .WithDescription("Activates a depreciation method for use")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

