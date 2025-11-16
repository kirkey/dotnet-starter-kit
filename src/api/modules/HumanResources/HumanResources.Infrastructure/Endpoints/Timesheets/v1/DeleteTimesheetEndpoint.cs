using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Timesheets.v1;

public static class DeleteTimesheetEndpoint
{
    internal static RouteHandlerBuilder MapDeleteTimesheetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteTimesheetCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteTimesheetEndpoint))
            .WithSummary("Deletes a timesheet")
            .WithDescription("Deletes a timesheet record")
            .Produces<DeleteTimesheetResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Timesheets))
            .MapToApiVersion(1);
    }
}

