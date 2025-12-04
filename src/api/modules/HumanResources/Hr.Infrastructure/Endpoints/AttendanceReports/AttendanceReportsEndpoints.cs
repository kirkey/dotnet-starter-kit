using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Generate.v1;
using FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Export.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.AttendanceReports;

/// <summary>
/// Attendance Reports endpoints using Carter module pattern.
/// </summary>
public class AttendanceReportsEndpoints : ICarterModule
{
    /// <summary>
    /// Adds attendance report routes to the endpoint route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/attendance-reports").WithTags("attendance-reports");

        // Generate attendance report
        group.MapPost("/generate", async (GenerateAttendanceReportCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute("GenerateAttendanceReportEndpoint", new { id = response.ReportId }, response);
        })
        .WithName("GenerateAttendanceReportEndpoint")
        .WithSummary("Generate attendance report")
        .WithDescription("Generates an attendance report based on specified criteria and report type")
        .Produces<GenerateAttendanceReportResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Attendance))
        .MapToApiVersion(1);

        // Get attendance report by ID
        group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var request = new GetAttendanceReportRequest(id);
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetAttendanceReportEndpoint")
        .WithSummary("Get attendance report")
        .WithDescription("Retrieves an attendance report by ID with all details")
        .Produces<AttendanceReportResponse>()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Attendance))
        .MapToApiVersion(1);

        // Search attendance reports
        group.MapPost("/search", async (SearchAttendanceReportsRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchAttendanceReportsEndpoint")
        .WithSummary("Search attendance reports")
        .WithDescription("Searches and filters attendance reports with pagination support")
        .Produces<PagedList<AttendanceReportDto>>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Attendance))
        .MapToApiVersion(1);

        // Download attendance report
        group.MapGet("/{id}/download", (DefaultIdType id, ISender mediator) =>
        {
            // TODO: Implement report download logic
            // Return file stream with report data
            return Results.Ok(new { message = "Report download functionality to be implemented" });
        })
        .WithName("DownloadAttendanceReportEndpoint")
        .WithSummary("Download attendance report")
        .WithDescription("Downloads an attendance report in specified format (PDF/Excel)")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Attendance))
        .MapToApiVersion(1);

        // Export attendance report
        group.MapPost("/{id}/export", (DefaultIdType id, ExportAttendanceReportRequest request, ISender mediator) =>
        {
            // TODO: Implement report export logic
            // Support formats: CSV, Excel, PDF, JSON
            return Results.Ok(new { message = "Report export functionality to be implemented" });
        })
        .WithName("ExportAttendanceReportEndpoint")
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

