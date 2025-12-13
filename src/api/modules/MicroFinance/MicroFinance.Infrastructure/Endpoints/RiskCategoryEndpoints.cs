using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class RiskCategoryEndpoints : CarterModule
{

    private const string ActivateRiskCategory = "ActivateRiskCategory";
    private const string CreateRiskCategory = "CreateRiskCategory";
    private const string DeactivateRiskCategory = "DeactivateRiskCategory";
    private const string GetRiskCategory = "GetRiskCategory";
    private const string SearchRiskCategories = "SearchRiskCategories";
    private const string UpdateRiskCategory = "UpdateRiskCategory";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/risk-categories").WithTags("Risk Categories");

        group.MapPost("/", async (CreateRiskCategoryCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/microfinance/risk-categories/{result.Id}", result);
        })
        .WithName(CreateRiskCategory)
        .WithSummary("Create a new risk category")
        .Produces<CreateRiskCategoryResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetRiskCategoryRequest(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(GetRiskCategory)
        .WithSummary("Get risk category by ID")
        .Produces<RiskCategoryResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateRiskCategoryCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(UpdateRiskCategory)
        .WithSummary("Update a risk category")
        .Produces<UpdateRiskCategoryResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/activate", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new ActivateRiskCategoryCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(ActivateRiskCategory)
        .WithSummary("Activate risk category")
        .Produces<ActivateRiskCategoryResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/deactivate", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new DeactivateRiskCategoryCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(DeactivateRiskCategory)
        .WithSummary("Deactivate risk category")
        .Produces<DeactivateRiskCategoryResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchRiskCategoriesCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchRiskCategories)
        .WithSummary("Search risk categories")
        .Produces<PagedList<RiskCategorySummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}
