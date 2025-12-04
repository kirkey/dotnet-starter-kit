namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Approve.v1;

public sealed record ApproveTrancheResponse(Guid Id, string Status, DateTime ApprovedAt);
