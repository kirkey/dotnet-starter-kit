namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Approve.v1;

public sealed record ApproveLoanResponse(Guid Id, string Status, DateOnly ApprovalDate);
