using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauReports.Create.v1;

public sealed class CreateCreditBureauReportHandler(
    ILogger<CreateCreditBureauReportHandler> logger,
    [FromKeyedServices("microfinance:creditbureaureports")] IRepository<CreditBureauReport> repository)
    : IRequestHandler<CreateCreditBureauReportCommand, CreateCreditBureauReportResponse>
{
    public async Task<CreateCreditBureauReportResponse> Handle(CreateCreditBureauReportCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = CreditBureauReport.Create(
            request.MemberId,
            request.ReportNumber,
            request.BureauName,
            request.ReportDate,
            request.InquiryId,
            request.CreditScore,
            request.ScoreMin,
            request.ScoreMax,
            request.ScoreModel,
            request.ExpiryDate);

        await repository.AddAsync(report, cancellationToken);
        logger.LogInformation("Credit bureau report {ReportNumber} created with ID {Id}", report.ReportNumber, report.Id);

        return new CreateCreditBureauReportResponse(report.Id, report.ReportNumber);
    }
}
