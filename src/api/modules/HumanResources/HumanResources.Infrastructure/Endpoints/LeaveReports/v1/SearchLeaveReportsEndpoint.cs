namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveReports.v1;

using FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Search.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for searching leave reports.
/// </summary>
public static class SearchLeaveReportsEndpoint
{
    /// <summary>
    /// Maps the search leave reports endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapSearchLeaveReportsEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/search", async (SearchLeaveReportsRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(SearchLeaveReportsEndpoint))
        .WithSummary("Search leave reports")
        .WithDescription("Searches and filters leave reports with pagination support")
        .Produces<PagedList<LeaveReportDto>>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Leaves))
        .MapToApiVersion(1);
    }
}

