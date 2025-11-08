using Accounting.Application.DepreciationMethods.Deactivate.v1;

namespace Accounting.Infrastructure.Endpoints.DepreciationMethods.v1;

public static class DepreciationMethodDeactivateEndpoint
{
    internal static RouteHandlerBuilder MapDepreciationMethodDeactivateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/deactivate", async (DefaultIdType id, ISender mediator) =>
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
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

