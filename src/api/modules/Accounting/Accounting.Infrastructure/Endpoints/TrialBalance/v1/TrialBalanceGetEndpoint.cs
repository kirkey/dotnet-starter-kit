using Accounting.Application.TrialBalance.Get.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.TrialBalance.v1;

/// <summary>
/// Endpoint for retrieving a trial balance by ID.
/// </summary>
public static class TrialBalanceGetEndpoint
{
    internal static RouteHandlerBuilder MapTrialBalanceGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new TrialBalanceGetRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(TrialBalanceGetEndpoint))
            .WithSummary("Get trial balance by ID")
            .WithDescription("Retrieves a trial balance report with all line items")
            .Produces<TrialBalanceGetResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

