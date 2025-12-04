using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauReports.Get.v1;

public sealed class GetCreditBureauReportHandler(
    [FromKeyedServices("microfinance:creditbureaureports")] IReadRepository<CreditBureauReport> repository)
    : IRequestHandler<GetCreditBureauReportRequest, CreditBureauReportResponse>
{
    public async Task<CreditBureauReportResponse> Handle(GetCreditBureauReportRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var report = await repository.FirstOrDefaultAsync(new CreditBureauReportByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Credit bureau report with id {request.Id} not found");

        return new CreditBureauReportResponse(
            report.Id,
            report.MemberId,
            report.InquiryId,
            report.ReportNumber,
            report.BureauName,
            report.ReportDate,
            report.ExpiryDate,
            report.CreditScore,
            report.ScoreMin,
            report.ScoreMax,
            report.ScoreModel,
            report.RiskGrade,
            report.ActiveAccounts,
            report.ClosedAccounts,
            report.DelinquentAccounts,
            report.TotalOutstandingBalance,
            report.TotalCreditLimit,
            report.CreditUtilization,
            report.RecentInquiries,
            report.CreditHistoryMonths,
            report.LatePayments12Months,
            report.LatePayments24Months,
            report.Defaults,
            report.Bankruptcies,
            report.Collections,
            report.PublicRecords,
            report.DebtToIncomeRatio,
            report.Status,
            report.Notes,
            report.CreatedOn);
    }
}
