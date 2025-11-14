using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Timesheets.v1;

public static class GetTimesheetEndpoint
{
    internal static RouteHandlerBuilder MapGetTimesheetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetTimesheetRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetTimesheetEndpoint))
            .WithSummary("Gets timesheet by ID")
            .WithDescription("Retrieves timesheet details")
            .Produces<TimesheetResponse>()
            .RequirePermission("Permissions.Timesheets.View")
            .MapToApiVersion(1);
    }
}

