namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.UpdateAmount.v1;

/// <summary>
/// Response after updating guaranteed amount.
/// </summary>
public sealed record UpdateGuaranteedAmountResponse(DefaultIdType Id, decimal GuaranteedAmount, string Message);
