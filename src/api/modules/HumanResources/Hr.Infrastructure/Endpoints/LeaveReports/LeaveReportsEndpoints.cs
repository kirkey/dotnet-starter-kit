using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Export.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Generate.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveReports;

/// <summary>
/// Leave Reports endpoints coordinator.
/// </summary>
public class LeaveReportsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all leave report endpoints.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/leave-reports").WithTags("leave-reports");

        group.MapPost("/generate", async (GenerateLeaveReportCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/hr/leave-reports/{response.ReportId}", response);
            })
            .WithName("GenerateLeaveReport")
            .WithSummary("Generate leave report")
            .WithDescription("Generates a leave report based on specified criteria and report type")
            .Produces<GenerateLeaveReportResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetLeaveReportRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetLeaveReport")
            .WithSummary("Get leave report")
            .WithDescription("Retrieves a leave report by ID with all details")
            .Produces<LeaveReportResponse>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchLeaveReportsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchLeaveReports")
            .WithSummary("Search leave reports")
            .WithDescription("Searches and filters leave reports with pagination support")
            .Produces<PagedList<LeaveReportDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapGet("/{id}/download", async (DefaultIdType id, ISender mediator) =>
            {
                return Results.Ok(new { message = "Report download functionality to be implemented" });
            })
            .WithName("DownloadLeaveReport")
            .WithSummary("Download leave report")
            .WithDescription("Downloads a leave report in specified format (PDF/Excel)")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapPost("/{id}/export", async (DefaultIdType id, ExportLeaveReportRequest request, ISender mediator) =>
            {
                return Results.Ok(new { message = "Report export functionality to be implemented" });
            })
            .WithName("ExportLeaveReport")
            .WithSummary("Export leave report")
            .WithDescription("Exports a leave report in specified format")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves))
            .MapToApiVersion(1);
    }
}

