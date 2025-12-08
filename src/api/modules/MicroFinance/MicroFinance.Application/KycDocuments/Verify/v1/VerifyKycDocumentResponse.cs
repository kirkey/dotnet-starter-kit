namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Verify.v1;

public sealed record VerifyKycDocumentResponse(DefaultIdType Id, string Status, DateTime VerifiedAt);
