using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class RiskCategoryEndpoints() : CarterModule("microfinance")
{

    private const string ActivateRiskCategory = "ActivateRiskCategory";
    private const string CreateRiskCategory = "CreateRiskCategory";
    private const string DeactivateRiskCategory = "DeactivateRiskCategory";
    private const string GetRiskCategory = "GetRiskCategory";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/risk-categories").WithTags("Risk Categories");

        group.MapPost("/", async (CreateRiskCategoryCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/risk-categories/{result.Id}", result);
        })
        .WithName(CreateRiskCategory)
        .WithSummary("Create a new risk category")
        .Produces<CreateRiskCategoryResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetRiskCategoryRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetRiskCategory)
        .WithSummary("Get risk category by ID")
        .Produces<RiskCategoryResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/activate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new ActivateRiskCategoryCommand(id));
            return Results.Ok(result);
        })
        .WithName(ActivateRiskCategory)
        .WithSummary("Activate risk category")
        .Produces<ActivateRiskCategoryResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/deactivate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeactivateRiskCategoryCommand(id));
            return Results.Ok(result);
        })
        .WithName(DeactivateRiskCategory)
        .WithSummary("Deactivate risk category")
        .Produces<DeactivateRiskCategoryResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}
