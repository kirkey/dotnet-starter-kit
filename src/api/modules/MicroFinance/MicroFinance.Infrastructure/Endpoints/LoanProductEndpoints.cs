using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Delete.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Loan Products.
/// </summary>
public class LoanProductEndpoints() : CarterModule("microfinance")
{
    /// <summary>
    /// Maps all Loan Product endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var loanProductsGroup = app.MapGroup("microfinance/loan-products").WithTags("loan-products");

        loanProductsGroup.MapPost("/", async (CreateLoanProductCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/loan-products/{response.Id}", response);
            })
            .WithName("CreateLoanProduct")
            .WithSummary("Creates a new loan product")
            .Produces<CreateLoanProductResponse>(StatusCodes.Status201Created);

        loanProductsGroup.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetLoanProductRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetLoanProduct")
            .WithSummary("Gets a loan product by ID")
            .Produces<LoanProductResponse>();

        loanProductsGroup.MapPut("/{id:guid}", async (Guid id, UpdateLoanProductCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateLoanProduct")
            .WithSummary("Updates a loan product")
            .Produces<UpdateLoanProductResponse>();

        loanProductsGroup.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
            {
                await sender.Send(new DeleteLoanProductCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteLoanProduct")
            .WithSummary("Deletes a loan product")
            .Produces(StatusCodes.Status204NoContent);

        loanProductsGroup.MapPost("/search", async ([FromBody] SearchLoanProductsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchLoanProducts")
            .WithSummary("Searches loan products with filters and pagination")
            .Produces<PagedList<LoanProductResponse>>();
    }
}
