using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Complete.v1;

public sealed record CompleteInquiryCommand(
    DefaultIdType Id,
    string ReferenceNumber,
    int? CreditScore = null,
    DefaultIdType? CreditReportId = null) : IRequest<CompleteInquiryResponse>;
