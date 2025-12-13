using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Dashboard;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Delete.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Loan Products.
/// </summary>
public class LoanProductEndpoints : CarterModule
{

    private const string CreateLoanProduct = "CreateLoanProduct";
    private const string DeleteLoanProduct = "DeleteLoanProduct";
    private const string GetLoanProduct = "GetLoanProduct";
    private const string GetLoanProductDashboard = "GetLoanProductDashboard";
    private const string SearchLoanProducts = "SearchLoanProducts";
    private const string UpdateLoanProduct = "UpdateLoanProduct";

    /// <summary>
    /// Maps all Loan Product endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var loanProductsGroup = app.MapGroup("microfinance/loan-products").WithTags("Loan Products");

        loanProductsGroup.MapPost("/", async (CreateLoanProductCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/loan-products/{response.Id}", response);
            })
            .WithName(CreateLoanProduct)
            .WithSummary("Creates a new loan product")
            .Produces<CreateLoanProductResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loanProductsGroup.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetLoanProductRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetLoanProduct)
            .WithSummary("Gets a loan product by ID")
            .Produces<LoanProductResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loanProductsGroup.MapPut("/{id:guid}", async (DefaultIdType id, UpdateLoanProductCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(UpdateLoanProduct)
            .WithSummary("Updates a loan product")
            .Produces<UpdateLoanProductResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loanProductsGroup.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                await sender.Send(new DeleteLoanProductCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(DeleteLoanProduct)
            .WithSummary("Deletes a loan product")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loanProductsGroup.MapPost("/search", async ([FromBody] SearchLoanProductsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchLoanProducts)
            .WithSummary("Searches loan products with filters and pagination")
            .Produces<PagedList<LoanProductResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loanProductsGroup.MapGet("/{id}/dashboard", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetLoanProductDashboardQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetLoanProductDashboard)
            .WithSummary("Gets comprehensive dashboard analytics for a loan product")
            .Produces<LoanProductDashboardResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);
    }
}
