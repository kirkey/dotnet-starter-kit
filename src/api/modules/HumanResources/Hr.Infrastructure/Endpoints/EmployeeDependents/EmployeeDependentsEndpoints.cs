using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDependents;

/// <summary>
/// Endpoint configuration for EmployeeDependents module.
/// </summary>
public class EmployeeDependentsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all EmployeeDependents endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/employee-dependents").WithTags("employee-dependents");

        group.MapPost("/", async (CreateEmployeeDependentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetEmployeeDependent", new { id = response.Id }, response);
            })
            .WithName("CreateEmployeeDependentEndpoint")
            .WithSummary("Creates a new employee dependent")
            .WithDescription("Creates a new employee dependent (family member, beneficiary)")
            .Produces<CreateEmployeeDependentResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetEmployeeDependentRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetEmployeeDependentEndpoint")
            .WithSummary("Gets employee dependent by ID")
            .WithDescription("Retrieves employee dependent details")
            .Produces<EmployeeDependentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchEmployeeDependentsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchEmployeeDependentsEndpoint")
            .WithSummary("Searches employee dependents")
            .WithDescription("Searches employee dependents with pagination and filters")
            .Produces<PagedList<EmployeeDependentResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateEmployeeDependentCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateEmployeeDependentEndpoint")
            .WithSummary("Updates an employee dependent")
            .WithDescription("Updates employee dependent information")
            .Produces<UpdateEmployeeDependentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteEmployeeDependentCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteEmployeeDependentEndpoint")
            .WithSummary("Deletes an employee dependent")
            .WithDescription("Deletes an employee dependent record")
            .Produces<DeleteEmployeeDependentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

