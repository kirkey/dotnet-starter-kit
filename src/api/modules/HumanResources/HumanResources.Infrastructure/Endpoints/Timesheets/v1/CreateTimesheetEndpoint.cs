using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Timesheets.v1;

public static class CreateTimesheetEndpoint
{
    internal static RouteHandlerBuilder MapCreateTimesheetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateTimesheetCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetTimesheetEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateTimesheetEndpoint))
            .WithSummary("Creates a new timesheet")
            .WithDescription("Creates a new timesheet for a pay period")
            .Produces<CreateTimesheetResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.Employees.Manage")
            .MapToApiVersion(1);
    }
}

