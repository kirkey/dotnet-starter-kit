using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeContacts;

/// <summary>
/// Endpoint configuration for EmployeeContacts module.
/// </summary>
public class EmployeeContactsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all EmployeeContacts endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/employee-contacts").WithTags("employee-contacts");

        group.MapPost("/", async (CreateEmployeeContactCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetEmployeeContact", new { id = response.Id }, response);
            })
            .WithName("CreateEmployeeContactEndpoint")
            .WithSummary("Creates a new employee contact")
            .WithDescription("Creates a new employee contact (emergency, reference, family)")
            .Produces<CreateEmployeeContactResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetEmployeeContactRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetEmployeeContactEndpoint")
            .WithSummary("Gets employee contact by ID")
            .WithDescription("Retrieves employee contact details")
            .Produces<EmployeeContactResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchEmployeeContactsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchEmployeeContactsEndpoint")
            .WithSummary("Searches employee contacts")
            .WithDescription("Searches employee contacts with pagination and filters")
            .Produces<PagedList<EmployeeContactResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateEmployeeContactCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateEmployeeContactEndpoint")
            .WithSummary("Updates an employee contact")
            .WithDescription("Updates employee contact information")
            .Produces<UpdateEmployeeContactResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteEmployeeContactCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteEmployeeContactEndpoint")
            .WithSummary("Deletes an employee contact")
            .WithDescription("Deletes an employee contact record")
            .Produces<DeleteEmployeeContactResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

