using Accounting.Application.RegulatoryReports.Update.v1;

namespace Accounting.Infrastructure.Endpoints.RegulatoryReports.v1;

public static class RegulatoryReportUpdateEndpoint
{
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
        .Produces<DefaultIdType>()
        .WithOpenApi();

        return group;
    }
}

