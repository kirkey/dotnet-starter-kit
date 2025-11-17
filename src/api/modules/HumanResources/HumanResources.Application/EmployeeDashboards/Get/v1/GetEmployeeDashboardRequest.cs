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
public sealed record PersonalSummaryDto(
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
public sealed record LeaveMetricsDto(
    decimal TotalEntitlement,
    decimal TakenDays,
    decimal PendingDays,
    decimal AvailableDays,
    List<LeaveBalanceItemDto> BalancesByType);

/// <summary>
/// Leave balance item for specific leave type.
/// </summary>
public sealed record LeaveBalanceItemDto(
    DefaultIdType LeaveTypeId,
    string LeaveTypeName,
    decimal Entitlement,
    decimal Taken,
    decimal Pending,
    decimal Available);

/// <summary>
/// Attendance metrics section.
/// </summary>
public sealed record AttendanceMetricsDto(
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
public sealed record PayrollSnapshotDto(
    decimal LastSalary,
    DateTime? LastPayrollDate,
    DateTime? NextPayrollDate,
    string? LastPayrollStatus);

/// <summary>
/// Pending approvals section.
/// </summary>
public sealed record PendingApprovalsDto(
    int PendingLeaveRequests,
    int PendingTimesheets,
    int PendingPerformanceReviews,
    List<PendingItemDto> RecentPending);

/// <summary>
/// Individual pending item.
/// </summary>
public sealed record PendingItemDto(
    DefaultIdType ItemId,
    string ItemType, // "LeaveRequest", "Timesheet", "PerformanceReview"
    string Description,
    DateTime SubmittedDate,
    string Status);

/// <summary>
/// Performance review section.
/// </summary>
public sealed record PerformanceSnapshotDto(
    int PendingReviews,
    int AcknowledgedReviews,
    List<RecentReviewDto> RecentReviews);

/// <summary>
/// Recent performance review item.
/// </summary>
public sealed record RecentReviewDto(
    DefaultIdType ReviewId,
    string ReviewerName,
    DateTime ReviewDate,
    decimal? OverallRating,
    string Status); // Pending, Completed, Acknowledged

/// <summary>
/// Upcoming schedule section.
/// </summary>
public sealed record UpcomingScheduleDto(
    List<UpcomingShiftDto> UpcomingShifts,
    List<HolidayDto> UpcomingHolidays);

/// <summary>
/// Upcoming shift item.
/// </summary>
public sealed record UpcomingShiftDto(
    DefaultIdType ShiftAssignmentId,
    string ShiftName,
    DateTime ShiftDate,
    TimeSpan StartTime,
    TimeSpan EndTime);

/// <summary>
/// Holiday item.
/// </summary>
public sealed record HolidayDto(
    DefaultIdType HolidayId,
    string HolidayName,
    DateTime HolidayDate,
    bool IsNationalHoliday);

/// <summary>
/// Quick actions available for employee.
/// </summary>
public sealed record QuickActionsDto(
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
    PersonalSummaryDto PersonalSummary,
    LeaveMetricsDto LeaveMetrics,
    AttendanceMetricsDto AttendanceMetrics,
    PayrollSnapshotDto PayrollSnapshot,
    PendingApprovalsDto PendingApprovals,
    PerformanceSnapshotDto PerformanceSnapshot,
    UpcomingScheduleDto UpcomingSchedule,
    QuickActionsDto QuickActions,
    DateTime GeneratedAt);

