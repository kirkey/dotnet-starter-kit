using Accounting.Application.Consumptions.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Consumptions.v1;

/// <summary>
/// Endpoint for searching consumption records.
/// </summary>
public static class ConsumptionSearchEndpoint
{
    internal static RouteGroupBuilder MapConsumptionSearchEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/search", async (SearchConsumptionsRequest request, ISender mediator) =>
        {
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(ConsumptionSearchEndpoint))
        .WithSummary("Search consumption records")
        .WithDescription("Search consumption records with filters and pagination")
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        return group;
    }
}

