namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.AttendanceReports.v1;

using FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Search.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for searching attendance reports.
/// </summary>
public static class SearchAttendanceReportsEndpoint
{
    /// <summary>
    /// Maps the search attendance reports endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapSearchAttendanceReportsEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/search", async (SearchAttendanceReportsRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(SearchAttendanceReportsEndpoint))
        .WithSummary("Search attendance reports")
        .WithDescription("Searches and filters attendance reports with pagination support")
        .Produces<PagedList<AttendanceReportDto>>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Attendance))
        .MapToApiVersion(1);
    }
}

