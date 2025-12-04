namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Approve.v1;

public sealed record ApproveSettlementResponse(Guid Id, string Status, DateOnly ApprovedDate);
