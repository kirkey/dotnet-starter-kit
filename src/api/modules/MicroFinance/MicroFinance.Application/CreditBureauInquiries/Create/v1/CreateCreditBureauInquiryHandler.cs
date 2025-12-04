using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Create.v1;

public sealed class CreateCreditBureauInquiryHandler(
    [FromKeyedServices("microfinance:creditbureauinquiries")] IRepository<CreditBureauInquiry> repository,
    ILogger<CreateCreditBureauInquiryHandler> logger)
    : IRequestHandler<CreateCreditBureauInquiryCommand, CreateCreditBureauInquiryResponse>
{
    public async Task<CreateCreditBureauInquiryResponse> Handle(
        CreateCreditBureauInquiryCommand request,
        CancellationToken cancellationToken)
    {
        var inquiry = CreditBureauInquiry.Create(
            request.MemberId,
            request.InquiryNumber,
            request.BureauName,
            request.Purpose,
            request.LoanId,
            request.RequestedBy,
            request.RequestedByUserId,
            request.InquiryCost);

        await repository.AddAsync(inquiry, cancellationToken);

        logger.LogInformation("Credit bureau inquiry created: {InquiryId} for member {MemberId}",
            inquiry.Id, request.MemberId);

        return new CreateCreditBureauInquiryResponse(inquiry.Id);
    }
}
