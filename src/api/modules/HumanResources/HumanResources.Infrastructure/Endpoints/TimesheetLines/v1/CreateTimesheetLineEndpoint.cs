using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TimesheetLines.v1;

/// <summary>
/// Endpoint for creating a timesheet line.
/// </summary>
public static class CreateTimesheetLineEndpoint
{
    internal static RouteHandlerBuilder MapCreateTimesheetLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateTimesheetLineCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetTimesheetLineEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateTimesheetLineEndpoint))
            .WithSummary("Creates a new timesheet line")
            .WithDescription("Creates a new daily timesheet entry with hours and project allocation")
            .Produces<CreateTimesheetLineResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Timesheets))
            .MapToApiVersion(1);
    }
}

