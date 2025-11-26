using Accounting.Application.RegulatoryReports.Responses;
using Accounting.Application.RegulatoryReports.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RegulatoryReports.v1;

public static class RegulatoryReportSearchEndpoint
{
    internal static RouteHandlerBuilder MapRegulatoryReportSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchRegulatoryReportsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(RegulatoryReportSearchEndpoint))
            .WithSummary("Search regulatory reports")
            .WithDescription("Searches regulatory reports with filtering support")
            .Produces<List<RegulatoryReportResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

