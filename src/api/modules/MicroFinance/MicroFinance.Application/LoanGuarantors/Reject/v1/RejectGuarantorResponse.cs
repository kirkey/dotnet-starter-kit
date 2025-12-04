namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Reject.v1;

/// <summary>
/// Response after rejecting a guarantor.
/// </summary>
public sealed record RejectGuarantorResponse(Guid Id, string Status, string Message);
