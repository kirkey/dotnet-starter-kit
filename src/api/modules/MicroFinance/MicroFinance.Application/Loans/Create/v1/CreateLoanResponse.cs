namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Create.v1;

public sealed record CreateLoanResponse(DefaultIdType Id, string LoanNumber, string Status);
