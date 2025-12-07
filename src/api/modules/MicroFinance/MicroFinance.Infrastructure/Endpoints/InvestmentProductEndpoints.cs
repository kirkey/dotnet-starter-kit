using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.UpdateNav.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class InvestmentProductEndpoints() : CarterModule
{

    private const string CreateInvestmentProduct = "CreateInvestmentProduct";
    private const string GetInvestmentProduct = "GetInvestmentProduct";
    private const string UpdateInvestmentProductNav = "UpdateInvestmentProductNav";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/investment-products").WithTags("Investment Products");

        group.MapPost("/", async (CreateInvestmentProductCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/investment-products/{result.Id}", result);
        })
        .WithName(CreateInvestmentProduct)
        .WithSummary("Create a new investment product")
        .Produces<CreateInvestmentProductResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetInvestmentProductRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetInvestmentProduct)
        .WithSummary("Get investment product by ID")
        .Produces<InvestmentProductResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}/nav", async (Guid id, UpdateNavRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateInvestmentProductNavCommand(id, request.NewNav, request.NavDate));
            return Results.Ok(result);
        })
        .WithName(UpdateInvestmentProductNav)
        .WithSummary("Update investment product NAV")
        .Produces<UpdateInvestmentProductNavResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record UpdateNavRequest(decimal NewNav, DateOnly NavDate);
