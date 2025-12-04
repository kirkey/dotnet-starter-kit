namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Reject.v1;

public sealed record RejectKycDocumentResponse(Guid Id, string Status, string RejectionReason);
