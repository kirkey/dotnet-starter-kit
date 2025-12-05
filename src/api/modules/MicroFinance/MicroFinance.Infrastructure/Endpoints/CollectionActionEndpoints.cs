using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollectionActionEndpoints() : CarterModule("microfinance")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collection-actions").WithTags("Collection Actions");

        group.MapPost("/", async (CreateCollectionActionCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/collection-actions/{result.Id}", result);
        })
        .WithName("CreateCollectionAction")
        .WithSummary("Create a new collection action")
        .Produces<CreateCollectionActionResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCollectionActionRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetCollectionAction")
        .WithSummary("Get collection action by ID")
        .Produces<CollectionActionResponse>();

    }
}
