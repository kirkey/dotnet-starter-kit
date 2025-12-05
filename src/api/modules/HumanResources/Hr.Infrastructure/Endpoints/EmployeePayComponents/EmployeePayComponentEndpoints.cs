using FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeePayComponents;

/// <summary>
/// Endpoint routes for managing employee pay components.
/// </summary>
public class EmployeePayComponentEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all employee pay component endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/employee-pay-components").WithTags("employee-pay-components");

        group.MapPost("/", async (CreateEmployeePayComponentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CreateEmployeePayComponentEndpoint")
            .WithSummary("Create employee pay component")
            .WithDescription("Creates employee-specific pay component assignment")
            .Produces<CreateEmployeePayComponentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetEmployeePayComponentRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetEmployeePayComponentEndpoint")
            .WithSummary("Get employee pay component by ID")
            .WithDescription("Retrieves employee pay component assignment by ID")
            .Produces<EmployeePayComponentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchEmployeePayComponentsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchEmployeePayComponentsEndpoint")
            .WithSummary("Searches employee pay components")
            .WithDescription("Searches and filters employee pay component assignments by employee, component, type, and active status with pagination support.")
            .Produces<PagedList<EmployeePayComponentResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateEmployeePayComponentCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateEmployeePayComponentEndpoint")
            .WithSummary("Update employee pay component")
            .WithDescription("Updates employee pay component assignment")
            .Produces<UpdateEmployeePayComponentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteEmployeePayComponentCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteEmployeePayComponentEndpoint")
            .WithSummary("Delete employee pay component")
            .WithDescription("Deletes employee pay component assignment by ID")
            .Produces<DeleteEmployeePayComponentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}



