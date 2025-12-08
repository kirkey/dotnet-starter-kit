using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Create.v1;

public sealed record CreateCreditBureauInquiryCommand(
    DefaultIdType MemberId,
    string InquiryNumber,
    string BureauName,
    string Purpose,
    DefaultIdType? LoanId = null,
    string? RequestedBy = null,
    DefaultIdType? RequestedByUserId = null,
    decimal? InquiryCost = null) : IRequest<CreateCreditBureauInquiryResponse>;
