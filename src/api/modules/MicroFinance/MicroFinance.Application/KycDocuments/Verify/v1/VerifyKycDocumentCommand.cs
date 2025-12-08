using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Verify.v1;

public sealed record VerifyKycDocumentCommand(
    DefaultIdType Id,
    DefaultIdType VerifiedById,
    string? Notes = null) : IRequest<VerifyKycDocumentResponse>;
