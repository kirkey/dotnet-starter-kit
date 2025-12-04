namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Verify.v1;

public sealed record VerifyKycDocumentResponse(Guid Id, string Status, DateTime VerifiedAt);
