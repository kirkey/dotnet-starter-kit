using Accounting.Application.Consumptions.Get.v1;
using Accounting.Application.Consumptions.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Consumptions.v1;

/// <summary>
/// Endpoint for retrieving a consumption record by ID.
/// </summary>
public static class ConsumptionGetEndpoint
{
    internal static RouteHandlerBuilder MapConsumptionGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetConsumptionRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ConsumptionGetEndpoint))
            .WithSummary("Get consumption record")
            .WithDescription("Retrieves a consumption record by ID")
            .Produces<ConsumptionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

