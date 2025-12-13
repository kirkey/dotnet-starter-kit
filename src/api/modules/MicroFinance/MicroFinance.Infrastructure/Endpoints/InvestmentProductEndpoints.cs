using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Update.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.UpdateNav.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Investment Products.
/// </summary>
public class InvestmentProductEndpoints : CarterModule
{
    private const string ActivateInvestmentProduct = "ActivateInvestmentProduct";
    private const string CreateInvestmentProduct = "CreateInvestmentProduct";
    private const string DeactivateInvestmentProduct = "DeactivateInvestmentProduct";
    private const string GetInvestmentProduct = "GetInvestmentProduct";
    private const string SearchInvestmentProducts = "SearchInvestmentProducts";
    private const string UpdateInvestmentProduct = "UpdateInvestmentProduct";
    private const string UpdateInvestmentProductNav = "UpdateInvestmentProductNav";

    /// <summary>
    /// Maps all Investment Product endpoints to the route builder.
    /// </summary>
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

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetInvestmentProductRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetInvestmentProduct)
        .WithSummary("Get investment product by ID")
        .Produces<InvestmentProductResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async ([FromBody] SearchInvestmentProductsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(SearchInvestmentProducts)
        .WithSummary("Search investment products with filters and pagination")
        .Produces<PagedList<InvestmentProductResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPut("/{id}/nav", async (DefaultIdType id, UpdateNavRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateInvestmentProductNavCommand(id, request.NewNav, request.NavDate));
            return Results.Ok(result);
        })
        .WithName(UpdateInvestmentProductNav)
        .WithSummary("Update investment product NAV")
        .Produces<UpdateInvestmentProductNavResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateInvestmentProductRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateInvestmentProductCommand(
                id,
                request.Name,
                request.Description,
                request.MinimumInvestment,
                request.MaximumInvestment,
                request.ManagementFeePercent,
                request.PerformanceFeePercent,
                request.EntryLoadPercent,
                request.ExitLoadPercent,
                request.MinimumHoldingDays,
                request.FundManager,
                request.Benchmark,
                request.AllowPartialRedemption,
                request.AllowSip,
                request.DisplayOrder)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(UpdateInvestmentProduct)
        .WithSummary("Update an investment product")
        .Produces<UpdateInvestmentProductResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/activate", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new ActivateInvestmentProductCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(ActivateInvestmentProduct)
        .WithSummary("Activate an investment product")
        .Produces<ActivateInvestmentProductResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/deactivate", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new DeactivateInvestmentProductCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(DeactivateInvestmentProduct)
        .WithSummary("Deactivate an investment product")
        .Produces<DeactivateInvestmentProductResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);
    }
}

public record UpdateNavRequest(decimal NewNav, DateOnly NavDate);

public sealed record UpdateInvestmentProductRequest(
    string? Name,
    string? Description,
    decimal? MinimumInvestment,
    decimal? MaximumInvestment,
    decimal? ManagementFeePercent,
    decimal? PerformanceFeePercent,
    decimal? EntryLoadPercent,
    decimal? ExitLoadPercent,
    int? MinimumHoldingDays,
    string? FundManager,
    string? Benchmark,
    bool? AllowPartialRedemption,
    bool? AllowSip,
    int? DisplayOrder);
