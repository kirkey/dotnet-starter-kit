using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Dashboard;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Savings Products.
/// </summary>
public class SavingsProductEndpoints : CarterModule
{

    private const string CreateSavingsProduct = "CreateSavingsProduct";
    private const string GetSavingsProduct = "GetSavingsProduct";
    private const string GetSavingsProductDashboard = "GetSavingsProductDashboard";
    private const string SearchSavingsProducts = "SearchSavingsProducts";
    private const string UpdateSavingsProduct = "UpdateSavingsProduct";

    /// <summary>
    /// Maps all Savings Product endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var savingsProductsGroup = app.MapGroup("microfinance/savings-products").WithTags("Savings Products");

        savingsProductsGroup.MapPost("/", async (CreateSavingsProductCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/savings-products/{response.Id}", response);
            })
            .WithName(CreateSavingsProduct)
            .WithSummary("Creates a new savings product")
            .Produces<CreateSavingsProductResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsProductsGroup.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetSavingsProductRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetSavingsProduct)
            .WithSummary("Gets a savings product by ID")
            .Produces<SavingsProductResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsProductsGroup.MapPost("/search", async (SearchSavingsProductsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchSavingsProducts)
            .WithSummary("Searches savings products with filters and pagination")
            .Produces<PagedList<SavingsProductResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsProductsGroup.MapPut("/{id:guid}", async (DefaultIdType id, UpdateSavingsProductCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(UpdateSavingsProduct)
            .WithSummary("Updates a savings product")
            .Produces<UpdateSavingsProductResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsProductsGroup.MapGet("/{id}/dashboard", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetSavingsProductDashboardQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetSavingsProductDashboard)
            .WithSummary("Gets comprehensive dashboard analytics for a savings product")
            .Produces<SavingsProductDashboardResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);
    }
}
