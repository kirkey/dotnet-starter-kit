using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Search.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Loan Collaterals.
/// </summary>
public static class LoanCollateralEndpoints
{
    /// <summary>
    /// Maps all Loan Collateral endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapLoanCollateralEndpoints(this IEndpointRouteBuilder app)
    {
        var collateralsGroup = app.MapGroup("loan-collaterals").WithTags("loan-collaterals");

        collateralsGroup.MapPost("/", async (CreateLoanCollateralCommand command, ISender mediator) =>
        {
            var response = await mediator.Send(command);
            return Results.Created($"/microfinance/loan-collaterals/{response.Id}", response);
        })
        .WithName("CreateLoanCollateral")
        .WithSummary("Creates a new loan collateral")
        .Produces<CreateLoanCollateralResponse>(StatusCodes.Status201Created);

        collateralsGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetLoanCollateralRequest(id));
            return Results.Ok(response);
        })
        .WithName("GetLoanCollateral")
        .WithSummary("Gets a loan collateral by ID")
        .Produces<LoanCollateralResponse>();

        collateralsGroup.MapPost("/search", async (SearchLoanCollateralsCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName("SearchLoanCollaterals")
        .WithSummary("Searches loan collaterals with pagination")
        .Produces<PagedList<LoanCollateralResponse>>();

        collateralsGroup.MapGet("/by-loan/{loanId:guid}", async (Guid loanId, ISender mediator) =>
        {
            var request = new SearchLoanCollateralsCommand
            {
                LoanId = loanId,
                PageNumber = 1,
                PageSize = 50
            };
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName("GetLoanCollateralsByLoan")
        .WithSummary("Gets all collaterals for a loan")
        .Produces<PagedList<LoanCollateralResponse>>();

        return app;
    }
}
