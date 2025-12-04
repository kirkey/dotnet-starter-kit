using FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Create.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Loan Repayments.
/// </summary>
public static class LoanRepaymentEndpoints
{
    /// <summary>
    /// Maps all Loan Repayment endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapLoanRepaymentEndpoints(this IEndpointRouteBuilder app)
    {
        var loanRepaymentsGroup = app.MapGroup("loan-repayments").WithTags("loan-repayments");

        loanRepaymentsGroup.MapPost("/", async (CreateLoanRepaymentCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/loan-repayments/{response.Id}", response);
            })
            .WithName("CreateLoanRepayment")
            .WithSummary("Records a loan repayment")
            .Produces<CreateLoanRepaymentResponse>(StatusCodes.Status201Created);

        return app;
    }
}
