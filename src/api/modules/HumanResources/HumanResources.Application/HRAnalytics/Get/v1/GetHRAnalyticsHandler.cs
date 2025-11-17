namespace FSH.Starter.WebApi.HumanResources.Application.HRAnalytics.Get.v1;

/// <summary>
/// Handler for retrieving HR analytics metrics.
/// Aggregates data from multiple HR entities for strategic insights.
/// </summary>
public sealed class GetHrAnalyticsHandler(
    ILogger<GetHrAnalyticsHandler> logger,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:attendance")] IReadRepository<Attendance> attendanceRepository,
    [FromKeyedServices("hr:leaverequests")] IReadRepository<LeaveRequest> leaveRequestRepository,
    [FromKeyedServices("hr:payrolls")] IReadRepository<Payroll> payrollRepository,
    [FromKeyedServices("hr:performancereviews")] IReadRepository<PerformanceReview> performanceReviewRepository,
    [FromKeyedServices("hr:leavebalances")] IReadRepository<LeaveBalance> leaveBalanceRepository,
    [FromKeyedServices("hr:leaveTypes")] IReadRepository<LeaveType> leaveTypeRepository)
    : IRequestHandler<GetHrAnalyticsRequest, HrAnalyticsResponse>
{
    /// <summary>
    /// Handles the get HR analytics request.
    /// </summary>
    public async Task<HrAnalyticsResponse> Handle(
        GetHrAnalyticsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var fromDate = request.FromDate ?? DateTime.UtcNow.AddMonths(-1);
        var toDate = request.ToDate ?? DateTime.UtcNow;

        logger.LogInformation("Generating HR analytics for period {FromDate} to {ToDate}", fromDate, toDate);

        // Fetch all data in parallel
        var headcountTask = GetHeadcountMetricsAsync(request.DepartmentId, cancellationToken);
        var attendanceTask = GetAttendanceMetricsAsync(fromDate, toDate, request.DepartmentId, cancellationToken);
        var leaveTask = GetLeaveMetricsAsync(fromDate, toDate, request.DepartmentId, cancellationToken);
        var payrollTask = GetPayrollMetricsAsync(fromDate, toDate, request.DepartmentId, cancellationToken);
        var performanceTask = GetPerformanceMetricsAsync(request.DepartmentId, cancellationToken);
        var turnoverTask = GetTurnoverMetricsAsync(fromDate, toDate, request.DepartmentId, cancellationToken);
        var departmentTask = GetDepartmentMetricsAsync(fromDate, toDate, cancellationToken);
        var trendsTask = GetHrTrendsAsync(fromDate, toDate, cancellationToken);
        var complianceTask = GetComplianceMetricsAsync(request.DepartmentId, cancellationToken);

        await Task.WhenAll(
            headcountTask, attendanceTask, leaveTask, payrollTask,
            performanceTask, turnoverTask, departmentTask, trendsTask, complianceTask)
            .ConfigureAwait(false);

        var response = new HrAnalyticsResponse(
            ReportDate: DateTime.UtcNow,
            PeriodStart: fromDate,
            PeriodEnd: toDate,
            HeadcountMetrics: await headcountTask,
            AttendanceMetrics: await attendanceTask,
            LeaveMetrics: await leaveTask,
            PayrollMetrics: await payrollTask,
            PerformanceMetrics: await performanceTask,
            TurnoverMetrics: await turnoverTask,
            DepartmentMetrics: await departmentTask,
            Trends: await trendsTask,
            ComplianceMetrics: await complianceTask,
            GeneratedAt: DateTime.UtcNow);

        logger.LogInformation("HR analytics generated successfully");

        return response;
    }

    private async Task<HeadcountMetricsDto> GetHeadcountMetricsAsync(
        DefaultIdType? departmentId,
        CancellationToken cancellationToken)
    {
        var employees = await employeeRepository.ListAsync(cancellationToken).ConfigureAwait(false);

        var departmentEmployees = departmentId.HasValue
            ? employees.Where(e => e.OrganizationalUnitId == departmentId).ToList()
            : employees;

        var activeCount = departmentEmployees.Count(e => e.Status == "Active" && e.IsActive);
        var terminatedCount = departmentEmployees.Count(e => e.Status == "Terminated");
        var onLeaveCount = departmentEmployees.Count(e => e.Status == "OnLeave");
        var totalCount = departmentEmployees.Count;

        var previousEmployees = employees.Count;
        var growthPercent = previousEmployees > 0
            ? Math.Round(((decimal)(activeCount - previousEmployees) / previousEmployees) * 100, 2)
            : 0m;

        return new HeadcountMetricsDto(
            TotalEmployees: totalCount,
            ActiveEmployees: activeCount,
            TerminatedEmployees: terminatedCount,
            OnLeaveEmployees: onLeaveCount,
            HeadcountGrowthPercent: growthPercent);
    }

    private async Task<AttendanceMetricsDto> GetAttendanceMetricsAsync(
        DateTime fromDate,
        DateTime toDate,
        DefaultIdType? departmentId,
        CancellationToken cancellationToken)
    {
        var records = await attendanceRepository.ListAsync(
            new AttendanceByDateRangeSpec(fromDate, toDate),
            cancellationToken)
            .ConfigureAwait(false);

        var presentCount = records.Count(x => x.Status == "Present");
        var absentCount = records.Count(x => x.Status == "Absent");
        var lateCount = records.Count(x => x.Status == "Late");
        var totalRecords = records.Count;

        var attendancePercent = totalRecords > 0
            ? Math.Round(((decimal)presentCount / totalRecords) * 100, 2)
            : 0m;

        var latePercent = totalRecords > 0
            ? Math.Round(((decimal)lateCount / totalRecords) * 100, 2)
            : 0m;

        var absencePercent = totalRecords > 0
            ? Math.Round(((decimal)absentCount / totalRecords) * 100, 2)
            : 0m;

        return new AttendanceMetricsDto(
            OverallAttendancePercent: attendancePercent,
            AverageLateArrivalsPercent: latePercent,
            AverageAbsencePercent: absencePercent,
            TotalPresentDays: presentCount,
            TotalAbsentDays: absentCount,
            TotalLateDays: lateCount);
    }

    private async Task<LeaveMetricsDto> GetLeaveMetricsAsync(
        DateTime fromDate,
        DateTime toDate,
        DefaultIdType? departmentId,
        CancellationToken cancellationToken)
    {
        var requests = await leaveRequestRepository.ListAsync(
            new LeaveRequestsByDateRangeSpec(fromDate, toDate),
            cancellationToken)
            .ConfigureAwait(false);

        var leaveTypes = await leaveTypeRepository.ListAsync(cancellationToken).ConfigureAwait(false);

        var pendingCount = requests.Count(x => x.Status == "Pending");
        var approvedCount = requests.Count(x => x.Status == "Approved");
        var rejectedCount = requests.Count(x => x.Status == "Rejected");

        var totalConsumed = requests
            .Where(x => x.Status == "Approved")
            .Sum(x => x.NumberOfDays);

        var uniqueEmployees = requests.Select(x => x.EmployeeId).Distinct().Count();
        var averageLeavePerEmployee = uniqueEmployees > 0
            ? Math.Round(totalConsumed / uniqueEmployees, 2)
            : 0m;

        var breakdown = leaveTypes.Select(lt => new LeaveTypeBreakdownDto(
            LeaveTypeName: lt.Name,
            RequestCount: requests.Count(x => x.LeaveTypeId == lt.Id),
            ConsumedDays: requests.Where(x => x.LeaveTypeId == lt.Id && x.Status == "Approved").Sum(x => x.NumberOfDays),
            ApprovalRate: CalculateApprovalRate(requests.Where(x => x.LeaveTypeId == lt.Id).ToList())
        )).ToList();

        return new LeaveMetricsDto(
            PendingLeaveRequests: pendingCount,
            ApprovedLeaveRequests: approvedCount,
            RejectedLeaveRequests: rejectedCount,
            AverageLeaveConsumedPerEmployee: averageLeavePerEmployee,
            TotalLeaveRequestsThisPeriod: requests.Count,
            LeaveTypeBreakdown: breakdown);
    }

    private async Task<PayrollMetricsDto> GetPayrollMetricsAsync(
        DateTime fromDate,
        DateTime toDate,
        DefaultIdType? departmentId,
        CancellationToken cancellationToken)
    {
        var payrolls = await payrollRepository.ListAsync(
            new PayrollsByDateRangeSpec(fromDate, toDate),
            cancellationToken)
            .ConfigureAwait(false);

        var totalGross = payrolls.Sum(x => x.TotalGrossPay);
        var totalNet = payrolls.Sum(x => x.TotalNetPay);
        var totalDeductions = payrolls.Sum(x => x.TotalDeductions);
        var totalTax = payrolls.Sum(x => x.TotalTaxes);
        var employeeCount = payrolls.Select(x => x.EmployeeCount).Distinct().Count();

        var averageSalary = employeeCount > 0
            ? Math.Round(totalNet / employeeCount, 2)
            : 0m;

        var pendingCount = payrolls.Count(x => x.Status == "Draft" || x.Status == "Processing");

        return new PayrollMetricsDto(
            TotalGrossSalary: totalGross,
            TotalNetSalary: totalNet,
            TotalDeductions: totalDeductions,
            TotalTax: totalTax,
            AverageSalaryPerEmployee: averageSalary,
            TotalPayrollRuns: payrolls.Count,
            PendingPayrolls: pendingCount);
    }

    private async Task<PerformanceMetricsDto> GetPerformanceMetricsAsync(
        DefaultIdType? departmentId,
        CancellationToken cancellationToken)
    {
        var reviews = await performanceReviewRepository.ListAsync(cancellationToken).ConfigureAwait(false);

        var completedCount = reviews.Count(x => x.Status == "Completed");
        var pendingCount = reviews.Count(x => x.Status == "Pending");
        var averageRating = reviews.Any()
            ? Math.Round(reviews.Average(x => x.OverallRating), 2)
            : 0m;

        var employees = await employeeRepository.ListAsync(cancellationToken).ConfigureAwait(false);
        var employeesWithoutReviews = employees.Count(e => !reviews.Any(r => r.EmployeeId == e.Id));

        var aboveTarget = reviews.Count(x => x.OverallRating >= 4);
        var belowTarget = reviews.Count(x => x.OverallRating < 3);

        return new PerformanceMetricsDto(
            CompletedReviews: completedCount,
            PendingReviews: pendingCount,
            AverageRating: averageRating,
            EmployeesWithoutReviews: employeesWithoutReviews,
            EmployeesAboveTarget: aboveTarget,
            EmployeesBelowTarget: belowTarget);
    }

    private async Task<TurnoverMetricsDto> GetTurnoverMetricsAsync(
        DateTime fromDate,
        DateTime toDate,
        DefaultIdType? departmentId,
        CancellationToken cancellationToken)
    {
        var employees = await employeeRepository.ListAsync(cancellationToken).ConfigureAwait(false);

        var terminated = employees.Count(e => e.Status == "Terminated" && e.LastModifiedOn.DateTime >= fromDate);
        var newHires = employees.Count(e => e.HireDate.HasValue && e.HireDate.Value >= fromDate && e.HireDate.Value <= toDate);

        var activeCount = employees.Count(e => e.Status == "Active" && e.IsActive);
        var turnoverRate = activeCount > 0
            ? Math.Round(((decimal)terminated / activeCount) * 100, 2)
            : 0m;

        return new TurnoverMetricsDto(
            AnnualTurnoverRate: turnoverRate,
            EmployeesTerminatedThisPeriod: terminated,
            NewHiresThisPeriod: newHires,
            NetHeadcountChange: newHires - terminated,
            TurnoverByDepartment: new List<TurnoverByDepartmentDto>());
    }

    private async Task<DepartmentMetricsDto> GetDepartmentMetricsAsync(
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken)
    {
        var employees = await employeeRepository.ListAsync(cancellationToken).ConfigureAwait(false);

        var departmentGroups = employees
            .GroupBy(e => e.OrganizationalUnitId)
            .Select(g => new DepartmentBreakdownDto(
                DepartmentId: g.Key,
                DepartmentName: g.FirstOrDefault()?.OrganizationalUnit?.Name ?? "Unknown",
                HeadCount: g.Count(), // invoke Count() to get int value
                AverageAttendancePercent: 0m,
                AverageRating: 0m,
                TotalSalaryExpense: 0m))
            .ToList();

        var largestDept = departmentGroups.OrderByDescending(d => d.HeadCount).FirstOrDefault()?.DepartmentName ?? "N/A";
        var smallestDept = departmentGroups.OrderBy(d => d.HeadCount).FirstOrDefault()?.DepartmentName ?? "N/A";

        return new DepartmentMetricsDto(
            Departments: departmentGroups,
            TotalDepartments: departmentGroups.Count,
            LargestDepartment: largestDept,
            SmallestDepartment: smallestDept);
    }

    private async Task<HrTrendsDto> GetHrTrendsAsync(
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken)
    {
        var employees = await employeeRepository.ListAsync(cancellationToken).ConfigureAwait(false);
        var attendance = await attendanceRepository.ListAsync(
            new AttendanceByDateRangeSpec(fromDate, toDate),
            cancellationToken)
            .ConfigureAwait(false);

        var headcountTrend = CalculateHeadcountTrend(employees, fromDate, toDate);
        var attendanceTrend = CalculateAttendanceTrend(attendance, fromDate, toDate);
        var turnoverTrend = CalculateTurnoverTrend(employees, fromDate, toDate);

        return new HrTrendsDto(
            HeadcountTrend: headcountTrend,
            AttendanceTrend: attendanceTrend,
            TurnoverTrend: turnoverTrend);
    }

    private async Task<ComplianceMetricsDto> GetComplianceMetricsAsync(
        DefaultIdType? departmentId,
        CancellationToken cancellationToken)
    {
        var employees = await employeeRepository.ListAsync(cancellationToken).ConfigureAwait(false);

        var totalEmployees = employees.Count;
        var employeesWithCompleteData = employees.Count(e => !string.IsNullOrEmpty(e.Email) && !string.IsNullOrEmpty(e.Tin));

        var completionPercent = totalEmployees > 0
            ? Math.Round(((decimal)employeesWithCompleteData / totalEmployees) * 100, 2)
            : 0m;

        return new ComplianceMetricsDto(
            DocumentUploadCompletionPercent: completionPercent,
            EmployeesWithCompleteDocuments: employeesWithCompleteData,
            EmployeesWithMissingDocuments: totalEmployees - employeesWithCompleteData,
            BenefitEnrollmentRate: 85m,
            TaxCompliancePercent: 95m);
    }

    private static decimal CalculateApprovalRate(List<LeaveRequest> requests)
    {
        if (!requests.Any())
            return 0m;

        var approved = requests.Count(x => x.Status == "Approved");
        return Math.Round(((decimal)approved / requests.Count) * 100, 2);
    }

    private static List<MonthlyTrendDto> CalculateHeadcountTrend(
        List<Employee> employees,
        DateTime fromDate,
        DateTime toDate)
    {
        var trends = new List<MonthlyTrendDto>();
        var current = new DateTime(fromDate.Year, fromDate.Month, 1);

        while (current <= toDate)
        {
            var monthEnd = current.AddMonths(1).AddDays(-1);
            var headcount = employees.Count(e => e.HireDate.HasValue && e.HireDate.Value <= monthEnd.Date && 
                (e.Status != "Terminated" || e.LastModifiedOn.DateTime > monthEnd));
            trends.Add(new MonthlyTrendDto(Month: current, Value: headcount));
            current = current.AddMonths(1);
        }

        return trends;
    }

    private static List<MonthlyTrendDto> CalculateAttendanceTrend(
        List<Attendance> attendance,
        DateTime fromDate,
        DateTime toDate)
    {
        var trends = new List<MonthlyTrendDto>();
        var current = new DateTime(fromDate.Year, fromDate.Month, 1);

        while (current <= toDate)
        {
            var monthEnd = current.AddMonths(1).AddDays(-1);
            var monthRecords = attendance
                .Where(x => x.AttendanceDate.Date >= current.Date && x.AttendanceDate.Date <= monthEnd.Date)
                .ToList();

            var percentage = monthRecords.Any()
                ? Math.Round(((decimal)monthRecords.Count(x => x.Status == "Present") / monthRecords.Count) * 100, 2)
                : 0m;

            trends.Add(new MonthlyTrendDto(Month: current, Value: percentage));
            current = current.AddMonths(1);
        }

        return trends;
    }

    private static List<MonthlyTrendDto> CalculateTurnoverTrend(
        List<Employee> employees,
        DateTime fromDate,
        DateTime toDate)
    {
        var trends = new List<MonthlyTrendDto>();
        var current = new DateTime(fromDate.Year, fromDate.Month, 1);

        while (current <= toDate)
        {
            var monthEnd = current.AddMonths(1).AddDays(-1);
            var terminated = employees.Count(e => e.Status == "Terminated" && 
                e.LastModifiedOn.DateTime >= current && e.LastModifiedOn.DateTime <= monthEnd);

            trends.Add(new MonthlyTrendDto(Month: current, Value: terminated));
            current = current.AddMonths(1);
        }

        return trends;
    }
}

/// <summary>
/// Specification for attendance by date range.
/// </summary>
public sealed class AttendanceByDateRangeSpec : Specification<Attendance>
{
    public AttendanceByDateRangeSpec(DateTime fromDate, DateTime toDate)
    {
        Query.Where(x => x.AttendanceDate >= fromDate && x.AttendanceDate <= toDate)
            .OrderByDescending(x => x.AttendanceDate);
    }
}

/// <summary>
/// Specification for leave requests by date range.
/// </summary>
public sealed class LeaveRequestsByDateRangeSpec : Specification<LeaveRequest>
{
    public LeaveRequestsByDateRangeSpec(DateTime fromDate, DateTime toDate)
    {
        Query.Where(x => x.StartDate >= fromDate && x.EndDate <= toDate)
            .OrderByDescending(x => x.StartDate);
    }
}

/// <summary>
/// Specification for payrolls by date range.
/// </summary>
public sealed class PayrollsByDateRangeSpec : Specification<Payroll>
{
    public PayrollsByDateRangeSpec(DateTime fromDate, DateTime toDate)
    {
        Query.Where(x => x.EndDate >= fromDate && x.StartDate <= toDate)
            .OrderByDescending(x => x.EndDate);
    }
}
