using Carter;
using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.OrganizationalUnits;

/// <summary>
/// Endpoint configuration for OrganizationalUnits module.
/// </summary>
public class OrganizationalUnitsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all OrganizationalUnits endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/organizational-units").WithTags("organizational-units");

        group.MapPost("/", async (CreateOrganizationalUnitCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/hr/organizational-units/{response.Id}", response);
            })
            .WithName("CreateOrganizationalUnit")
            .WithSummary("Creates a new organizational unit")
            .WithDescription("Creates a new organizational unit (Department, Division, or Section)")
            .Produces<CreateOrganizationalUnitResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Organization))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetOrganizationalUnitRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetOrganizationalUnit")
            .WithSummary("Gets organizational unit by ID")
            .WithDescription("Retrieves organizational unit details by ID")
            .Produces<OrganizationalUnitResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Organization))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateOrganizationalUnitCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateOrganizationalUnit")
            .WithSummary("Updates an organizational unit")
            .WithDescription("Updates organizational unit information")
            .Produces<UpdateOrganizationalUnitResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Organization))
            .MapToApiVersion(1);

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteOrganizationalUnitCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteOrganizationalUnit")
            .WithSummary("Deletes an organizational unit")
            .WithDescription("Deletes an organizational unit if it has no children")
            .Produces<DeleteOrganizationalUnitResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Organization))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchOrganizationalUnitsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchOrganizationalUnits")
            .WithSummary("Searches organizational units")
            .WithDescription("Searches organizational units with pagination and filters")
            .Produces<PagedList<OrganizationalUnitResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Organization))
            .MapToApiVersion(1);
    }
}

