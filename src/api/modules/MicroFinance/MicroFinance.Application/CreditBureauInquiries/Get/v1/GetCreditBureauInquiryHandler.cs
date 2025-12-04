using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Get.v1;

public sealed class GetCreditBureauInquiryHandler(
    [FromKeyedServices("microfinance:creditbureauinquiries")] IReadRepository<CreditBureauInquiry> repository)
    : IRequestHandler<GetCreditBureauInquiryRequest, CreditBureauInquiryResponse>
{
    public async Task<CreditBureauInquiryResponse> Handle(
        GetCreditBureauInquiryRequest request,
        CancellationToken cancellationToken)
    {
        var inquiry = await repository.FirstOrDefaultAsync(
            new CreditBureauInquiryByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Credit bureau inquiry {request.Id} not found");

        return new CreditBureauInquiryResponse(
            inquiry.Id,
            inquiry.MemberId,
            inquiry.LoanId,
            inquiry.InquiryNumber,
            inquiry.BureauName,
            inquiry.Purpose,
            inquiry.InquiryDate,
            inquiry.RequestedBy,
            inquiry.RequestedByUserId,
            inquiry.ReferenceNumber,
            inquiry.Status,
            inquiry.ResponseReceivedAt,
            inquiry.CreditScore,
            inquiry.CreditReportId,
            inquiry.InquiryCost,
            inquiry.ErrorMessage,
            inquiry.Notes);
    }
}
