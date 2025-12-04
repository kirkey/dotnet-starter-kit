namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Reject.v1;

public sealed record RejectValuationResponse(Guid Id, string Status, string RejectionReason);
