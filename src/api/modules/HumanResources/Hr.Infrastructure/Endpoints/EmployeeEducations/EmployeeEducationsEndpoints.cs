using Carter;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeEducations;

/// <summary>
/// Endpoint configuration for EmployeeEducations module.
/// </summary>
public class EmployeeEducationsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all EmployeeEducations endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/employee-educations").WithTags("employee-educations");

        group.MapPost("/", async (CreateEmployeeEducationCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetEmployeeEducation", new { id = response.Id }, response);
            })
            .WithName("CreateEmployeeEducation")
            .WithSummary("Creates a new employee education record")
            .WithDescription("Creates a new employee education record with qualification details")
            .Produces<CreateEmployeeEducationResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Employees));

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetEmployeeEducationRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetEmployeeEducation")
            .WithSummary("Gets employee education details")
            .WithDescription("Retrieves detailed information about a specific employee education record")
            .Produces<EmployeeEducationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees));

        group.MapPost("/search", async (SearchEmployeeEducationsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchEmployeeEducations")
            .WithSummary("Searches employee education records")
            .WithDescription("Searches and filters employee education records with pagination")
            .Produces<PagedList<EmployeeEducationResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees));

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateEmployeeEducationCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateEmployeeEducation")
            .WithSummary("Updates employee education record")
            .WithDescription("Updates education details for an existing employee education record")
            .Produces<UpdateEmployeeEducationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Employees));

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteEmployeeEducationCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteEmployeeEducation")
            .WithSummary("Deletes an employee education record")
            .WithDescription("Deletes a specific employee education record from the system")
            .Produces<DeleteEmployeeEducationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Employees));
    }
}

