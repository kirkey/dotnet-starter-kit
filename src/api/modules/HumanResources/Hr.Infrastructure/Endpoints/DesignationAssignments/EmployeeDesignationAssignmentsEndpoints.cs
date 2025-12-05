using FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.End.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Search.v1;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DesignationAssignments.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DesignationAssignments;

/// <summary>
/// Endpoint configuration for DesignationAssignments module.
/// </summary>
public class EmployeeDesignationAssignmentsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all DesignationAssignments endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/employee-designation-assignments").WithTags("employee-designation-assignments");

        group.MapPost("/plantilla", async (AssignPlantillaDesignationCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("AssignPlantillaDesignationEndpoint")
            .WithSummary("Assigns a plantilla designation to an employee")
            .WithDescription("Assigns a primary/plantilla designation to an employee")
            .Produces<AssignDesignationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Assign, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/acting-as", async (AssignActingAsDesignationCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("AssignActingAsDesignationEndpoint")
            .WithSummary("Assigns an acting as designation to an employee")
            .WithDescription("Assigns a temporary acting designation to an employee")
            .Produces<AssignDesignationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Assign, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator
                    .Send(new GetDesignationAssignmentRequest(id))
                    .ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetDesignationAssignmentEndpoint")
            .WithSummary("Gets designation assignment by ID")
            .WithDescription("Retrieves designation assignment details including tenure and status")
            .Produces<DesignationAssignmentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/end", async (DefaultIdType id, EndDesignationRequest request, ISender mediator) =>
            {
                var response = await mediator
                    .Send(new EndDesignationAssignmentCommand(id, request.EndDate, request.Reason))
                    .ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("EndDesignationAssignmentEndpoint")
            .WithSummary("Ends a designation assignment")
            .WithDescription("Ends an active designation assignment on a specified date")
            .Produces<EndDesignationAssignmentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/history/search", async (SearchEmployeeHistoryRequest request, ISender mediator) =>
            {
                var result = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("SearchEmployeeDesignationHistoryEndpoint")
            .WithSummary("Search employee designation history")
            .WithDescription("Searches employee designation history with support for temporal queries, filtering by organization, designation, date range, and employment status")
            .Produces<PagedList<EmployeeHistoryDto>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

