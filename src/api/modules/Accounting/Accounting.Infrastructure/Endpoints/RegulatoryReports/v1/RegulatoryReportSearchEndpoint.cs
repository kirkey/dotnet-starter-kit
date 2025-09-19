using Accounting.Application.RegulatoryReports.Responses;
using Accounting.Application.RegulatoryReports.Search.v1;

namespace Accounting.Infrastructure.Endpoints.RegulatoryReports.v1;

public static class RegulatoryReportSearchEndpoint
{
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
        .Produces<List<RegulatoryReportResponse>>()
        .WithOpenApi();

        return group;
    }
}

