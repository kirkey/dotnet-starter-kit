using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollectionStrategyEndpoints : CarterModule
{

    private const string ActivateStrategy = "ActivateStrategy";
    private const string CreateCollectionStrategy = "CreateCollectionStrategy";
    private const string DeactivateStrategy = "DeactivateStrategy";
    private const string GetCollectionStrategy = "GetCollectionStrategy";
    private const string SearchCollectionStrategies = "SearchCollectionStrategies";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collection-strategies").WithTags("Collection Strategies");

        group.MapPost("/", async (CreateCollectionStrategyCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/collection-strategies/{result.Id}", result);
        })
        .WithName(CreateCollectionStrategy)
        .WithSummary("Create a new collection strategy")
        .Produces<CreateCollectionStrategyResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetCollectionStrategyRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCollectionStrategy)
        .WithSummary("Get collection strategy by ID")
        .Produces<CollectionStrategyResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/activate", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new ActivateStrategyCommand(id));
            return Results.Ok(result);
        })
        .WithName(ActivateStrategy)
        .WithSummary("Activate a collection strategy")
        .Produces<ActivateStrategyResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/deactivate", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new DeactivateStrategyCommand(id));
            return Results.Ok(result);
        })
        .WithName(DeactivateStrategy)
        .WithSummary("Deactivate a collection strategy")
        .Produces<DeactivateStrategyResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchCollectionStrategiesCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchCollectionStrategies)
        .WithSummary("Search collection strategies")
        .Produces<PagedList<CollectionStrategySummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}
