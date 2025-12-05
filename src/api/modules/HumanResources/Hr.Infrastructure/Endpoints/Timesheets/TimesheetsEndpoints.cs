using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Timesheets;

/// <summary>
/// Endpoint configuration for Timesheets module.
/// </summary>
public class TimesheetsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all Timesheets endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/timesheets").WithTags("timesheets");

        group.MapPost("/", async (CreateTimesheetCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetTimesheet", new { id = response.Id }, response);
            })
            .WithName("CreateTimesheet")
            .WithSummary("Creates a new timesheet")
            .WithDescription("Creates a timesheet for an employee for a pay period")
            .Produces<CreateTimesheetResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Timesheets))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetTimesheetRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetTimesheet")
            .WithSummary("Gets timesheet by ID")
            .WithDescription("Retrieves timesheet details")
            .Produces<TimesheetResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Timesheets))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchTimesheetsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchTimesheets")
            .WithSummary("Searches timesheets")
            .WithDescription("Searches timesheets with pagination and filters")
            .Produces<PagedList<TimesheetResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Timesheets))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateTimesheetCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateTimesheet")
            .WithSummary("Updates a timesheet")
            .WithDescription("Updates timesheet status and approval information")
            .Produces<UpdateTimesheetResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Timesheets))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteTimesheetCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteTimesheet")
            .WithSummary("Deletes a timesheet")
            .WithDescription("Deletes a timesheet record")
            .Produces<DeleteTimesheetResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Timesheets))
            .MapToApiVersion(1);
    }
}

