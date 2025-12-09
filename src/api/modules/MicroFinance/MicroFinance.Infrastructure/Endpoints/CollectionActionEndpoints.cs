using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollectionActionEndpoints : CarterModule
{

    private const string CreateCollectionAction = "CreateCollectionAction";
    private const string GetCollectionAction = "GetCollectionAction";
    private const string SearchCollectionActions = "SearchCollectionActions";
    private const string UpdateCollectionAction = "UpdateCollectionAction";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collection-actions").WithTags("Collection Actions");

        group.MapPost("/", async (CreateCollectionActionCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/collection-actions/{result.Id}", result);
        })
        .WithName(CreateCollectionAction)
        .WithSummary("Create a new collection action")
        .Produces<CreateCollectionActionResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetCollectionActionRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCollectionAction)
        .WithSummary("Get collection action by ID")
        .Produces<CollectionActionResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateCollectionActionCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(UpdateCollectionAction)
        .WithSummary("Update a collection action")
        .Produces<UpdateCollectionActionResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchCollectionActionsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchCollectionActions)
        .WithSummary("Search collection actions")
        .Produces<PagedList<CollectionActionSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}
