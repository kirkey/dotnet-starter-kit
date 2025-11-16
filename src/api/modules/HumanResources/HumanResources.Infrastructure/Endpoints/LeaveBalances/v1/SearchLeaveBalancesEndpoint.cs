using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveBalances.v1;

public static class SearchLeaveBalancesEndpoint
{
    internal static RouteHandlerBuilder MapSearchLeaveBalancesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchLeaveBalancesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchLeaveBalancesEndpoint))
            .WithSummary("Searches leave balances")
            .WithDescription("Searches and filters leave balances by employee, leave type, year with pagination support")
            .Produces<PagedList<LeaveBalanceResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves))
            .MapToApiVersion(1);
    }
}
