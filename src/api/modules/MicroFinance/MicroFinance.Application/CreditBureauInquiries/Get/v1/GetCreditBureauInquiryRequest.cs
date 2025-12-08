using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Get.v1;

public sealed record GetCreditBureauInquiryRequest(DefaultIdType Id) : IRequest<CreditBureauInquiryResponse>;
