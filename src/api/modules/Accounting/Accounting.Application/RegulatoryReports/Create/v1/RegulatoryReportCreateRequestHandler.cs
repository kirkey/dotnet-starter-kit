namespace Accounting.Application.RegulatoryReports.Create.v1;

public sealed class RegulatoryReportCreateRequestHandler(
    ILogger<RegulatoryReportCreateRequestHandler> logger,
    [FromKeyedServices("accounting:regulatory-reports")] IRepository<RegulatoryReport> repository)
    : IRequestHandler<RegulatoryReportCreateRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(RegulatoryReportCreateRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Validate report type
        var validReportTypes = new[] { "FERC Form 1", "FERC Form 2", "FERC Form 6", "EIA Form 861", "State Commission" };
        if (!validReportTypes.Contains(request.ReportType))
        {
            throw new RegulatoryReportForbiddenException($"Invalid report type: {request.ReportType}");
        }

        // Validate reporting period
        var validPeriods = new[] { "Annual", "Monthly", "Quarterly" };
        if (!validPeriods.Contains(request.ReportingPeriod))
        {
            throw new RegulatoryReportForbiddenException($"Invalid reporting period: {request.ReportingPeriod}");
        }

        var report = RegulatoryReport.Create(
            reportName: request.ReportName,
            reportType: request.ReportType,
            reportingPeriod: request.ReportingPeriod,
            periodStartDate: request.PeriodStartDate,
            periodEndDate: request.PeriodEndDate,
            dueDate: request.DueDate,
            regulatoryBody: request.RegulatoryBody,
            requiresAudit: request.RequiresAudit,
            description: request.Description,
            notes: request.Notes);

        await repository.AddAsync(report, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Regulatory report {ReportName} created with ID {Id}", request.ReportName, report.Id);

        return report.Id;
    }
}
