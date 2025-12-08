using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Reject.v1;

public sealed record RejectKycDocumentCommand(
    DefaultIdType Id,
    DefaultIdType RejectedById,
    string Reason) : IRequest<RejectKycDocumentResponse>;
