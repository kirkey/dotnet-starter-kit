namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Verify.v1;

/// <summary>
/// Response after verifying a document.
/// </summary>
public sealed record VerifyDocumentResponse(DefaultIdType Id, bool IsVerified, DateTimeOffset VerifiedAt);
