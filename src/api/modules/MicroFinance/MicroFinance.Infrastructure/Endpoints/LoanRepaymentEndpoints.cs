using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Loan Repayments.
/// </summary>
public class LoanRepaymentEndpoints() : CarterModule
{

    private const string CreateLoanRepayment = "CreateLoanRepayment";
    private const string GetLoanRepayment = "GetLoanRepayment";
    private const string GetLoanRepaymentsByLoan = "GetLoanRepaymentsByLoan";
    private const string SearchLoanRepayments = "SearchLoanRepayments";

    /// <summary>
    /// Maps all Loan Repayment endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var loanRepaymentsGroup = app.MapGroup("microfinance/loan-repayments").WithTags("loan-repayments");

        loanRepaymentsGroup.MapPost("/", async (CreateLoanRepaymentCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/loan-repayments/{response.Id}", response);
            })
            .WithName(CreateLoanRepayment)
            .WithSummary("Records a loan repayment")
            .Produces<CreateLoanRepaymentResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loanRepaymentsGroup.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetLoanRepaymentRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetLoanRepayment)
            .WithSummary("Gets a loan repayment by ID")
            .Produces<LoanRepaymentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loanRepaymentsGroup.MapPost("/search", async (SearchLoanRepaymentsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchLoanRepayments)
            .WithSummary("Searches loan repayments with filters and pagination")
            .Produces<PagedList<LoanRepaymentResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loanRepaymentsGroup.MapGet("/by-loan/{loanId:guid}", async (Guid loanId, ISender sender) =>
            {
                var command = new SearchLoanRepaymentsCommand
                {
                    LoanId = loanId,
                    PageNumber = 1,
                    PageSize = 100
                };
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetLoanRepaymentsByLoan)
            .WithSummary("Gets all repayments for a specific loan")
            .Produces<PagedList<LoanRepaymentResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);
    }
}
