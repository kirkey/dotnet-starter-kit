using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Complete.v1;

public sealed class CompleteInquiryHandler(
    [FromKeyedServices("microfinance:creditbureauinquiries")] IRepository<CreditBureauInquiry> repository,
    ILogger<CompleteInquiryHandler> logger)
    : IRequestHandler<CompleteInquiryCommand, CompleteInquiryResponse>
{
    public async Task<CompleteInquiryResponse> Handle(
        CompleteInquiryCommand request,
        CancellationToken cancellationToken)
    {
        var inquiry = await repository.FirstOrDefaultAsync(
            new CreditBureauInquiryByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Credit bureau inquiry {request.Id} not found");

        inquiry.Complete(request.ReferenceNumber, request.CreditScore, request.CreditReportId);
        await repository.UpdateAsync(inquiry, cancellationToken);

        logger.LogInformation("Credit bureau inquiry completed: {InquiryId}", inquiry.Id);

        return new CompleteInquiryResponse(inquiry.Id, inquiry.Status, inquiry.CreditScore);
    }
}
