using FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Search.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DesignationAssignments.v1;

/// <summary>
/// Endpoint for searching employee designation history with temporal queries.
/// </summary>
public static class SearchEmployeeHistoryEndpoint
{
    internal static RouteHandlerBuilder MapSearchEmployeeHistoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/history/search", async (SearchEmployeeHistoryRequest request, ISender mediator) =>
            {
                var result = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName(nameof(SearchEmployeeHistoryEndpoint))
            .WithSummary("Search employee designation history")
            .WithDescription("Searches employee designation history with support for temporal queries, filtering by organization, designation, date range, and employment status")
            .Produces<PagedList<EmployeeHistoryDto>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

