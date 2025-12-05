using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.ShiftAssignments;

/// <summary>
/// Endpoint configuration for Shift Assignments module.
/// </summary>
public class ShiftAssignmentsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all Shift Assignments endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/shift-assignments").WithTags("shift-assignments");

        group.MapPost("/", async (CreateShiftAssignmentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetShiftAssignment", new { id = response.Id }, response);
            })
            .WithName("CreateShiftAssignmentEndpoint")
            .WithSummary("Creates a new shift assignment")
            .WithDescription("Assigns a shift to an employee for a specified date range")
            .Produces<CreateShiftAssignmentResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Attendance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetShiftAssignmentRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetShiftAssignmentEndpoint")
            .WithSummary("Gets shift assignment details")
            .WithDescription("Retrieves detailed information about a specific shift assignment")
            .Produces<ShiftAssignmentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Attendance))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateShiftAssignmentCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("Route ID and request ID do not match.");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateShiftAssignmentEndpoint")
            .WithSummary("Updates a shift assignment")
            .WithDescription("Updates the dates, recurrence, or notes for a shift assignment")
            .Produces<UpdateShiftAssignmentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Attendance))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteShiftAssignmentCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteShiftAssignmentEndpoint")
            .WithSummary("Deletes a shift assignment")
            .WithDescription("Removes a shift assignment from the system")
            .Produces<DeleteShiftAssignmentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Attendance))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchShiftAssignmentsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchShiftAssignmentsEndpoint")
            .WithSummary("Searches shift assignments")
            .WithDescription("Searches and filters shift assignments with pagination")
            .Produces<PagedList<ShiftAssignmentResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Attendance))
            .MapToApiVersion(1);
    }
}
