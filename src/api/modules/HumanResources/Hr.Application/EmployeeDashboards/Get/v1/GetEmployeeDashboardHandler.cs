using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDashboards.Get.v1;

/// <summary>
/// Handler for retrieving employee dashboard data.
/// Aggregates data from multiple HR entities into a single dashboard view.
/// </summary>
public sealed class GetEmployeeDashboardHandler(
    ILogger<GetEmployeeDashboardHandler> logger,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:leavebalances")] IReadRepository<LeaveBalance> leaveBalanceRepository,
    [FromKeyedServices("hr:attendance")] IReadRepository<Attendance> attendanceRepository,
    [FromKeyedServices("hr:payrolls")] IReadRepository<Payroll> payrollRepository,
    [FromKeyedServices("hr:leaverequests")] IReadRepository<LeaveRequest> leaveRequestRepository,
    [FromKeyedServices("hr:timesheets")] IReadRepository<Timesheet> timesheetRepository,
    [FromKeyedServices("hr:performancereviews")] IReadRepository<PerformanceReview> performanceReviewRepository,
    [FromKeyedServices("hr:shiftassignments")] IReadRepository<ShiftAssignment> shiftAssignmentRepository,
    [FromKeyedServices("hr:holidays")] IReadRepository<Holiday> holidayRepository)
    : IRequestHandler<GetEmployeeDashboardRequest, EmployeeDashboardResponse>
{
    /// <summary>
    /// Handles the get employee dashboard request.
    /// </summary>
    public async Task<EmployeeDashboardResponse> Handle(
        GetEmployeeDashboardRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Generating dashboard for employee: {EmployeeId}", request.EmployeeId);

        // Fetch all data in parallel
        var personalTask = GetPersonalSummaryAsync(request.EmployeeId, cancellationToken);
        var leaveTask = GetLeaveMetricsAsync(request.EmployeeId, cancellationToken);
        var attendanceTask = GetAttendanceMetricsAsync(request.EmployeeId, cancellationToken);
        var payrollTask = GetPayrollSnapshotAsync(request.EmployeeId, cancellationToken);
        var pendingApprovalsTask = GetPendingApprovalsAsync(request.EmployeeId, cancellationToken);
        var performanceTask = GetPerformanceSnapshotAsync(request.EmployeeId, cancellationToken);
        var scheduleTask = GetUpcomingScheduleAsync(request.EmployeeId, cancellationToken);
        var quickActionsTask = GetQuickActionsAsync(request.EmployeeId, cancellationToken);

        await Task.WhenAll(
            personalTask, leaveTask, attendanceTask, payrollTask,
            pendingApprovalsTask, performanceTask, scheduleTask, quickActionsTask)
            .ConfigureAwait(false);

        var response = new EmployeeDashboardResponse(
            EmployeeId: request.EmployeeId,
            PersonalSummary: await personalTask,
            LeaveMetrics: await leaveTask,
            AttendanceMetrics: await attendanceTask,
            PayrollSnapshot: await payrollTask,
            PendingApprovals: await pendingApprovalsTask,
            PerformanceSnapshot: await performanceTask,
            UpcomingSchedule: await scheduleTask,
            QuickActions: await quickActionsTask,
            GeneratedAt: DateTime.UtcNow);

        logger.LogInformation("Dashboard generated successfully for employee: {EmployeeId}", request.EmployeeId);

        return response;
    }

    private async Task<EmployeePersonalSummaryDto> GetPersonalSummaryAsync(
        DefaultIdType employeeId,
        CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(employeeId, cancellationToken)
            ?? throw new NotFoundException($"Employee with ID {employeeId} not found");

        return new EmployeePersonalSummaryDto(
            EmployeeId: employee.Id,
            FirstName: employee.FirstName,
            LastName: employee.LastName,
            Email: employee.Email ?? string.Empty,
            PhoneNumber: employee.PhoneNumber,
            Designation: employee.DesignationAssignments?.FirstOrDefault()?.Designation?.Name,
            Department: employee.OrganizationalUnit?.Name,
            ProfilePhotoUrl: null,
            JoinDate: employee.HireDate ?? DateTime.MinValue,
            EmploymentStatus: employee.Status);
    }

    private async Task<EmployeeLeaveMetricsDto> GetLeaveMetricsAsync(
        DefaultIdType employeeId,
        CancellationToken cancellationToken)
    {
        var currentYear = DateTime.UtcNow.Year;
        var balances = await leaveBalanceRepository.ListAsync(
            new LeaveBalancesByEmployeeAndYearSpec(employeeId, currentYear),
            cancellationToken)
            .ConfigureAwait(false);

        var totalEntitlement = balances.Sum(x => x.AvailableDays);
        var takenDays = balances.Sum(x => x.TakenDays);
        var pendingDays = balances.Sum(x => x.PendingDays);
        var availableDays = balances.Sum(x => x.RemainingDays);

        var balanceItems = balances.Select(x => new EmployeeLeaveBalanceItemDto(
            LeaveTypeId: x.LeaveTypeId,
            LeaveTypeName: x.LeaveType?.Name ?? "Unknown",
            Entitlement: x.AvailableDays,
            Taken: x.TakenDays,
            Pending: x.PendingDays,
            Available: x.RemainingDays)).ToList();

        return new EmployeeLeaveMetricsDto(
            TotalEntitlement: totalEntitlement,
            TakenDays: takenDays,
            PendingDays: pendingDays,
            AvailableDays: availableDays,
            BalancesByType: balanceItems);
    }

    private async Task<EmployeeAttendanceMetricsDto> GetAttendanceMetricsAsync(
        DefaultIdType employeeId,
        CancellationToken cancellationToken)
    {
        var currentDate = DateTime.UtcNow;
        var monthStart = new DateTime(currentDate.Year, currentDate.Month, 1);
        var yearStart = new DateTime(currentDate.Year, 1, 1);

        var attendanceRecords = await attendanceRepository.ListAsync(
            new AttendanceByEmployeeAndDateRangeSpec(employeeId, yearStart, currentDate),
            cancellationToken)
            .ConfigureAwait(false);

        var thisMonthRecords = attendanceRecords
            .Where(x => x.AttendanceDate >= monthStart && x.AttendanceDate <= currentDate)
            .ToList();

        var thisYearRecords = attendanceRecords;

        var monthPresent = thisMonthRecords.Count(x => x.Status == "Present");
        var monthAbsent = thisMonthRecords.Count(x => x.Status == "Absent");
        var monthLate = thisMonthRecords.Count(x => x.Status == "Late");
        var monthWorkingDays = thisMonthRecords.Count;

        var yearPresent = thisYearRecords.Count(x => x.Status == "Present");
        var yearWorkingDays = thisYearRecords.Count;

        var monthAttendancePercentage = monthWorkingDays > 0
            ? Math.Round((decimal)monthPresent / monthWorkingDays * 100, 2)
            : 0m;

        var yearAttendancePercentage = yearWorkingDays > 0
            ? Math.Round((decimal)yearPresent / yearWorkingDays * 100, 2)
            : 0m;

        return new EmployeeAttendanceMetricsDto(
            WorkingDaysThisMonth: monthWorkingDays,
            PresentDaysThisMonth: monthPresent,
            AbsentDaysThisMonth: monthAbsent,
            LateArrivalsThisMonth: monthLate,
            AttendancePercentageThisMonth: monthAttendancePercentage,
            WorkingDaysThisYear: yearWorkingDays,
            PresentDaysThisYear: yearPresent,
            AttendancePercentageThisYear: yearAttendancePercentage);
    }

    private async Task<EmployeePayrollSnapshotDto> GetPayrollSnapshotAsync(
        DefaultIdType employeeId,
        CancellationToken cancellationToken)
    {
        var payrolls = await payrollRepository.ListAsync(
            new PayrollsByEmployeeSpec(employeeId),
            cancellationToken)
            .ConfigureAwait(false);

        var lastPayroll = payrolls.FirstOrDefault();

        return new EmployeePayrollSnapshotDto(
            LastSalary: lastPayroll?.TotalNetPay ?? 0m,
            LastPayrollDate: lastPayroll?.ProcessedDate,
            NextPayrollDate: CalculateNextPayrollDate(),
            LastPayrollStatus: lastPayroll?.Status);
    }

    private async Task<EmployeePendingApprovalsDto> GetPendingApprovalsAsync(
        DefaultIdType employeeId,
        CancellationToken cancellationToken)
    {
        var leaveRequests = await leaveRequestRepository.ListAsync(
            new PendingLeaveRequestsSpec(employeeId),
            cancellationToken)
            .ConfigureAwait(false);

        var timesheets = await timesheetRepository.ListAsync(
            new PendingTimesheetsSpec(employeeId),
            cancellationToken)
            .ConfigureAwait(false);

        var performanceReviews = await performanceReviewRepository.ListAsync(
            new PendingPerformanceReviewsSpec(employeeId),
            cancellationToken)
            .ConfigureAwait(false);

        var recentPending = new List<EmployeePendingItemDto>();

        recentPending.AddRange(leaveRequests.Take(3).Select(x => new EmployeePendingItemDto(
            ItemId: x.Id,
            ItemType: "LeaveRequest",
            Description: $"{x.LeaveType?.Name} from {x.StartDate:MMM dd} to {x.EndDate:MMM dd}",
            SubmittedDate: (x.SubmittedDate ?? x.CreatedOn).DateTime,
            Status: x.Status)));

        recentPending.AddRange(timesheets.Take(2).Select(x => new EmployeePendingItemDto(
            ItemId: x.Id,
            ItemType: "Timesheet",
            Description: $"Timesheet for {x.PeriodType} ({x.StartDate:MMM dd} - {x.EndDate:MMM dd})",
            SubmittedDate: x.CreatedOn.DateTime,
            Status: x.Status)));

        recentPending.AddRange(performanceReviews.Take(1).Select(x => new EmployeePendingItemDto(
            ItemId: x.Id,
            ItemType: "PerformanceReview",
            Description: $"Review by {x.Reviewer?.FirstName} {x.Reviewer?.LastName}",
            SubmittedDate: x.ReviewPeriodEnd,
            Status: x.Status)));

        return new EmployeePendingApprovalsDto(
            PendingLeaveRequests: leaveRequests.Count(x => x.Status == "Pending"),
            PendingTimesheets: timesheets.Count(x => x.Status == "Pending"),
            PendingPerformanceReviews: performanceReviews.Count(x => x.Status == "Pending"),
            RecentPending: recentPending.OrderByDescending(x => x.SubmittedDate).Take(5).ToList());
    }

    private async Task<EmployeePerformanceSnapshotDto> GetPerformanceSnapshotAsync(
        DefaultIdType employeeId,
        CancellationToken cancellationToken)
    {
        var reviews = await performanceReviewRepository.ListAsync(
            new PerformanceReviewsByEmployeeSpec(employeeId),
            cancellationToken)
            .ConfigureAwait(false);

        var recentReviews = reviews.Take(3).Select(x => new EmployeeRecentReviewDto(
            ReviewId: x.Id,
            ReviewerName: $"{x.Reviewer?.FirstName} {x.Reviewer?.LastName}",
            ReviewDate: x.ReviewPeriodEnd,
            OverallRating: x.OverallRating,
            Status: x.Status)).ToList();

        return new EmployeePerformanceSnapshotDto(
            PendingReviews: reviews.Count(x => x.Status == "Pending"),
            AcknowledgedReviews: reviews.Count(x => x.Status == "Acknowledged"),
            RecentReviews: recentReviews);
    }

    private async Task<EmployeeUpcomingScheduleDto> GetUpcomingScheduleAsync(
        DefaultIdType employeeId,
        CancellationToken cancellationToken)
    {
        var currentDate = DateTime.UtcNow;
        var upcomingShifts = await shiftAssignmentRepository.ListAsync(
            new UpcomingShiftAssignmentsSpec(employeeId, currentDate),
            cancellationToken)
            .ConfigureAwait(false);

        var holidays = await holidayRepository.ListAsync(
            new UpcomingHolidaysSpec(currentDate),
            cancellationToken)
            .ConfigureAwait(false);

        var upcomingShiftDtos = upcomingShifts.Take(5).Select(x => new EmployeeUpcomingShiftDto(
            ShiftAssignmentId: x.Id,
            ShiftName: x.Shift?.Name ?? "Shift",
            ShiftDate: x.StartDate,
            StartTime: x.Shift?.StartTime ?? TimeSpan.Zero,
            EndTime: x.Shift?.EndTime ?? TimeSpan.Zero)).ToList();

        var holidayDtos = holidays.Take(5).Select(x => new EmployeeHolidayDto(
            HolidayId: x.Id,
            HolidayName: x.HolidayName,
            HolidayDate: x.HolidayDate,
            IsNationalHoliday: x.Type == "RegularPublicHoliday")).ToList();

        return new EmployeeUpcomingScheduleDto(
            UpcomingShifts: upcomingShiftDtos,
            UpcomingHolidays: holidayDtos);
    }

    private async Task<EmployeeQuickActionsDto> GetQuickActionsAsync(
        DefaultIdType employeeId,
        CancellationToken cancellationToken)
    {
        // Simplified quick actions - all true for now
        // In production, add permission and state checks
        return new EmployeeQuickActionsDto(
            CanSubmitLeave: true,
            CanClockIn: true,
            CanClockOut: true,
            CanUploadDocument: true,
            CanSubmitTimesheet: true,
            NextActionMessage: "You have 15 available leave days this year");
    }

    private static DateTime CalculateNextPayrollDate()
    {
        var currentDate = DateTime.UtcNow;
        // Assuming monthly payroll on the 25th
        var nextPayrollDay = new DateTime(currentDate.Year, currentDate.Month, 25);
        if (nextPayrollDay <= currentDate)
        {
            nextPayrollDay = nextPayrollDay.AddMonths(1);
        }
        return nextPayrollDay;
    }
}

/// <summary>
/// Specification for leave balances by employee and year.
/// </summary>
public sealed class LeaveBalancesByEmployeeAndYearSpec : Specification<LeaveBalance>
{
    public LeaveBalancesByEmployeeAndYearSpec(DefaultIdType employeeId, int year)
    {
        Query.Where(x => x.EmployeeId == employeeId && x.Year == year);
    }
}

/// <summary>
/// Specification for attendance by employee and date range.
/// </summary>
public sealed class AttendanceByEmployeeAndDateRangeSpec : Specification<Attendance>
{
    public AttendanceByEmployeeAndDateRangeSpec(DefaultIdType employeeId, DateTime fromDate, DateTime toDate)
    {
        Query.Where(x => x.EmployeeId == employeeId && x.AttendanceDate >= fromDate && x.AttendanceDate <= toDate)
            .OrderByDescending(x => x.AttendanceDate);
    }
}

/// <summary>
/// Specification for recent payrolls by employee.
/// </summary>
public sealed class PayrollsByEmployeeSpec : Specification<Payroll>
{
    public PayrollsByEmployeeSpec(DefaultIdType employeeId)
    {
        Query.OrderByDescending(x => x.ProcessedDate);
    }
}

/// <summary>
/// Specification for pending leave requests by employee.
/// </summary>
public sealed class PendingLeaveRequestsSpec : Specification<LeaveRequest>
{
    public PendingLeaveRequestsSpec(DefaultIdType employeeId)
    {
        Query.Where(x => x.EmployeeId == employeeId && (x.Status == "Pending" || x.Status == "Submitted"))
            .OrderByDescending(x => x.SubmittedDate ?? x.CreatedOn);
    }
}

/// <summary>
/// Specification for pending timesheets by employee.
/// </summary>
public sealed class PendingTimesheetsSpec : Specification<Timesheet>
{
    public PendingTimesheetsSpec(DefaultIdType employeeId)
    {
        Query.Where(x => x.EmployeeId == employeeId && x.Status == "Pending")
            .OrderByDescending(x => x.CreatedOn);
    }
}

/// <summary>
/// Specification for pending performance reviews by employee.
/// </summary>
public sealed class PendingPerformanceReviewsSpec : Specification<PerformanceReview>
{
    public PendingPerformanceReviewsSpec(DefaultIdType employeeId)
    {
        Query.Where(x => x.EmployeeId == employeeId && (x.Status == "Pending" || x.Status == "Completed"))
            .OrderByDescending(x => x.ReviewPeriodEnd);
    }
}

/// <summary>
/// Specification for performance reviews by employee.
/// </summary>
public sealed class PerformanceReviewsByEmployeeSpec : Specification<PerformanceReview>
{
    public PerformanceReviewsByEmployeeSpec(DefaultIdType employeeId)
    {
        Query.Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.ReviewPeriodEnd);
    }
}

/// <summary>
/// Specification for upcoming shift assignments.
/// </summary>
public sealed class UpcomingShiftAssignmentsSpec : Specification<ShiftAssignment>
{
    public UpcomingShiftAssignmentsSpec(DefaultIdType employeeId, DateTime fromDate)
    {
        Query.Where(x => x.EmployeeId == employeeId && x.StartDate >= fromDate)
            .OrderBy(x => x.StartDate);
    }
}

/// <summary>
/// Specification for upcoming holidays.
/// </summary>
public sealed class UpcomingHolidaysSpec : Specification<Holiday>
{
    public UpcomingHolidaysSpec(DateTime fromDate)
    {
        Query.Where(x => x.HolidayDate >= fromDate)
            .OrderBy(x => x.HolidayDate);
    }
}

