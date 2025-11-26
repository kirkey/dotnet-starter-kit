using Accounting.Application.RegulatoryReports.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RegulatoryReports.v1;

public static class RegulatoryReportUpdateEndpoint
{
    internal static RouteHandlerBuilder MapRegulatoryReportUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateRegulatoryReportRequest request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID in URL does not match ID in request body");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(RegulatoryReportUpdateEndpoint))
            .WithSummary("Update regulatory report")
            .WithDescription("Updates an existing regulatory report")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

