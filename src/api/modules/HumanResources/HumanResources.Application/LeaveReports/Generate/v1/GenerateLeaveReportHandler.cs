namespace FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Generate.v1;

/// <summary>
/// Handler for generating leave reports.
/// Aggregates leave data and creates report records.
/// </summary>
public sealed class GenerateLeaveReportHandler(
    ILogger<GenerateLeaveReportHandler> logger,
    [FromKeyedServices("hr:leavereports")] IRepository<LeaveReport> repository,
    [FromKeyedServices("hr:leaverequests")] IReadRepository<LeaveRequest> leaveRequestRepository,
    [FromKeyedServices("hr:leavebalances")] IReadRepository<LeaveBalance> leaveBalanceRepository)
    : IRequestHandler<GenerateLeaveReportCommand, GenerateLeaveReportResponse>
{
    /// <summary>
    /// Handles the generate leave report command.
    /// </summary>
    public async Task<GenerateLeaveReportResponse> Handle(
        GenerateLeaveReportCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var fromDate = request.FromDate ?? DateTime.UtcNow.AddMonths(-1);
        var toDate = request.ToDate ?? DateTime.UtcNow;

        // Create report entity
        var report = LeaveReport.Create(
            reportType: request.ReportType,
            title: request.Title,
            fromDate: fromDate,
            toDate: toDate,
            departmentId: request.DepartmentId,
            employeeId: request.EmployeeId);

        if (!string.IsNullOrWhiteSpace(request.Notes))
            report.AddNotes(request.Notes);

        // Fetch leave request data for the period
        var leaveRequestSpec = new LeaveRequestsByDateRangeSpec(fromDate, toDate);
        var leaveRequests = await leaveRequestRepository.ListAsync(leaveRequestSpec, cancellationToken)
            .ConfigureAwait(false);

        // Fetch leave balance data
        var leaveBalances = await leaveBalanceRepository.ListAsync(cancellationToken)
            .ConfigureAwait(false);

        // Aggregate data based on report type
        var (employees, leaveTypes, totalRequests, approved, pending, rejected, consumed) = request.ReportType switch
        {
            "Summary" => AggregateSummary(leaveRequests, leaveBalances),
            "Detailed" => AggregateDetailed(leaveRequests, leaveBalances),
            "Departmental" => AggregateDepartmental(leaveRequests, leaveBalances, request.DepartmentId),
            "Trends" => AggregateTrends(leaveRequests, leaveBalances),
            "Balances" => AggregateBalances(leaveRequests, leaveBalances),
            "EmployeeDetails" => AggregateEmployeeDetails(leaveRequests, leaveBalances, request.EmployeeId),
            _ => (0, 0, 0, 0, 0, 0, 0m)
        };

        // Set metrics
        report.SetMetrics(employees, leaveTypes, totalRequests, approved, pending, rejected, consumed);

        // Persist report
        await repository.AddAsync(report, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Leave report generated: {ReportType}, Requests: {Count}, Approved: {Approved}",
            request.ReportType,
            totalRequests,
            approved);

        return new GenerateLeaveReportResponse(
            ReportId: report.Id,
            ReportType: report.ReportType,
            Title: report.Title,
            GeneratedOn: report.GeneratedOn,
            TotalEmployees: employees,
            TotalLeaveRequests: totalRequests,
            ApprovedLeaveCount: approved,
            PendingLeaveCount: pending,
            TotalLeaveConsumed: consumed);
    }

    /// <summary>
    /// Aggregates data for summary report (company-wide totals).
    /// </summary>
    private static (int employees, int leaveTypes, int requests, int approved, int pending, int rejected, decimal consumed)
        AggregateSummary(List<LeaveRequest> requests, List<LeaveBalance> balances)
    {
        var employees = requests.Select(x => x.EmployeeId).Distinct().Count();
        var leaveTypes = requests.Select(x => x.LeaveTypeId).Distinct().Count();
        var totalRequests = requests.Count;
        var approved = requests.Count(x => x.Status == "Approved");
        var pending = requests.Count(x => x.Status == "Pending");
        var rejected = requests.Count(x => x.Status == "Rejected");
        var consumed = balances.Sum(x => x.TakenDays);

        return (employees, leaveTypes, totalRequests, approved, pending, rejected, consumed);
    }

    /// <summary>
    /// Aggregates data for detailed report (with breakdowns).
    /// </summary>
    private static (int employees, int leaveTypes, int requests, int approved, int pending, int rejected, decimal consumed)
        AggregateDetailed(List<LeaveRequest> requests, List<LeaveBalance> balances)
    {
        return AggregateSummary(requests, balances);
    }

    /// <summary>
    /// Aggregates data filtered by department.
    /// </summary>
    private static (int employees, int leaveTypes, int requests, int approved, int pending, int rejected, decimal consumed)
        AggregateDepartmental(List<LeaveRequest> requests, List<LeaveBalance> balances, DefaultIdType? departmentId)
    {
        // Note: Would require department info in LeaveRequest or Employee relationship
        return AggregateSummary(requests, balances);
    }

    /// <summary>
    /// Aggregates data for trends analysis.
    /// </summary>
    private static (int employees, int leaveTypes, int requests, int approved, int pending, int rejected, decimal consumed)
        AggregateTrends(List<LeaveRequest> requests, List<LeaveBalance> balances)
    {
        return AggregateSummary(requests, balances);
    }

    /// <summary>
    /// Aggregates data for leave balance analysis.
    /// </summary>
    private static (int employees, int leaveTypes, int requests, int approved, int pending, int rejected, decimal consumed)
        AggregateBalances(List<LeaveRequest> requests, List<LeaveBalance> balances)
    {
        var employees = balances.Select(x => x.EmployeeId).Distinct().Count();
        var leaveTypes = balances.Select(x => x.LeaveTypeId).Distinct().Count();
        var totalRequests = requests.Count;
        var approved = requests.Count(x => x.Status == "Approved");
        var pending = requests.Count(x => x.Status == "Pending");
        var rejected = requests.Count(x => x.Status == "Rejected");
        var consumed = balances.Sum(x => x.TakenDays);

        return (employees, leaveTypes, totalRequests, approved, pending, rejected, consumed);
    }

    /// <summary>
    /// Aggregates data for a specific employee.
    /// </summary>
    private static (int employees, int leaveTypes, int requests, int approved, int pending, int rejected, decimal consumed)
        AggregateEmployeeDetails(List<LeaveRequest> requests, List<LeaveBalance> balances, DefaultIdType? employeeId)
    {
        if (!employeeId.HasValue)
            return (0, 0, 0, 0, 0, 0, 0m);

        var filteredRequests = requests.Where(x => x.EmployeeId == employeeId).ToList();
        var filteredBalances = balances.Where(x => x.EmployeeId == employeeId).ToList();

        var employees = filteredRequests.Select(x => x.EmployeeId).Distinct().Count();
        var leaveTypes = filteredRequests.Select(x => x.LeaveTypeId).Distinct().Count();
        var totalRequests = filteredRequests.Count;
        var approved = filteredRequests.Count(x => x.Status == "Approved");
        var pending = filteredRequests.Count(x => x.Status == "Pending");
        var rejected = filteredRequests.Count(x => x.Status == "Rejected");
        var consumed = filteredBalances.Sum(x => x.TakenDays);

        return (employees, leaveTypes, totalRequests, approved, pending, rejected, consumed);
    }
}

/// <summary>
/// Specification for filtering leave requests by date range.
/// </summary>
public sealed class LeaveRequestsByDateRangeSpec : Specification<LeaveRequest>
{
    /// <summary>
    /// Initializes the specification with date range filter.
    /// </summary>
    public LeaveRequestsByDateRangeSpec(DateTime fromDate, DateTime toDate)
    {
        Query.Where(x => x.StartDate >= fromDate && x.EndDate <= toDate)
            .OrderByDescending(x => x.StartDate);
    }
}

