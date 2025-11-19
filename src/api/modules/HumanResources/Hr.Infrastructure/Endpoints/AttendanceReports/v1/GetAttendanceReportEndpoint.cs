namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.AttendanceReports.v1;

using FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Get.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for retrieving an attendance report by ID.
/// </summary>
public static class GetAttendanceReportEndpoint
{
    /// <summary>
    /// Maps the get attendance report endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapGetAttendanceReportEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var request = new GetAttendanceReportRequest(id);
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetAttendanceReportEndpoint))
        .WithSummary("Get attendance report")
        .WithDescription("Retrieves an attendance report by ID with all details")
        .Produces<AttendanceReportResponse>()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Attendance))
        .MapToApiVersion(1);
    }
}

