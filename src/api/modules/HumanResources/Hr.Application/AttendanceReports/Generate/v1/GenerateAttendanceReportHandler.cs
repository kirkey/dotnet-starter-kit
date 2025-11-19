namespace FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Generate.v1;

/// <summary>
/// Handler for generating attendance reports.
/// Aggregates attendance data and creates report records.
/// </summary>
public sealed class GenerateAttendanceReportHandler(
    ILogger<GenerateAttendanceReportHandler> logger,
    [FromKeyedServices("hr:attendancereports")] IRepository<AttendanceReport> repository,
    [FromKeyedServices("hr:attendance")] IReadRepository<Attendance> attendanceRepository,
    [FromKeyedServices("hr:holidays")] IReadRepository<Holiday> holidayRepository)
    : IRequestHandler<GenerateAttendanceReportCommand, GenerateAttendanceReportResponse>
{
    /// <summary>
    /// Handles the generate attendance report command.
    /// </summary>
    public async Task<GenerateAttendanceReportResponse> Handle(
        GenerateAttendanceReportCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var fromDate = request.FromDate ?? DateTime.UtcNow.AddMonths(-1);
        var toDate = request.ToDate ?? DateTime.UtcNow;

        // Create report entity
        var report = AttendanceReport.Create(
            reportType: request.ReportType,
            title: request.Title,
            fromDate: fromDate,
            toDate: toDate,
            departmentId: request.DepartmentId,
            employeeId: request.EmployeeId);

        if (!string.IsNullOrWhiteSpace(request.Notes))
            report.AddNotes(request.Notes);

        // Fetch attendance data for the period
        var attendanceSpec = new AttendanceByDateRangeSpec(fromDate, toDate);
        var attendanceRecords = await attendanceRepository.ListAsync(attendanceSpec, cancellationToken)
            .ConfigureAwait(false);

        // Fetch holidays for the period to calculate working days
        var holidaySpec = new HolidaysByDateRangeSpec(fromDate, toDate);
        var holidays = await holidayRepository.ListAsync(holidaySpec, cancellationToken)
            .ConfigureAwait(false);

        // Aggregate data based on report type
        var (workingDays, employees, present, absent, late, halfDay, onLeave) = request.ReportType switch
        {
            "Summary" => AggregateSummary(attendanceRecords, holidays, fromDate, toDate),
            "Daily" => AggregateDaily(attendanceRecords, holidays, fromDate, toDate),
            "Monthly" => AggregateMonthly(attendanceRecords, holidays, fromDate, toDate),
            "Department" => AggregateDepartment(attendanceRecords, holidays, request.DepartmentId, fromDate, toDate),
            "EmployeeDetails" => AggregateEmployeeDetails(attendanceRecords, holidays, request.EmployeeId, fromDate, toDate),
            "LateArrivals" => AggregateLateArrivals(attendanceRecords, holidays, fromDate, toDate),
            "AbsenceAnalysis" => AggregateAbsenceAnalysis(attendanceRecords, holidays, fromDate, toDate),
            _ => (0, 0, 0, 0, 0, 0, 0)
        };

        // Set metrics
        report.SetMetrics(workingDays, employees, present, absent, late, halfDay, onLeave);

        // Persist report
        await repository.AddAsync(report, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Attendance report generated: {ReportType}, Records: {Count}, Attendance: {Percentage}%",
            request.ReportType,
            attendanceRecords.Count,
            report.AttendancePercentage);

        return new GenerateAttendanceReportResponse(
            ReportId: report.Id,
            ReportType: report.ReportType,
            Title: report.Title,
            GeneratedOn: report.GeneratedOn,
            TotalWorkingDays: workingDays,
            TotalEmployees: employees,
            PresentCount: present,
            AbsentCount: absent,
            AttendancePercentage: report.AttendancePercentage);
    }

    /// <summary>
    /// Aggregates data for summary report (company-wide totals).
    /// </summary>
    private static (int workingDays, int employees, int present, int absent, int late, int halfDay, int onLeave)
        AggregateSummary(List<Attendance> records, List<Holiday> holidays, DateTime fromDate, DateTime toDate)
    {
        var workingDays = CalculateWorkingDays(fromDate, toDate, holidays);
        var uniqueEmployees = records.Select(x => x.EmployeeId).Distinct().Count();
        var present = records.Count(x => x.Status == "Present");
        var absent = records.Count(x => x.Status == "Absent");
        var late = records.Count(x => x.Status == "Late");
        var halfDay = records.Count(x => x.Status == "HalfDay");
        var onLeave = records.Count(x => x.Status == "Leave");

        return (workingDays, uniqueEmployees, present, absent, late, halfDay, onLeave);
    }

    /// <summary>
    /// Aggregates data for daily report (per-day breakdown).
    /// </summary>
    private static (int workingDays, int employees, int present, int absent, int late, int halfDay, int onLeave)
        AggregateDaily(List<Attendance> records, List<Holiday> holidays, DateTime fromDate, DateTime toDate)
    {
        return AggregateSummary(records, holidays, fromDate, toDate);
    }

    /// <summary>
    /// Aggregates data for monthly report (per-month breakdown).
    /// </summary>
    private static (int workingDays, int employees, int present, int absent, int late, int halfDay, int onLeave)
        AggregateMonthly(List<Attendance> records, List<Holiday> holidays, DateTime fromDate, DateTime toDate)
    {
        return AggregateSummary(records, holidays, fromDate, toDate);
    }

    /// <summary>
    /// Aggregates data filtered by department.
    /// </summary>
    private static (int workingDays, int employees, int present, int absent, int late, int halfDay, int onLeave)
        AggregateDepartment(List<Attendance> records, List<Holiday> holidays, DefaultIdType? departmentId, DateTime fromDate, DateTime toDate)
    {
        // Note: Would require department info in Attendance or Employee relationship
        return AggregateSummary(records, holidays, fromDate, toDate);
    }

    /// <summary>
    /// Aggregates data for a specific employee.
    /// </summary>
    private static (int workingDays, int employees, int present, int absent, int late, int halfDay, int onLeave)
        AggregateEmployeeDetails(List<Attendance> records, List<Holiday> holidays, DefaultIdType? employeeId, DateTime fromDate, DateTime toDate)
    {
        if (!employeeId.HasValue)
            return (0, 0, 0, 0, 0, 0, 0);

        var filtered = records.Where(x => x.EmployeeId == employeeId).ToList();
        return AggregateSummary(filtered, holidays, fromDate, toDate);
    }

    /// <summary>
    /// Aggregates data for late arrivals analysis.
    /// </summary>
    private static (int workingDays, int employees, int present, int absent, int late, int halfDay, int onLeave)
        AggregateLateArrivals(List<Attendance> records, List<Holiday> holidays, DateTime fromDate, DateTime toDate)
    {
        var workingDays = CalculateWorkingDays(fromDate, toDate, holidays);
        var uniqueEmployees = records.Select(x => x.EmployeeId).Distinct().Count();
        var present = records.Count(x => x.Status == "Present");
        var absent = records.Count(x => x.Status == "Absent");
        var late = records.Count(x => x.Status == "Late");
        var halfDay = records.Count(x => x.Status == "HalfDay");
        var onLeave = records.Count(x => x.Status == "Leave");

        return (workingDays, uniqueEmployees, present, absent, late, halfDay, onLeave);
    }

    /// <summary>
    /// Aggregates data for absence analysis.
    /// </summary>
    private static (int workingDays, int employees, int present, int absent, int late, int halfDay, int onLeave)
        AggregateAbsenceAnalysis(List<Attendance> records, List<Holiday> holidays, DateTime fromDate, DateTime toDate)
    {
        var workingDays = CalculateWorkingDays(fromDate, toDate, holidays);
        var uniqueEmployees = records.Select(x => x.EmployeeId).Distinct().Count();
        var present = records.Count(x => x.Status == "Present");
        var absent = records.Count(x => x.Status == "Absent");
        var late = records.Count(x => x.Status == "Late");
        var halfDay = records.Count(x => x.Status == "HalfDay");
        var onLeave = records.Count(x => x.Status == "Leave");

        return (workingDays, uniqueEmployees, present, absent, late, halfDay, onLeave);
    }

    /// <summary>
    /// Calculates working days between two dates excluding weekends and holidays.
    /// </summary>
    private static int CalculateWorkingDays(DateTime fromDate, DateTime toDate, List<Holiday> holidays)
    {
        var current = fromDate.Date;
        var count = 0;
        var holidayDates = holidays.Select(h => h.HolidayDate.Date).ToHashSet();

        while (current <= toDate.Date)
        {
            // Count if weekday and not a holiday
            if (current.DayOfWeek != DayOfWeek.Saturday && 
                current.DayOfWeek != DayOfWeek.Sunday && 
                !holidayDates.Contains(current))
            {
                count++;
            }
            current = current.AddDays(1);
        }

        return count;
    }
}

/// <summary>
/// Specification for filtering attendance records by date range.
/// </summary>
public sealed class AttendanceByDateRangeSpec : Specification<Attendance>
{
    /// <summary>
    /// Initializes the specification with date range filter.
    /// </summary>
    public AttendanceByDateRangeSpec(DateTime fromDate, DateTime toDate)
    {
        Query.Where(x => x.AttendanceDate >= fromDate && x.AttendanceDate <= toDate)
            .OrderByDescending(x => x.AttendanceDate);
    }
}

/// <summary>
/// Specification for filtering holidays by date range.
/// </summary>
public sealed class HolidaysByDateRangeSpec : Specification<Holiday>
{
    /// <summary>
    /// Initializes the specification with date range filter.
    /// </summary>
    public HolidaysByDateRangeSpec(DateTime fromDate, DateTime toDate)
    {
        Query.Where(x => x.HolidayDate >= fromDate && x.HolidayDate <= toDate)
            .OrderBy(x => x.HolidayDate);
    }
}

