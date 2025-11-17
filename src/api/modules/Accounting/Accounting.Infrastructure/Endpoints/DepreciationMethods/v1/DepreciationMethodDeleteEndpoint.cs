using Accounting.Application.DepreciationMethods.Delete;
using Shared.Authorization;

// Endpoint for deleting a depreciation method
namespace Accounting.Infrastructure.Endpoints.DepreciationMethods.v1;

public static class DepreciationMethodDeleteEndpoint
{
    internal static RouteHandlerBuilder MapDepreciationMethodDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteDepreciationMethodRequest(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(DepreciationMethodDeleteEndpoint))
            .WithSummary("Delete a depreciation method")
            .WithDescription("Deletes a depreciation method by its ID")
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
