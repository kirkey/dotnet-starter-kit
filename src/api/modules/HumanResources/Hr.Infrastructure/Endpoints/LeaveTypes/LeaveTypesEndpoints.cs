using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveTypes;

public class LeaveTypesEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/leave-types").WithTags("leave-types");

        group.MapPost("/", async (CreateLeaveTypeCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/hr/leave-types/{response.Id}", response);
            })
            .WithName("CreateLeaveType")
            .WithSummary("Creates a new leave type")
            .WithDescription("Creates a new leave type with Philippines Labor Code compliance including classification, accrual frequency, and approval requirements")
            .Produces<CreateLeaveTypeResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Leaves));

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetLeaveTypeRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetLeaveType")
            .WithSummary("Gets leave type by ID")
            .WithDescription("Retrieves detailed information about a specific leave type including accrual rules and approval requirements")
            .Produces<LeaveTypeResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves));

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateLeaveTypeCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("Route ID and request ID do not match.");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateLeaveType")
            .WithSummary("Updates a leave type")
            .WithDescription("Updates leave type information including accrual allowance, frequency, and approval requirements")
            .Produces<UpdateLeaveTypeResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Leaves));

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteLeaveTypeCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteLeaveType")
            .WithSummary("Deletes a leave type")
            .WithDescription("Removes a leave type from the system")
            .Produces<DeleteLeaveTypeResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Leaves));

        group.MapPost("/search", async (SearchLeaveTypesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchLeaveTypes")
            .WithSummary("Searches leave types")
            .WithDescription("Searches and filters leave types with pagination support")
            .Produces<PagedList<LeaveTypeResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves));
    }
}

