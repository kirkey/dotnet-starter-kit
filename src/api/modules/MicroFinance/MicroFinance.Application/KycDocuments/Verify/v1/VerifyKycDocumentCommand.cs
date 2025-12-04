using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Verify.v1;

public sealed record VerifyKycDocumentCommand(
    Guid Id,
    Guid VerifiedById,
    string? Notes = null) : IRequest<VerifyKycDocumentResponse>;
