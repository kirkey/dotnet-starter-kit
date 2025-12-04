using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollectionStrategyEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collection-strategies").WithTags("Collection Strategies");

        group.MapPost("/", async (CreateCollectionStrategyCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/collection-strategies/{result.Id}", result);
        })
        .WithName("CreateCollectionStrategy")
        .WithSummary("Create a new collection strategy")
        .Produces<CreateCollectionStrategyResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCollectionStrategyRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetCollectionStrategy")
        .WithSummary("Get collection strategy by ID")
        .Produces<CollectionStrategyResponse>();

        group.MapPost("/{id:guid}/activate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new ActivateStrategyCommand(id));
            return Results.Ok(result);
        })
        .WithName("ActivateStrategy")
        .WithSummary("Activate a collection strategy")
        .Produces<ActivateStrategyResponse>();

        group.MapPost("/{id:guid}/deactivate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeactivateStrategyCommand(id));
            return Results.Ok(result);
        })
        .WithName("DeactivateStrategy")
        .WithSummary("Deactivate a collection strategy")
        .Produces<DeactivateStrategyResponse>();

    }
}
