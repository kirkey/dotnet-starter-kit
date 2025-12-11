namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDashboards.Get.v1;

/// <summary>
/// Query to retrieve employee dashboard data aggregation.
/// Includes leave balances, attendance metrics, payroll info, pending approvals, and more.
/// </summary>
public sealed record GetEmployeeDashboardRequest(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId) 
    : IRequest<EmployeeDashboardResponse>;

/// <summary>
/// Personal information summary section.
/// </summary>
public sealed record EmployeePersonalSummaryDto(
    DefaultIdType EmployeeId,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string? Designation,
    string? Department,
    string? ProfilePhotoUrl,
    DateTime JoinDate,
    string? EmploymentStatus);

/// <summary>
/// Leave metrics and balances section.
/// </summary>
public sealed record EmployeeLeaveMetricsDto(
    decimal TotalEntitlement,
    decimal TakenDays,
    decimal PendingDays,
    decimal AvailableDays,
    List<EmployeeLeaveBalanceItemDto> BalancesByType);

/// <summary>
/// Leave balance item for specific leave type.
/// </summary>
public sealed record EmployeeLeaveBalanceItemDto(
    DefaultIdType LeaveTypeId,
    string LeaveTypeName,
    decimal Entitlement,
    decimal Taken,
    decimal Pending,
    decimal Available);

/// <summary>
/// Attendance metrics section.
/// </summary>
public sealed record EmployeeAttendanceMetricsDto(
    int WorkingDaysThisMonth,
    int PresentDaysThisMonth,
    int AbsentDaysThisMonth,
    int LateArrivalsThisMonth,
    decimal AttendancePercentageThisMonth,
    int WorkingDaysThisYear,
    int PresentDaysThisYear,
    decimal AttendancePercentageThisYear);

/// <summary>
/// Payroll information snapshot.
/// </summary>
public sealed record EmployeePayrollSnapshotDto(
    decimal LastSalary,
    DateTime? LastPayrollDate,
    DateTime? NextPayrollDate,
    string? LastPayrollStatus);

/// <summary>
/// Pending approvals section.
/// </summary>
public sealed record EmployeePendingApprovalsDto(
    int PendingLeaveRequests,
    int PendingTimesheets,
    int PendingPerformanceReviews,
    List<EmployeePendingItemDto> RecentPending);

/// <summary>
/// Individual pending item.
/// </summary>
public sealed record EmployeePendingItemDto(
    DefaultIdType ItemId,
    string ItemType, // "LeaveRequest", "Timesheet", "PerformanceReview"
    string Description,
    DateTime SubmittedDate,
    string Status);

/// <summary>
/// Performance review section.
/// </summary>
public sealed record EmployeePerformanceSnapshotDto(
    int PendingReviews,
    int AcknowledgedReviews,
    List<EmployeeRecentReviewDto> RecentReviews);

/// <summary>
/// Recent performance review item.
/// </summary>
public sealed record EmployeeRecentReviewDto(
    DefaultIdType ReviewId,
    string ReviewerName,
    DateTime ReviewDate,
    decimal? OverallRating,
    string Status); // Pending, Completed, Acknowledged

/// <summary>
/// Upcoming schedule section.
/// </summary>
public sealed record EmployeeUpcomingScheduleDto(
    List<EmployeeUpcomingShiftDto> UpcomingShifts,
    List<EmployeeHolidayDto> UpcomingHolidays);

/// <summary>
/// Upcoming shift item.
/// </summary>
public sealed record EmployeeUpcomingShiftDto(
    DefaultIdType ShiftAssignmentId,
    string ShiftName,
    DateTime ShiftDate,
    TimeSpan StartTime,
    TimeSpan EndTime);

/// <summary>
/// Holiday item.
/// </summary>
public sealed record EmployeeHolidayDto(
    DefaultIdType HolidayId,
    string HolidayName,
    DateTime HolidayDate,
    bool IsNationalHoliday);

/// <summary>
/// Quick actions available for employee.
/// </summary>
public sealed record EmployeeQuickActionsDto(
    bool CanSubmitLeave,
    bool CanClockIn,
    bool CanClockOut,
    bool CanUploadDocument,
    bool CanSubmitTimesheet,
    string? NextActionMessage);

/// <summary>
/// Complete employee dashboard response.
/// </summary>
public sealed record EmployeeDashboardResponse(
    DefaultIdType EmployeeId,
    EmployeePersonalSummaryDto PersonalSummary,
    EmployeeLeaveMetricsDto LeaveMetrics,
    EmployeeAttendanceMetricsDto AttendanceMetrics,
    EmployeePayrollSnapshotDto PayrollSnapshot,
    EmployeePendingApprovalsDto PendingApprovals,
    EmployeePerformanceSnapshotDto PerformanceSnapshot,
    EmployeeUpcomingScheduleDto UpcomingSchedule,
    EmployeeQuickActionsDto QuickActions,
    DateTime GeneratedAt);

