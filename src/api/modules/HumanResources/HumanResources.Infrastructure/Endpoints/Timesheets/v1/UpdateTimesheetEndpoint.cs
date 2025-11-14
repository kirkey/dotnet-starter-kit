using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Timesheets.v1;

public static class UpdateTimesheetEndpoint
{
    internal static RouteHandlerBuilder MapUpdateTimesheetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateTimesheetCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateTimesheetEndpoint))
            .WithSummary("Updates a timesheet")
            .WithDescription("Updates timesheet status and approval information")
            .Produces<UpdateTimesheetResponse>()
            .RequirePermission("Permissions.Timesheets.Edit")
            .MapToApiVersion(1);
    }
}

