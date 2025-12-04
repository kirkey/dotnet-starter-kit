using Carter;
using FSH.Starter.WebApi.HumanResources.Application.Designations.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.Designations.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.Designations.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Designations.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.Designations.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Designations;

/// <summary>
/// Endpoint configuration for Designations module.
/// </summary>
public class DesignationsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Designations endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/designations").WithTags("designations");

        group.MapPost("/", async (CreateDesignationCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CreateDesignation")
            .WithSummary("Creates a new designation")
            .WithDescription("Creates a new designation in an organizational unit")
            .Produces<CreateDesignationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Organization))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetDesignationRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetDesignation")
            .WithSummary("Gets designation by ID")
            .WithDescription("Retrieves designation details by ID")
            .Produces<DesignationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Organization))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateDesignationCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateDesignation")
            .WithSummary("Updates a designation")
            .WithDescription("Updates designation information")
            .Produces<UpdateDesignationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Organization))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteDesignationCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteDesignation")
            .WithSummary("Deletes a designation")
            .WithDescription("Deletes a designation")
            .Produces<DeleteDesignationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Organization))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchDesignationsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchDesignations")
            .WithSummary("Searches designations")
            .WithDescription("Searches designations with pagination and filters")
            .Produces<PagedList<DesignationResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Organization))
            .MapToApiVersion(1);
    }
}

