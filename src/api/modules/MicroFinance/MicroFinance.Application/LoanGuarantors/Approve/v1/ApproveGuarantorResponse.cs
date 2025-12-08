namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Approve.v1;

/// <summary>
/// Response after approving a guarantor.
/// </summary>
public sealed record ApproveGuarantorResponse(DefaultIdType Id, string Status, string Message);
