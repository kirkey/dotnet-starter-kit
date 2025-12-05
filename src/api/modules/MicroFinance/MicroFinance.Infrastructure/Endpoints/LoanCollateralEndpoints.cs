using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Pledge.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Release.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Seize.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.UpdateValuation.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Verify.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Loan Collaterals.
/// </summary>
public class LoanCollateralEndpoints() : CarterModule("microfinance")
{

    private const string CreateLoanCollateral = "CreateLoanCollateral";
    private const string GetLoanCollateral = "GetLoanCollateral";
    private const string GetLoanCollateralsByLoan = "GetLoanCollateralsByLoan";
    private const string PledgeLoanCollateral = "PledgeLoanCollateral";
    private const string ReleaseLoanCollateral = "ReleaseLoanCollateral";
    private const string SearchLoanCollaterals = "SearchLoanCollaterals";
    private const string SeizeLoanCollateral = "SeizeLoanCollateral";
    private const string UpdateCollateralValuation = "UpdateCollateralValuation";
    private const string VerifyLoanCollateral = "VerifyLoanCollateral";

    /// <summary>
    /// Maps all Loan Collateral endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var collateralsGroup = app.MapGroup("microfinance/loan-collaterals").WithTags("loan-collaterals");

        collateralsGroup.MapPost("/", async (CreateLoanCollateralCommand command, ISender mediator) =>
        {
            var response = await mediator.Send(command);
            return Results.Created($"/microfinance/loan-collaterals/{response.Id}", response);
        })
        .WithName(CreateLoanCollateral)
        .WithSummary("Creates a new loan collateral")
        .Produces<CreateLoanCollateralResponse>(StatusCodes.Status201Created);

        collateralsGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetLoanCollateralRequest(id));
            return Results.Ok(response);
        })
        .WithName(GetLoanCollateral)
        .WithSummary("Gets a loan collateral by ID")
        .Produces<LoanCollateralResponse>();

        collateralsGroup.MapPost("/search", async (SearchLoanCollateralsCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(SearchLoanCollaterals)
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
        .WithName(GetLoanCollateralsByLoan)
        .WithSummary("Gets all collaterals for a loan")
        .Produces<PagedList<LoanCollateralResponse>>();

        collateralsGroup.MapPut("/{id:guid}/valuation", async (Guid id, UpdateCollateralValuationCommand command, ISender mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(UpdateCollateralValuation)
        .WithSummary("Updates the valuation of a loan collateral")
        .Produces<UpdateCollateralValuationResponse>();

        collateralsGroup.MapPost("/{id:guid}/verify", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new VerifyCollateralCommand(id));
            return Results.Ok(response);
        })
        .WithName(VerifyLoanCollateral)
        .WithSummary("Verifies a loan collateral")
        .Produces<VerifyCollateralResponse>();

        collateralsGroup.MapPost("/{id:guid}/pledge", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new PledgeCollateralCommand(id));
            return Results.Ok(response);
        })
        .WithName(PledgeLoanCollateral)
        .WithSummary("Pledges a loan collateral")
        .Produces<PledgeCollateralResponse>();

        collateralsGroup.MapPost("/{id:guid}/release", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new ReleaseCollateralCommand(id));
            return Results.Ok(response);
        })
        .WithName(ReleaseLoanCollateral)
        .WithSummary("Releases a loan collateral")
        .Produces<ReleaseCollateralResponse>();

        collateralsGroup.MapPost("/{id:guid}/seize", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new SeizeCollateralCommand(id));
            return Results.Ok(response);
        })
        .WithName(SeizeLoanCollateral)
        .WithSummary("Seizes a loan collateral")
        .Produces<SeizeCollateralResponse>();
    }
}
