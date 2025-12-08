namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Reject.v1;

/// <summary>
/// Response after rejecting a guarantor.
/// </summary>
public sealed record RejectGuarantorResponse(DefaultIdType Id, string Status, string Message);
