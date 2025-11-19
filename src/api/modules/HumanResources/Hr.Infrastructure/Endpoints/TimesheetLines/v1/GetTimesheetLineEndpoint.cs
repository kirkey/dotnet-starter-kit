using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TimesheetLines.v1;

/// <summary>
/// Endpoint for retrieving timesheet line details.
/// </summary>
public static class GetTimesheetLineEndpoint
{
    internal static RouteHandlerBuilder MapGetTimesheetLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetTimesheetLineRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetTimesheetLineEndpoint))
            .WithSummary("Gets timesheet line details")
            .WithDescription("Retrieves detailed information about a specific timesheet line entry")
            .Produces<TimesheetLineResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Timesheets))
            .MapToApiVersion(1);
    }
}

