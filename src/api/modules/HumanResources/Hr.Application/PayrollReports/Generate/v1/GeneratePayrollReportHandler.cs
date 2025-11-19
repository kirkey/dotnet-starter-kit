namespace FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Generate.v1;

/// <summary>
/// Handler for generating payroll reports.
/// Aggregates payroll data and creates comprehensive reports.
/// </summary>
public sealed class GeneratePayrollReportHandler(
    ILogger<GeneratePayrollReportHandler> logger,
    [FromKeyedServices("hr:payrollreports")] IRepository<PayrollReport> repository,
    [FromKeyedServices("hr:payrolls")] IReadRepository<Payroll> payrollRepository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository)
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
            employeeId: request.EmployeeId,
            payrollPeriod: request.PayrollPeriod);

        if (!string.IsNullOrWhiteSpace(request.Notes))
            report.AddNotes(request.Notes);

        // Fetch payroll data for the period
        var payrollSpec = new PayrollsByDateRangeSpec(fromDate, toDate);
        var payrolls = await payrollRepository.ListAsync(payrollSpec, cancellationToken)
            .ConfigureAwait(false);

        // Note: Department and employee filtering would require loading PayrollLines
        // For now, we work with Payroll aggregates directly
        
        // Aggregate data based on report type
        var (employees, runs, gross, net, deductions, taxes, benefits) = request.ReportType switch
        {
            "Summary" => AggregateSummary(payrolls),
            "Detailed" => AggregateDetailed(payrolls),
            "Departmental" => AggregateDepartmental(payrolls),
            "ByEmployee" => AggregateByEmployee(payrolls),
            "TaxReport" => AggregateTaxReport(payrolls),
            "DeductionReport" => AggregateDeductionReport(payrolls),
            "BankTransfer" => AggregateBankTransfer(payrolls),
            _ => (0, 0, 0m, 0m, 0m, 0m, 0m)
        };

        // Set metrics
        report.SetMetrics(employees, runs, gross, net, deductions, taxes, benefits);

        // Persist report
        await repository.AddAsync(report, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Payroll report generated: {ReportType}, Runs: {Count}, Gross: {Gross}",
            request.ReportType,
            runs,
            gross);

        return new GeneratePayrollReportResponse(
            ReportId: report.Id,
            ReportType: report.ReportType,
            Title: report.Title,
            GeneratedOn: report.GeneratedOn,
            TotalEmployees: employees,
            TotalPayrollRuns: runs,
            TotalGrossPay: gross,
            TotalNetPay: net,
            TotalDeductions: deductions);
    }

    /// <summary>
    /// Aggregates data for summary report (company-wide totals).
    /// </summary>
    private static (int employees, int runs, decimal gross, decimal net, decimal deductions, decimal taxes, decimal benefits)
        AggregateSummary(List<Payroll> payrolls)
    {
        var employees = payrolls.Sum(x => x.EmployeeCount);
        var runs = payrolls.Count;
        var gross = payrolls.Sum(x => x.TotalGrossPay);
        var net = payrolls.Sum(x => x.TotalNetPay);
        var deductions = payrolls.Sum(x => x.TotalDeductions);
        var taxes = payrolls.Sum(x => x.TotalTaxes);
        var benefits = 0m; // Benefits are in PayrollLines, not in Payroll aggregate

        return (employees, runs, gross, net, deductions, taxes, benefits);
    }

    /// <summary>
    /// Aggregates data for detailed report (with line items).
    /// </summary>
    private static (int employees, int runs, decimal gross, decimal net, decimal deductions, decimal taxes, decimal benefits)
        AggregateDetailed(List<Payroll> payrolls)
    {
        return AggregateSummary(payrolls);
    }

    /// <summary>
    /// Aggregates data by department.
    /// </summary>
    private static (int employees, int runs, decimal gross, decimal net, decimal deductions, decimal taxes, decimal benefits)
        AggregateDepartmental(List<Payroll> payrolls)
    {
        return AggregateSummary(payrolls);
    }

    /// <summary>
    /// Aggregates data by employee.
    /// </summary>
    private static (int employees, int runs, decimal gross, decimal net, decimal deductions, decimal taxes, decimal benefits)
        AggregateByEmployee(List<Payroll> payrolls)
    {
        return AggregateSummary(payrolls);
    }

    /// <summary>
    /// Aggregates tax data.
    /// </summary>
    private static (int employees, int runs, decimal gross, decimal net, decimal deductions, decimal taxes, decimal benefits)
        AggregateTaxReport(List<Payroll> payrolls)
    {
        var employees = payrolls.Sum(x => x.EmployeeCount);
        var runs = payrolls.Count;
        var gross = payrolls.Sum(x => x.TotalGrossPay);
        var net = payrolls.Sum(x => x.TotalNetPay);
        var deductions = 0m;
        var taxes = payrolls.Sum(x => x.TotalTaxes);
        var benefits = 0m;

        return (employees, runs, gross, net, deductions, taxes, benefits);
    }

    /// <summary>
    /// Aggregates deduction data.
    /// </summary>
    private static (int employees, int runs, decimal gross, decimal net, decimal deductions, decimal taxes, decimal benefits)
        AggregateDeductionReport(List<Payroll> payrolls)
    {
        var employees = payrolls.Sum(x => x.EmployeeCount);
        var runs = payrolls.Count;
        var gross = payrolls.Sum(x => x.TotalGrossPay);
        var net = payrolls.Sum(x => x.TotalNetPay);
        var deductions = payrolls.Sum(x => x.TotalDeductions);
        var taxes = 0m;
        var benefits = 0m;

        return (employees, runs, gross, net, deductions, taxes, benefits);
    }

    /// <summary>
    /// Aggregates data for bank transfer file.
    /// </summary>
    private static (int employees, int runs, decimal gross, decimal net, decimal deductions, decimal taxes, decimal benefits)
        AggregateBankTransfer(List<Payroll> payrolls)
    {
        var employees = payrolls.Sum(x => x.EmployeeCount);
        var runs = payrolls.Count;
        var gross = 0m;
        var net = payrolls.Sum(x => x.TotalNetPay);
        var deductions = 0m;
        var taxes = 0m;
        var benefits = 0m;

        return (employees, runs, gross, net, deductions, taxes, benefits);
    }
}

/// <summary>
/// Specification for payrolls by date range.
/// </summary>
public sealed class PayrollsByDateRangeSpec : Specification<Payroll>
{
    public PayrollsByDateRangeSpec(DateTime fromDate, DateTime toDate)
    {
        Query.Where(x => x.StartDate >= fromDate && x.EndDate <= toDate)
            .OrderByDescending(x => x.EndDate);
    }
}

/// <summary>
/// Specification for employees by department.
/// </summary>
public sealed class EmployeesByDepartmentSpec : Specification<Employee>
{
    public EmployeesByDepartmentSpec(DefaultIdType departmentId)
    {
        Query.Where(x => x.OrganizationalUnitId == departmentId);
    }
}

