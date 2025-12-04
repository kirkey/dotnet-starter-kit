using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Complete.v1;

public sealed record CompleteInquiryCommand(
    Guid Id,
    string ReferenceNumber,
    int? CreditScore = null,
    Guid? CreditReportId = null) : IRequest<CompleteInquiryResponse>;
