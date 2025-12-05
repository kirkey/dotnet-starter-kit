using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Accrue.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveBalances;

public class LeaveBalancesEndpoints() : CarterModule("humanresources")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/leave-balances").WithTags("leave-balances");

        group.MapPost("/", async (CreateLeaveBalanceCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/hr/leave-balances/{response.Id}", response);
            })
            .WithName("CreateLeaveBalance")
            .WithSummary("Creates a new leave balance")
            .WithDescription("Creates a new leave balance for an employee for a specific leave type and year with opening balance")
            .Produces<CreateLeaveBalanceResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetLeaveBalanceRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetLeaveBalance")
            .WithSummary("Gets leave balance by ID")
            .WithDescription("Retrieves detailed information about a specific leave balance including opening, used, and remaining days")
            .Produces<LeaveBalanceResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateLeaveBalanceCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("Route ID and request ID do not match.");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateLeaveBalance")
            .WithSummary("Updates a leave balance")
            .WithDescription("Updates leave balance opening balance or other details")
            .Produces<UpdateLeaveBalanceResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteLeaveBalanceCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteLeaveBalance")
            .WithSummary("Deletes a leave balance")
            .WithDescription("Removes a leave balance from the system")
            .Produces<DeleteLeaveBalanceResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchLeaveBalancesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchLeaveBalances")
            .WithSummary("Searches leave balances")
            .WithDescription("Searches and filters leave balances by employee, leave type, year with pagination support")
            .Produces<PagedList<LeaveBalanceResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapPost("/{id}/accrue", async (AccrueLeaveCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("AccrueLeave")
            .WithSummary("Accrues leave to a balance")
            .WithDescription("Adds accrued leave amount to a leave balance based on accrual frequency")
            .Produces<AccrueLeaveResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Accrue, FshResources.Leaves))
            .MapToApiVersion(1);
    }
}

