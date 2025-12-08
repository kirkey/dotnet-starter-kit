namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Reject.v1;

public sealed record RejectValuationResponse(DefaultIdType Id, string Status, string RejectionReason);
