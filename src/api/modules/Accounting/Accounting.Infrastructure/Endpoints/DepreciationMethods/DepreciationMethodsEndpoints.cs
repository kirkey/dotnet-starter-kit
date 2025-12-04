using Accounting.Application.DepreciationMethods.Activate.v1;
using Accounting.Application.DepreciationMethods.Create;
using Accounting.Application.DepreciationMethods.Deactivate.v1;
using Accounting.Application.DepreciationMethods.Delete;
using Accounting.Application.DepreciationMethods.Get;
using Accounting.Application.DepreciationMethods.Responses;
using Accounting.Application.DepreciationMethods.Search.v1;
using Accounting.Application.DepreciationMethods.Update;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.DepreciationMethods;

/// <summary>
/// Endpoint configuration for Depreciation Methods module.
/// </summary>
public class DepreciationMethodsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Depreciation Methods endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/depreciation-methods").WithTags("depreciation-methods");

        // Create depreciation method
        group.MapPost("/", async (CreateDepreciationMethodRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CreateDepreciationMethod")
            .WithSummary("Create a depreciation method")
            .WithDescription("Creates a new depreciation method")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);

        // Get depreciation method by ID
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetDepreciationMethodRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetDepreciationMethod")
            .WithSummary("Get a depreciation method by ID")
            .WithDescription("Gets the details of a depreciation method by its ID")
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Update depreciation method
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateDepreciationMethodRequest request, ISender mediator) =>
            {
                var updateRequest = request with { Id = id };
                var response = await mediator.Send(updateRequest).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateDepreciationMethod")
            .WithSummary("Update a depreciation method")
            .WithDescription("Updates an existing depreciation method")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Delete depreciation method
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteDepreciationMethodRequest(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteDepreciationMethod")
            .WithSummary("Delete a depreciation method")
            .WithDescription("Deletes a depreciation method by its ID")
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);

        // Search depreciation methods
        group.MapPost("/search", async (SearchDepreciationMethodsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchDepreciationMethods")
            .WithSummary("Search depreciation methods")
            .WithDescription("Searches depreciation methods with filtering and pagination")
            .Produces<PagedList<DepreciationMethodResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Activate depreciation method
        group.MapPost("/{id:guid}/activate", async (DefaultIdType id, ISender mediator) =>
            {
                var methodId = await mediator.Send(new ActivateDepreciationMethodCommand(id)).ConfigureAwait(false);
                return Results.Ok(new { Id = methodId, Message = "Depreciation method activated successfully" });
            })
            .WithName("ActivateDepreciationMethod")
            .WithSummary("Activate depreciation method")
            .WithDescription("Activates a depreciation method for use")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Deactivate depreciation method
        group.MapPost("/{id:guid}/deactivate", async (DefaultIdType id, ISender mediator) =>
            {
                var methodId = await mediator.Send(new DeactivateDepreciationMethodCommand(id)).ConfigureAwait(false);
                return Results.Ok(new { Id = methodId, Message = "Depreciation method deactivated successfully" });
            })
            .WithName("DeactivateDepreciationMethod")
            .WithSummary("Deactivate depreciation method")
            .WithDescription("Deactivates a depreciation method")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
