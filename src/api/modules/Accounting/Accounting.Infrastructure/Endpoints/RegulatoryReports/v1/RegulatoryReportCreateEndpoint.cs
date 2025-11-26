using Accounting.Application.RegulatoryReports.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RegulatoryReports.v1;

public static class RegulatoryReportCreateEndpoint
{
    internal static RouteHandlerBuilder MapRegulatoryReportCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (RegulatoryReportCreateRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(RegulatoryReportCreateEndpoint))
            .WithSummary("Create a new regulatory report")
            .WithDescription("Creates a new regulatory report")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
