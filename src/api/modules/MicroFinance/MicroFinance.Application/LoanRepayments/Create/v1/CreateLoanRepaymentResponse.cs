namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Create.v1;

public sealed record CreateLoanRepaymentResponse(
    Guid Id,
    decimal PrincipalPaid,
    decimal InterestPaid,
    decimal PenaltyPaid,
    decimal RemainingBalance);
