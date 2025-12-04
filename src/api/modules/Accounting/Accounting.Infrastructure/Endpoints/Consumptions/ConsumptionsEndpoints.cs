using Accounting.Application.Consumptions.Create.v1;
using Accounting.Application.Consumptions.Delete.v1;
using Accounting.Application.Consumptions.Get.v1;
using Accounting.Application.Consumptions.Responses;
using Accounting.Application.Consumptions.Search.v1;
using Accounting.Application.Consumptions.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Consumptions;

/// <summary>
/// Endpoint configuration for Consumptions module.
/// </summary>
public class ConsumptionsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Consumptions endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/consumptions").WithTags("consumptions");

        // Create endpoint
        group.MapPost("/", async (CreateConsumptionCommand command, ISender mediator) =>
            {
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("CreateConsumption")
            .WithSummary("Create consumption record")
            .WithDescription("Creates a new consumption/meter reading record")
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetConsumptionRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetConsumption")
            .WithSummary("Get consumption record")
            .WithDescription("Retrieves a consumption record by ID")
            .Produces<ConsumptionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateConsumptionCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID in URL does not match ID in request body");

                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateConsumption")
            .WithSummary("Update consumption record")
            .WithDescription("Updates a consumption record")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Delete endpoint
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteConsumptionCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteConsumption")
            .WithSummary("Delete consumption record")
            .WithDescription("Deletes a consumption record")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);

        // Search endpoint
        group.MapPost("/search", async (SearchConsumptionsRequest request, ISender mediator) =>
            {
                var result = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("SearchConsumptions")
            .WithSummary("Search consumption records")
            .WithDescription("Search consumption records with filters and pagination")
            .Produces<PagedList<ConsumptionResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
