using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Create.v1;

public sealed record CreateCreditBureauInquiryCommand(
    Guid MemberId,
    string InquiryNumber,
    string BureauName,
    string Purpose,
    Guid? LoanId = null,
    string? RequestedBy = null,
    Guid? RequestedByUserId = null,
    decimal? InquiryCost = null) : IRequest<CreateCreditBureauInquiryResponse>;
