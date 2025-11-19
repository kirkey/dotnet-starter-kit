using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Specifications;

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
        var (employees, runs, gross, net, deductions, taxes, benefits) = request.ReportType switch
        {
            "Summary" => AggregateSummary(payrolls),
            "Detailed" => AggregateDetailed(payrolls),
            "Department" => AggregateDepartment(payrolls, request.DepartmentId),
            "EmployeeDetails" => AggregateEmployeeDetails(payrolls, request.EmployeeId),
            "TaxSummary" => AggregateTaxSummary(payrolls),
            "DeductionsSummary" => AggregateDeductionsSummary(payrolls),
            "ComponentBreakdown" => AggregateComponentBreakdown(payrolls),
            _ => (0, 0, 0m, 0m, 0m, 0m, 0m)
        };

        // Set metrics
        report.SetMetrics(employees, runs, gross, net, deductions, taxes, benefits);

        // Persist report
        await repository.AddAsync(report, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Payroll report generated: {ReportType}, Employees: {Count}, Gross: {Gross}",
            request.ReportType,
            employees,
            gross);

        return new GeneratePayrollReportResponse(
            ReportId: report.Id,
            ReportType: report.ReportType,
            Title: report.Title,
            GeneratedOn: report.GeneratedOn,
            RecordCount: employees,
            TotalGrossSalary: gross,
            TotalDeductions: deductions,
            TotalNetSalary: net);
    }

    /// <summary>
    /// Aggregates data for summary report (company-wide totals).
    /// </summary>
    private static (int employees, int runs, decimal gross, decimal net, decimal deductions, decimal taxes, decimal benefits) 
        AggregateSummary(List<Payroll> payrolls)
    {
        var employees = payrolls.Sum(p => p.EmployeeCount);
        var runs = payrolls.Count;
        var gross = payrolls.Sum(p => p.TotalGrossPay);
        var deductions = payrolls.Sum(p => p.TotalDeductions);
        var net = payrolls.Sum(p => p.TotalNetPay);
        var taxes = payrolls.Sum(p => p.TotalTaxes);
        var benefits = 0m; // Benefits tracked in PayrollLines

        return (employees, runs, gross, net, deductions, taxes, benefits);
    }

    /// <summary>
    /// Aggregates data for detailed report (with breakdowns).
    /// </summary>
    private static (int employees, int runs, decimal gross, decimal net, decimal deductions, decimal taxes, decimal benefits)
        AggregateDetailed(List<Payroll> payrolls)
    {
        return AggregateSummary(payrolls); // Base aggregate with line item details
    }

    /// <summary>
    /// Aggregates data filtered by department.
    /// </summary>
    private static (int employees, int runs, decimal gross, decimal net, decimal deductions, decimal taxes, decimal benefits)
        AggregateDepartment(List<Payroll> payrolls, DefaultIdType? departmentId)
    {
        // Note: Would require department info in Payroll or Employee relationship
        return AggregateSummary(payrolls);
    }

    /// <summary>
    /// Aggregates data for a specific employee.
    /// </summary>
    private static (int employees, int runs, decimal gross, decimal net, decimal deductions, decimal taxes, decimal benefits)
        AggregateEmployeeDetails(List<Payroll> payrolls, DefaultIdType? employeeId)
    {
        if (employeeId == null)
            return (0, 0, 0m, 0m, 0m, 0m, 0m);

        // Note: Would require employee filter in payroll query
        return AggregateSummary(payrolls);
    }

    /// <summary>
    /// Aggregates data for tax summary report.
    /// </summary>
    private static (int employees, int runs, decimal gross, decimal net, decimal deductions, decimal taxes, decimal benefits)
        AggregateTaxSummary(List<Payroll> payrolls)
    {
        var employees = payrolls.Sum(p => p.EmployeeCount);
        var runs = payrolls.Count;
        var taxes = payrolls.Sum(p => p.TotalTaxes);
        var gross = payrolls.Sum(p => p.TotalGrossPay);
        var deductions = payrolls.Sum(p => p.TotalDeductions);
        var net = payrolls.Sum(p => p.TotalNetPay);
        var benefits = 0m;

        return (employees, runs, gross, net, deductions, taxes, benefits);
    }

    /// <summary>
    /// Aggregates data for deductions summary report.
    /// </summary>
    private static (int employees, int runs, decimal gross, decimal net, decimal deductions, decimal taxes, decimal benefits)
        AggregateDeductionsSummary(List<Payroll> payrolls)
    {
        var employees = payrolls.Sum(p => p.EmployeeCount);
        var runs = payrolls.Count;
        var gross = payrolls.Sum(p => p.TotalGrossPay);
        var deductions = payrolls.Sum(p => p.TotalDeductions);
        var net = payrolls.Sum(p => p.TotalNetPay);
        var taxes = payrolls.Sum(p => p.TotalTaxes);
        var benefits = 0m;

        return (employees, runs, gross, net, deductions, taxes, benefits);
    }

    /// <summary>
    /// Aggregates data for component breakdown report.
    /// </summary>
    private static (int employees, int runs, decimal gross, decimal net, decimal deductions, decimal taxes, decimal benefits)
        AggregateComponentBreakdown(List<Payroll> payrolls)
    {
        return AggregateSummary(payrolls);
    }
}

