namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Approve.v1;

public sealed record ApproveSettlementResponse(DefaultIdType Id, string Status, DateOnly ApprovedDate);
