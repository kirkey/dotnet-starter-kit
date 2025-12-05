using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.Attendances.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.Attendances.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.Attendances.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Attendances.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.Attendances.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Attendance;

public class AttendanceEndpoints() : CarterModule("humanresources")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/attendance").WithTags("attendance");

        group.MapPost("/", async (CreateAttendanceCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute("GetAttendance", new { id = response.Id }, response);
        })
        .WithName("CreateAttendance")
        .WithSummary("Creates a new attendance record")
        .WithDescription("Records employee attendance for a specific date")
        .Produces<CreateAttendanceResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Attendance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetAttendanceRequest(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetAttendance")
        .WithSummary("Gets attendance record by ID")
        .WithDescription("Retrieves attendance details")
        .Produces<AttendanceResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Attendance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchAttendanceRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchAttendance")
        .WithSummary("Searches attendance records")
        .WithDescription("Searches attendance records with pagination and filters")
        .Produces<PagedList<AttendanceResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Attendance))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateAttendanceCommand request, ISender mediator) =>
        {
            if (id != request.Id)
                return Results.BadRequest("ID mismatch");

            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("UpdateAttendance")
        .WithSummary("Updates an attendance record")
        .WithDescription("Updates attendance information")
        .Produces<UpdateAttendanceResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Attendance))
        .MapToApiVersion(1);

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new DeleteAttendanceCommand(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("DeleteAttendance")
        .WithSummary("Deletes an attendance record")
        .WithDescription("Deletes an attendance record")
        .Produces<DeleteAttendanceResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Attendance))
        .MapToApiVersion(1);
    }
}

