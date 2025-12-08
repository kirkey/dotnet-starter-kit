namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Approve.v1;

public sealed record ApproveLoanResponse(DefaultIdType Id, string Status, DateOnly ApprovalDate);
