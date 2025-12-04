using Carter;
using FSH.Starter.WebApi.HumanResources.Application.Benefits.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.Benefits.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.Benefits.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Benefits.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.Benefits.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Benefits;

/// <summary>
/// Endpoint routes for managing benefit master data.
/// </summary>
public class BenefitEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all benefit endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/benefits").WithTags("benefits");

        group.MapPost("/", async (CreateBenefitCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetBenefit", new { id = response.Id }, response);
            })
            .WithName("CreateBenefit")
            .WithSummary("Create Benefit")
            .WithDescription("Creates a new benefit offering (mandatory or optional) with contribution details per Philippines Labor Code.")
            .Produces<CreateBenefitResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Benefits))
            .MapToApiVersion(1);

        group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetBenefitRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetBenefit")
            .WithSummary("Get Benefit Details")
            .WithDescription("Retrieves detailed information for the specified benefit including contributions, coverage, and effective dates.")
            .Produces<BenefitResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Benefits))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchBenefitsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchBenefits")
            .WithSummary("Search Benefits")
            .WithDescription("Search benefit catalog by type, mandatory flag, and active status with pagination.")
            .Produces<PagedList<BenefitDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Benefits))
            .MapToApiVersion(1);

        group.MapPut("/{id}", async (DefaultIdType id, UpdateBenefitCommand body, ISender mediator) =>
            {
                if (body.Id != id)
                    return Results.BadRequest(new { title = "ID mismatch." });

                var response = await mediator.Send(body).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateBenefit")
            .WithSummary("Update Benefit")
            .WithDescription("Updates benefit contribution, coverage, activation status, and description.")
            .Produces<UpdateBenefitResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Benefits))
            .MapToApiVersion(1);

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeleteBenefitCommand(id);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return response.Success ? Results.Ok(response) : Results.NotFound();
            })
            .WithName("DeleteBenefit")
            .WithSummary("Delete Benefit")
            .WithDescription("Deletes a benefit from the catalog.")
            .Produces<DeleteBenefitResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}


