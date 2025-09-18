using Accounting.Application.RegulatoryReports.Create.v1;

namespace Accounting.Infrastructure.Endpoints.RegulatoryReports.v1;

public static class RegulatoryReportCreateEndpoint
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
            .Produces<DefaultIdType>()
            .WithOpenApi();

        return group;
    }
}
