using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TimesheetLines.v1;

/// <summary>
/// Endpoint for updating timesheet line.
/// </summary>
public static class UpdateTimesheetLineEndpoint
{
    internal static RouteHandlerBuilder MapUpdateTimesheetLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateTimesheetLineCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateTimesheetLineEndpoint))
            .WithSummary("Updates timesheet line")
            .WithDescription("Updates hours, project allocation, or billing information for a timesheet line")
            .Produces<UpdateTimesheetLineResponse>()
            .RequirePermission("Permissions.TimesheetLines.Update")
            .MapToApiVersion(1);
    }
}
