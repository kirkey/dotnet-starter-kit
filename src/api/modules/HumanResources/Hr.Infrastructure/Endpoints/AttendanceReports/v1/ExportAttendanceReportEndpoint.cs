using System.ComponentModel;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.AttendanceReports.v1;

using FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Export.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for exporting attendance reports.
/// </summary>
public static class ExportAttendanceReportEndpoint
{
    /// <summary>
    /// Maps the export attendance report endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapExportAttendanceReportEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/{id}/export", (DefaultIdType id, ExportAttendanceReportRequest request, ISender mediator) =>
        {
            // TODO: Implement report export logic
            // Support formats: CSV, Excel, PDF, JSON
            return Results.Ok(new { message = "Report export functionality to be implemented" });
        })
        .WithName(nameof(ExportAttendanceReportEndpoint))
        .WithSummary("Export attendance report")
        .WithDescription("Exports an attendance report in specified format")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Attendance))
        .MapToApiVersion(1);
    }
}


