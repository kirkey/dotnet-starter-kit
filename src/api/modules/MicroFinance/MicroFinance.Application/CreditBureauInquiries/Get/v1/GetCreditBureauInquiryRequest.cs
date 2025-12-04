using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Get.v1;

public sealed record GetCreditBureauInquiryRequest(Guid Id) : IRequest<CreditBureauInquiryResponse>;
