using Accounting.Application.RegulatoryReports.Responses;
using Accounting.Application.RegulatoryReports.Search.v1;
using Shared.Authorization;

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
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .Produces<List<RegulatoryReportResponse>>()
        .WithOpenApi();

        return group;
    }
}

