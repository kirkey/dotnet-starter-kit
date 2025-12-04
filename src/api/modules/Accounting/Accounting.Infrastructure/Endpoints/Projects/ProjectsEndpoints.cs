using Accounting.Application.Projects.Create.v1;
using Accounting.Application.Projects.Delete.v1;
using Accounting.Application.Projects.Get.v1;
using Accounting.Application.Projects.Responses;
using Accounting.Application.Projects.Search.v1;
using Accounting.Application.Projects.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Projects;

/// <summary>
/// Endpoint configuration for Projects module.
/// Provides comprehensive REST API endpoints for managing projects.
/// </summary>
public class ProjectsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Projects endpoints to the route builder.
    /// Includes Create, Read, Update, Delete, and Search operations.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/projects").WithTags("projects");

        // Create endpoint
        group.MapPost("/", async (CreateProjectCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CreateProject")
            .WithSummary("create a project")
            .WithDescription("create a project")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetProjectQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetProject")
            .WithSummary("get a project by id")
            .WithDescription("get a project by id")
            .Produces<ProjectResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateProjectCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateProject")
            .WithSummary("update a project")
            .WithDescription("update a project")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Delete endpoint
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteProjectCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteProject")
            .WithSummary("Delete a project by id")
            .WithDescription("Deletes a project by its unique identifier and returns the result.")
            .Produces<DeleteProjectResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);

        // Search endpoint
        group.MapPost("/search", async (ISender mediator, [FromBody] SearchProjectsCommand command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchProjects")
            .WithSummary("Gets a list of projects")
            .WithDescription("Gets a list of projects with pagination and filtering support")
            .Produces<PagedList<ProjectResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
