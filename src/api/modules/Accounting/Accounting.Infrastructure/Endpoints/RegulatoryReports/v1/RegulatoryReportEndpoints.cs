using Accounting.Application.RegulatoryReports.Create.v1;
using Accounting.Application.RegulatoryReports.Get.v1;
using Accounting.Application.RegulatoryReports.Search.v1;
using Accounting.Application.RegulatoryReports.Update.v1;

namespace Accounting.Infrastructure.Endpoints.RegulatoryReports.v1;

public static class RegulatoryReportEndpoints
{
    internal static RouteGroupBuilder MapRegulatoryReportCreateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (RegulatoryReportCreateRequest request, ISender mediator) =>
        {
            var reportId = await mediator.Send(request);
            return Results.Ok(reportId);
        })
        .WithName(nameof(RegulatoryReportCreateRequest))
        .WithSummary("Create a new regulatory report")
        .RequirePermission("Permissions.RegulatoryReports.Create")
        .WithOpenApi();

        return group;
    }

    internal static RouteGroupBuilder MapRegulatoryReportGetEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var report = await mediator.Send(new GetRegulatoryReportRequest(id));
            return Results.Ok(report);
        })
        .WithName(nameof(GetRegulatoryReportRequest))
        .WithSummary("Get regulatory report by ID")
        .RequirePermission("Permissions.RegulatoryReports.View")
        .WithOpenApi();

        return group;
    }

    internal static RouteGroupBuilder MapRegulatoryReportSearchEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/search", async (SearchRegulatoryReportsRequest request, ISender mediator) =>
        {
            var reports = await mediator.Send(request);
            return Results.Ok(reports);
        })
        .WithName(nameof(SearchRegulatoryReportsRequest))
        .WithSummary("Search regulatory reports")
        .RequirePermission("Permissions.RegulatoryReports.View")
        .WithOpenApi();

        return group;
    }

    internal static RouteGroupBuilder MapRegulatoryReportUpdateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateRegulatoryReportRequest request, ISender mediator) =>
        {
            if (id != request.Id)
                return Results.BadRequest("ID in URL does not match ID in request body");

            var reportId = await mediator.Send(request);
            return Results.Ok(reportId);
        })
        .WithName(nameof(UpdateRegulatoryReportRequest))
        .WithSummary("Update regulatory report")
        .RequirePermission("Permissions.RegulatoryReports.Edit")
        .WithOpenApi();

        return group;
    }
}
