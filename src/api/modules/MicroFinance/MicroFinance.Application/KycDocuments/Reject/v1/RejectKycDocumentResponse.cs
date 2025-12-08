namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Reject.v1;

public sealed record RejectKycDocumentResponse(DefaultIdType Id, string Status, string RejectionReason);
