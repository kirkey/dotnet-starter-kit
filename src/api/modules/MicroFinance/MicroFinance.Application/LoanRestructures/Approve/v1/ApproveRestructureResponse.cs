namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Approve.v1;

public sealed record ApproveRestructureResponse(Guid Id, string Status, DateOnly EffectiveDate);
