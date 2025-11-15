using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TimesheetLines.v1;

/// <summary>
/// Endpoint for deleting a timesheet line.
/// </summary>
public static class DeleteTimesheetLineEndpoint
{
    internal static RouteHandlerBuilder MapDeleteTimesheetLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteTimesheetLineCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteTimesheetLineEndpoint))
            .WithSummary("Deletes a timesheet line")
            .WithDescription("Deletes a specific timesheet line entry from the system")
            .Produces<DeleteTimesheetLineResponse>()
            .RequirePermission("Permissions.TimesheetLines.Delete")
            .MapToApiVersion(1);
    }
}

