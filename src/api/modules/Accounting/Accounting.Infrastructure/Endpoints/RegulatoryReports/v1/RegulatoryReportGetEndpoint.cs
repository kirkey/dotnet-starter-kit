using Accounting.Application.RegulatoryReports.Get.v1;
using Accounting.Application.RegulatoryReports.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RegulatoryReports.v1;

public static class RegulatoryReportGetEndpoint
{
    internal static RouteHandlerBuilder MapRegulatoryReportGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetRegulatoryReportRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(RegulatoryReportGetEndpoint))
            .WithSummary("Get regulatory report by ID")
            .WithDescription("Retrieves a regulatory report by its unique identifier")
            .Produces<RegulatoryReportResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

