using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Reject.v1;

public sealed record RejectKycDocumentCommand(
    Guid Id,
    Guid RejectedById,
    string Reason) : IRequest<RejectKycDocumentResponse>;
