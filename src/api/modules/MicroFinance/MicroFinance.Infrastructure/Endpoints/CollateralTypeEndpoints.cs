using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollateralTypeEndpoints : CarterModule
{

    private const string CreateCollateralType = "CreateCollateralType";
    private const string GetCollateralType = "GetCollateralType";
    private const string SearchCollateralTypes = "SearchCollateralTypes";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collateral-types").WithTags("Collateral Types");

        group.MapPost("/", async (CreateCollateralTypeCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/collateral-types/{result.Id}", result);
        })
        .WithName(CreateCollateralType)
        .WithSummary("Create a new collateral type")
        .Produces<CreateCollateralTypeResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetCollateralTypeRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCollateralType)
        .WithSummary("Get collateral type by ID")
        .Produces<CollateralTypeResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchCollateralTypesCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchCollateralTypes)
        .WithSummary("Search collateral types")
        .Produces<PagedList<CollateralTypeSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}
