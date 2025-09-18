using Accounting.Application.RegulatoryReports.Get.v1;
using Accounting.Application.RegulatoryReports.Dtos;

namespace Accounting.Infrastructure.Endpoints.RegulatoryReports.v1;

public static class RegulatoryReportGetEndpoint
{
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
        .Produces<RegulatoryReportDto>()
        .WithOpenApi();

        return group;
    }
}

