namespace FSH.Starter.WebApi.HumanResources.Application.HRAnalytics.Get.v1;

/// <summary>
/// Query to retrieve HR analytics metrics and KPIs.
/// Aggregates HR data for dashboard and reporting purposes.
/// </summary>
public sealed record GetHrAnalyticsRequest(
    [property: DefaultValue(null)] DateTime? FromDate = null,
    [property: DefaultValue(null)] DateTime? ToDate = null,
    [property: DefaultValue(null)] DefaultIdType? DepartmentId = null) 
    : IRequest<HrAnalyticsResponse>;

/// <summary>
/// HR headcount metrics section.
/// </summary>
public sealed record HeadcountMetricsDto(
    int TotalEmployees,
    int ActiveEmployees,
    int TerminatedEmployees,
    int OnLeaveEmployees,
    decimal HeadcountGrowthPercent);

/// <summary>
/// HR attendance metrics section.
/// </summary>
public sealed record AttendanceMetricsDto(
    decimal OverallAttendancePercent,
    decimal AverageLateArrivalsPercent,
    decimal AverageAbsencePercent,
    int TotalPresentDays,
    int TotalAbsentDays,
    int TotalLateDays);

/// <summary>
/// HR leave metrics section.
/// </summary>
public sealed record LeaveMetricsDto(
    int PendingLeaveRequests,
    int ApprovedLeaveRequests,
    int RejectedLeaveRequests,
    decimal AverageLeaveConsumedPerEmployee,
    int TotalLeaveRequestsThisPeriod,
    List<LeaveTypeBreakdownDto> LeaveTypeBreakdown);

/// <summary>
/// Leave type breakdown item.
/// </summary>
public sealed record LeaveTypeBreakdownDto(
    string LeaveTypeName,
    int RequestCount,
    decimal ConsumedDays,
    decimal ApprovalRate);

/// <summary>
/// HR payroll metrics section.
/// </summary>
public sealed record PayrollMetricsDto(
    decimal TotalGrossSalary,
    decimal TotalNetSalary,
    decimal TotalDeductions,
    decimal TotalTax,
    decimal AverageSalaryPerEmployee,
    int TotalPayrollRuns,
    int PendingPayrolls);

/// <summary>
/// HR performance metrics section.
/// </summary>
public sealed record PerformanceMetricsDto(
    int CompletedReviews,
    int PendingReviews,
    decimal AverageRating,
    int EmployeesWithoutReviews,
    int EmployeesAboveTarget,
    int EmployeesBelowTarget);

/// <summary>
/// HR turnover metrics section.
/// </summary>
public sealed record TurnoverMetricsDto(
    decimal AnnualTurnoverRate,
    int EmployeesTerminatedThisPeriod,
    int NewHiresThisPeriod,
    decimal NetHeadcountChange,
    List<TurnoverByDepartmentDto> TurnoverByDepartment);

/// <summary>
/// Turnover by department breakdown.
/// </summary>
public sealed record TurnoverByDepartmentDto(
    string DepartmentName,
    int Terminated,
    int NewHires,
    decimal TurnoverRate);

/// <summary>
/// HR departmental metrics section.
/// </summary>
public sealed record DepartmentMetricsDto(
    List<DepartmentBreakdownDto> Departments,
    int TotalDepartments,
    string LargestDepartment,
    string SmallestDepartment);

/// <summary>
/// Department breakdown item.
/// </summary>
public sealed record DepartmentBreakdownDto(
    DefaultIdType DepartmentId,
    string DepartmentName,
    int HeadCount,
    decimal AverageAttendancePercent,
    decimal AverageRating,
    decimal TotalSalaryExpense);

/// <summary>
/// HR trends data section.
/// </summary>
public sealed record HrTrendsDto(
    List<MonthlyTrendDto> HeadcountTrend,
    List<MonthlyTrendDto> AttendanceTrend,
    List<MonthlyTrendDto> TurnoverTrend);

/// <summary>
/// Monthly trend data point.
/// </summary>
public sealed record MonthlyTrendDto(
    DateTime Month,
    decimal Value);

/// <summary>
/// HR compliance and audit metrics section.
/// </summary>
public sealed record ComplianceMetricsDto(
    decimal DocumentUploadCompletionPercent,
    int EmployeesWithCompleteDocuments,
    int EmployeesWithMissingDocuments,
    decimal BenefitEnrollmentRate,
    decimal TaxCompliancePercent);

/// <summary>
/// Complete HR analytics response.
/// </summary>
public sealed record HrAnalyticsResponse(
    DateTime ReportDate,
    DateTime? PeriodStart,
    DateTime? PeriodEnd,
    HeadcountMetricsDto HeadcountMetrics,
    AttendanceMetricsDto AttendanceMetrics,
    LeaveMetricsDto LeaveMetrics,
    PayrollMetricsDto PayrollMetrics,
    PerformanceMetricsDto PerformanceMetrics,
    TurnoverMetricsDto TurnoverMetrics,
    DepartmentMetricsDto DepartmentMetrics,
    HrTrendsDto Trends,
    ComplianceMetricsDto ComplianceMetrics,
    DateTime GeneratedAt);

