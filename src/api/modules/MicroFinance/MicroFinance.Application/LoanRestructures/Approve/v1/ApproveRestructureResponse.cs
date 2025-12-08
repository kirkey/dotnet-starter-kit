namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Approve.v1;

public sealed record ApproveRestructureResponse(DefaultIdType Id, string Status, DateOnly EffectiveDate);
