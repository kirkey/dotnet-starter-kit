using FSH.Starter.WebApi.HumanResources.Application.Shifts.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.Shifts.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.Shifts.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Shifts.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.Shifts.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Shifts;

/// <summary>
/// Endpoint configuration for Shifts module.
/// </summary>
public class ShiftsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all Shifts endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/shifts").WithTags("shifts");

        group.MapPost("/", async (CreateShiftCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetShift", new { id = response.Id }, response);
            })
            .WithName("CreateShiftEndpoint")
            .WithSummary("Creates a new shift")
            .WithDescription("Creates a new shift template (morning, evening, night, etc.)")
            .Produces<CreateShiftResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetShiftRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetShiftEndpoint")
            .WithSummary("Gets shift by ID")
            .WithDescription("Retrieves shift details with breaks and working hours")
            .Produces<ShiftResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateShiftCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateShiftEndpoint")
            .WithSummary("Updates a shift")
            .WithDescription("Updates shift information")
            .Produces<UpdateShiftResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteShiftCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteShiftEndpoint")
            .WithSummary("Deletes a shift")
            .WithDescription("Deletes a shift template")
            .Produces<DeleteShiftResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchShiftsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchShiftsEndpoint")
            .WithSummary("Searches shifts")
            .WithDescription("Searches shifts with pagination and filters")
            .Produces<PagedList<ShiftResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

