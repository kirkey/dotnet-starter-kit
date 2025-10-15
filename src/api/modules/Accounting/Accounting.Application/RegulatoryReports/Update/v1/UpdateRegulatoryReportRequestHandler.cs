using Accounting.Application.RegulatoryReports.Queries;
using Accounting.Domain.Entities;

namespace Accounting.Application.RegulatoryReports.Update.v1;

public sealed class UpdateRegulatoryReportRequestHandler(
    ILogger<UpdateRegulatoryReportRequestHandler> logger,
    [FromKeyedServices("accounting:regulatoryreports")] IRepository<RegulatoryReport> repository)
    : IRequestHandler<UpdateRegulatoryReportRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateRegulatoryReportRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await repository.FirstOrDefaultAsync(
            new RegulatoryReportByIdEntitySpec(request.Id), cancellationToken);

        if (report == null)
        {
            throw new RegulatoryReportNotFoundException(request.Id);
        }

        // Update financial data if provided
        report.UpdateFinancialData(
            totalAssets: request.TotalAssets,
            totalLiabilities: request.TotalLiabilities,
            totalEquity: request.TotalEquity,
            totalRevenue: request.TotalRevenue,
            totalExpenses: request.TotalExpenses,
            netIncome: request.NetIncome,
            rateBase: request.RateBase,
            allowedReturn: request.AllowedReturn);

        await repository.UpdateAsync(report, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Regulatory report {ReportName} updated with ID {Id}", request.ReportName, report.Id);

        return report.Id;
    }
}
