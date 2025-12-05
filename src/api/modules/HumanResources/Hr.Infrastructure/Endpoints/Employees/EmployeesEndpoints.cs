using FSH.Starter.WebApi.HumanResources.Application.Employees.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.Employees.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.Employees.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Employees.Regularize.v1;
using FSH.Starter.WebApi.HumanResources.Application.Employees.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.Employees.Terminate.v1;
using FSH.Starter.WebApi.HumanResources.Application.Employees.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Employees;

/// <summary>
/// Endpoint configuration for Employees module.
/// </summary>
public class EmployeesEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all Employees endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/employees").WithTags("employees");

        group.MapPost("/", async (CreateEmployeeCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/hr/employees/{response.Id}", response);
            })
            .WithName("CreateEmployee")
            .WithSummary("Creates a new employee")
            .WithDescription("Creates a new employee record in the system")
            .Produces<CreateEmployeeResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetEmployeeRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetEmployee")
            .WithSummary("Gets an employee by ID")
            .WithDescription("Retrieves detailed information about a specific employee")
            .Produces<EmployeeResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateEmployeeCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateEmployee")
            .WithSummary("Updates an employee")
            .WithDescription("Updates employee information")
            .Produces<UpdateEmployeeResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteEmployeeCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteEmployee")
            .WithSummary("Deletes an employee")
            .WithDescription("Deletes an employee record from the system")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchEmployeesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchEmployees")
            .WithSummary("Searches employees")
            .WithDescription("Searches for employees with pagination and filtering")
            .Produces<PagedList<EmployeeResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/{id}/terminate", async (DefaultIdType id, TerminateEmployeeCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("TerminateEmployee")
            .WithSummary("Terminates an employee")
            .WithDescription("Terminates an employee per Philippines Labor Code. Computes separation pay.")
            .Produces<TerminateEmployeeResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Terminate, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/{id}/regularize", async (DefaultIdType id, RegularizeEmployeeCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("RegularizeEmployee")
            .WithSummary("Regularizes a probationary employee")
            .WithDescription("Regularizes a probationary employee per Philippines Labor Code Article 280. Typically after probation period (6-12 months).")
            .Produces<RegularizeEmployeeResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Regularize, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

