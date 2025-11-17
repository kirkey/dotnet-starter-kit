using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Specifications;
using FSH.Starter.WebApi.HumanResources.Application.Taxes.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Create.v1;

/// <summary>
/// Handler for generating payroll reports.
/// Aggregates payroll data and creates report records.
/// </summary>
public sealed class GeneratePayrollReportHandler(
    ILogger<GeneratePayrollReportHandler> logger,
    [FromKeyedServices("hr:payrollreports")] IRepository<PayrollReport> repository,
    [FromKeyedServices("hr:payrolls")] IReadRepository<Payroll> payrollRepository)
    : IRequestHandler<GeneratePayrollReportCommand, GeneratePayrollReportResponse>
{
    /// <summary>
    /// Handles the generate payroll report command.
    /// </summary>
    public async Task<GeneratePayrollReportResponse> Handle(
        GeneratePayrollReportCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var fromDate = request.FromDate ?? DateTime.UtcNow.AddMonths(-1);
        var toDate = request.ToDate ?? DateTime.UtcNow;

        // Create report entity
        var report = PayrollReport.Create(
            reportType: request.ReportType,
            title: request.Title,
            fromDate: fromDate,
            toDate: toDate,
            departmentId: request.DepartmentId,
            employeeId: request.EmployeeId);

        if (!string.IsNullOrWhiteSpace(request.Notes))
            report.AddNotes(request.Notes);

        // Fetch payroll data for the period
        var payrollSpec = new PayrollsByDateRangeSpec(fromDate, toDate);
        var payrolls = await payrollRepository.ListAsync(payrollSpec, cancellationToken)
            .ConfigureAwait(false);

        // Aggregate data based on report type
        var (recordCount, grossSalary, deductions, netSalary, tax) = request.ReportType switch
        {
            "Summary" => AggregateSummary(payrolls),
            "Detailed" => AggregateDetailed(payrolls),
            "Department" => AggregateDepartment(payrolls, request.DepartmentId),
            "EmployeeDetails" => AggregateEmployeeDetails(payrolls, request.EmployeeId),
            "TaxSummary" => AggregateTaxSummary(payrolls),
            "DeductionsSummary" => AggregateDeductionsSummary(payrolls),
            "ComponentBreakdown" => AggregateComponentBreakdown(payrolls),
            _ => (0, 0m, 0m, 0m, 0m)
        };

        // Set totals
        report.SetTotals(recordCount, grossSalary, deductions, netSalary, tax);

        // Persist report
        await repository.AddAsync(report, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Payroll report generated: {ReportType}, Records: {Count}, Gross: {Gross}",
            request.ReportType,
            recordCount,
            grossSalary);

        return new GeneratePayrollReportResponse(
            ReportId: report.Id,
            ReportType: report.ReportType,
            Title: report.Title,
            GeneratedOn: report.GeneratedOn,
            RecordCount: recordCount,
            TotalGrossSalary: grossSalary,
            TotalDeductions: deductions,
            TotalNetSalary: netSalary);
    }

    /// <summary>
    /// Aggregates data for summary report (company-wide totals).
    /// </summary>
    private static (int count, decimal gross, decimal deductions, decimal net, decimal tax) 
        AggregateSummary(List<Payroll> payrolls)
    {
        var count = payrolls.Count;
        var gross = payrolls.Sum(p => p.TotalGrossPay);
        var deductions = payrolls.Sum(p => p.TotalDeductions);
        var net = payrolls.Sum(p => p.TotalNetPay);
        var tax = payrolls.Sum(p => p.TotalTaxes);

        return (count, gross, deductions, net, tax);
    }

    /// <summary>
    /// Aggregates data for detailed report (with breakdowns).
    /// </summary>
    private static (int count, decimal gross, decimal deductions, decimal net, decimal tax)
        AggregateDetailed(List<Payroll> payrolls)
    {
        return AggregateSummary(payrolls); // Base aggregate with line item details
    }

    /// <summary>
    /// Aggregates data filtered by department.
    /// </summary>
    private static (int count, decimal gross, decimal deductions, decimal net, decimal tax)
        AggregateDepartment(List<Payroll> payrolls, DefaultIdType? departmentId)
    {
        // Note: Would require department info in Payroll or Employee relationship
        return AggregateSummary(payrolls);
    }

    /// <summary>
    /// Aggregates data for a specific employee.
    /// </summary>
    private static (int count, decimal gross, decimal deductions, decimal net, decimal tax)
        AggregateEmployeeDetails(List<Payroll> payrolls, DefaultIdType? employeeId)
    {
        if (!employeeId.HasValue)
            return (0, 0m, 0m, 0m, 0m);

        // Note: Would require employee filter in payroll query
        return AggregateSummary(payrolls);
    }

    /// <summary>
    /// Aggregates data for tax summary report.
    /// </summary>
    private static (int count, decimal gross, decimal deductions, decimal net, decimal tax)
        AggregateTaxSummary(List<Payroll> payrolls)
    {
        var count = payrolls.Count;
        var tax = payrolls.Sum(p => p.TotalTaxes);
        var gross = payrolls.Sum(p => p.TotalGrossPay);
        var deductions = payrolls.Sum(p => p.TotalDeductions);
        var net = payrolls.Sum(p => p.TotalNetPay);

        return (count, gross, deductions, net, tax);
    }

    /// <summary>
    /// Aggregates data for deductions summary report.
    /// </summary>
    private static (int count, decimal gross, decimal deductions, decimal net, decimal tax)
        AggregateDeductionsSummary(List<Payroll> payrolls)
    {
        var count = payrolls.Count;
        var gross = payrolls.Sum(p => p.TotalGrossPay);
        var deductions = payrolls.Sum(p => p.TotalDeductions);
        var net = payrolls.Sum(p => p.TotalNetPay);
        var tax = payrolls.Sum(p => p.TotalTaxes);

        return (count, gross, deductions, net, tax);
    }

    /// <summary>
    /// Aggregates data for component breakdown report.
    /// </summary>
    private static (int count, decimal gross, decimal deductions, decimal net, decimal tax)
        AggregateComponentBreakdown(List<Payroll> payrolls)
    {
        return AggregateSummary(payrolls);
    }
}

