using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TimesheetLines;

/// <summary>
/// Endpoint configuration for TimesheetLines module.
/// </summary>
public class TimesheetLinesEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all TimesheetLines endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/timesheet-lines").WithTags("timesheet-lines");

        group.MapPost("/", async (CreateTimesheetLineCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetTimesheetLine", new { id = response.Id }, response);
            })
            .WithName("CreateTimesheetLine")
            .WithSummary("Creates a new timesheet line")
            .WithDescription("Creates a new daily timesheet entry with hours and project allocation")
            .Produces<CreateTimesheetLineResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Timesheets))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetTimesheetLineRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetTimesheetLine")
            .WithSummary("Gets timesheet line details")
            .WithDescription("Retrieves detailed information about a specific timesheet line entry")
            .Produces<TimesheetLineResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Timesheets))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchTimesheetLinesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchTimesheetLines")
            .WithSummary("Searches timesheet lines")
            .WithDescription("Searches and filters timesheet lines with pagination")
            .Produces<PagedList<TimesheetLineResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Timesheets))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateTimesheetLineCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateTimesheetLine")
            .WithSummary("Updates timesheet line")
            .WithDescription("Updates hours, project allocation, or billing information for a timesheet line")
            .Produces<UpdateTimesheetLineResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Timesheets))
            .MapToApiVersion(1);

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteTimesheetLineCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteTimesheetLine")
            .WithSummary("Deletes a timesheet line")
            .WithDescription("Deletes a specific timesheet line entry from the system")
            .Produces<DeleteTimesheetLineResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Timesheets))
            .MapToApiVersion(1);
    }
}

