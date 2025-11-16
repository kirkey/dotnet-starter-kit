using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence;

/// <summary>
/// Seeds sample/demo data for all HR entities for testing and development.
/// Adds Philippine-compliant sample data when real data is not available.
/// </summary>
internal sealed class HRDemoDataSeeder
{
    private readonly ILogger<HRDemoDataSeeder> _logger;
    private readonly HumanResourcesDbContext _context;

    public HRDemoDataSeeder(
        ILogger<HRDemoDataSeeder> logger,
        HumanResourcesDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// Seeds all HR demo data.
    /// </summary>
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedEmployeesAsync(cancellationToken);
        await SeedEmployeeContactsAsync(cancellationToken);
        await SeedEmployeeDependentsAsync(cancellationToken);
        await SeedShiftsAsync(cancellationToken);
        await SeedShiftAssignmentsAsync(cancellationToken);
        await SeedHolidaysAsync(cancellationToken);
        await SeedLeaveTypesAsync(cancellationToken);
        await SeedLeaveBalancesAsync(cancellationToken);
        await SeedBenefitsAsync(cancellationToken);
        await SeedBenefitEnrollmentsAsync(cancellationToken);
        await SeedTimesheetsAsync(cancellationToken);
        await SeedDocumentTemplatesAsync(cancellationToken);
    }

    /// <summary>
    /// Seeds sample employees with proper Philippine data.
    /// </summary>
    private async Task SeedEmployeesAsync(CancellationToken cancellationToken)
    {
        if (await _context.Employees.AnyAsync(cancellationToken))
            return;

        var orgUnit = await _context.OrganizationalUnits.FirstAsync(cancellationToken);

        var employees = new[]
        {
            Employee.Create("EMP-001", "Juan", "dela Cruz", orgUnit.Id, "Santos",
                "juan.delacruz@company.com", "+639171234567")
                .SetHireDate(new DateTime(2020, 1, 15))
                .SetPersonalInfo(new DateTime(1990, 5, 15), "Male", "Married")
                .SetGovernmentIds("123-456-789-000", "34-1234567-8", "12-345678901-2", "1234-5678-9012")
                .SetBasicSalary(35000m),

            Employee.Create("EMP-002", "Maria", "Reyes", orgUnit.Id, "Garcia",
                "maria.reyes@company.com", "+639187654321")
                .SetHireDate(new DateTime(2021, 3, 1))
                .SetPersonalInfo(new DateTime(1992, 8, 20), "Female", "Single")
                .SetGovernmentIds("987-654-321-000", "34-7654321-1", "12-987654321-1", "9876-5432-1098")
                .SetBasicSalary(32000m),

            Employee.Create("EMP-003", "Pedro", "Tan", orgUnit.Id, "Lim",
                "pedro.tan@company.com", "+639191234567")
                .SetHireDate(new DateTime(2019, 6, 15))
                .SetPersonalInfo(new DateTime(1988, 12, 10), "Male", "Married")
                .SetGovernmentIds("111-222-333-000", "34-1112223-3", "12-111222333-4", "1112-2233-3444")
                .SetBasicSalary(45000m),

            Employee.Create("EMP-004", "Ana", "Lopez", orgUnit.Id, "Cruz",
                "ana.lopez@company.com", "+639177654321")
                .SetHireDate(new DateTime(2022, 9, 1))
                .SetPersonalInfo(new DateTime(1995, 3, 25), "Female", "Single")
                .SetGovernmentIds("444-555-666-000", "34-4445556-6", "12-444555666-7", "4445-5566-6777")
                .SetBasicSalary(28000m),

            Employee.Create("EMP-005", "Roberto", "Martinez", orgUnit.Id, "Silva",
                "roberto.martinez@company.com", "+639198765432")
                .SetHireDate(new DateTime(2018, 2, 1))
                .SetPersonalInfo(new DateTime(1985, 7, 30), "Male", "Married")
                .SetGovernmentIds("777-888-999-000", "34-7778889-9", "12-777888999-0", "7778-8899-9000")
                .SetBasicSalary(50000m)
        };

        await _context.Employees.AddRangeAsync(employees, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} sample employees", 
            _context.TenantInfo!.Identifier, employees.Length);
    }

    /// <summary>
    /// Seeds sample employee emergency contacts.
    /// </summary>
    private async Task SeedEmployeeContactsAsync(CancellationToken cancellationToken)
    {
        if (await _context.EmployeeContacts.AnyAsync(cancellationToken))
            return;

        var employees = await _context.Employees.Take(3).ToListAsync(cancellationToken);
        if (!employees.Any()) return;

        var contacts = new List<EmployeeContact>();

        foreach (var employee in employees)
        {
            contacts.Add(EmployeeContact.Create(
                employee.Id,
                "Emergency",
                "Maria",
                "dela Cruz",
                "Spouse",
                "+639171111111",
                "emergency@example.com",
                "Same address")
                .SetPriority(1));
        }

        await _context.EmployeeContacts.AddRangeAsync(contacts, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} employee contacts",
            _context.TenantInfo!.Identifier, contacts.Count);
    }

    /// <summary>
    /// Seeds sample employee dependents.
    /// </summary>
    private async Task SeedEmployeeDependentsAsync(CancellationToken cancellationToken)
    {
        if (await _context.EmployeeDependents.AnyAsync(cancellationToken))
            return;

        var employees = await _context.Employees.Take(3).ToListAsync(cancellationToken);
        if (!employees.Any()) return;

        var dependents = new List<EmployeeDependent>();

        foreach (var employee in employees)
        {
            dependents.Add(EmployeeDependent.Create(
                employee.Id,
                "Child",
                "Child",
                employee.LastName,
                new DateTime(2015, 1, 1),
                "Child")
                .SetAsBeneficiary(true)
                .SetAsClaimableDependent(true));
        }

        await _context.EmployeeDependents.AddRangeAsync(dependents, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} employee dependents",
            _context.TenantInfo!.Identifier, dependents.Count);
    }

    /// <summary>
    /// Seeds standard work shifts.
    /// </summary>
    private async Task SeedShiftsAsync(CancellationToken cancellationToken)
    {
        if (await _context.Shifts.AnyAsync(cancellationToken))
            return;

        var dayShift = Shift.Create("Day Shift", new TimeSpan(8, 0, 0), new TimeSpan(17, 0, 0), true);
        dayShift.SetDescription("Standard day shift 8AM-5PM");

        var nightShift = Shift.Create("Night Shift", new TimeSpan(22, 0, 0), new TimeSpan(6, 0, 0), true);
        nightShift.SetDescription("Night shift 10PM-6AM with night differential");

        var midShift = Shift.Create("Mid Shift", new TimeSpan(14, 0, 0), new TimeSpan(22, 0, 0), true);
        midShift.SetDescription("Mid shift 2PM-10PM");

        var shifts = new[] { dayShift, nightShift, midShift };

        await _context.Shifts.AddRangeAsync(shifts, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} shifts",
            _context.TenantInfo!.Identifier, shifts.Length);
    }

    /// <summary>
    /// Seeds sample shift assignments.
    /// </summary>
    private async Task SeedShiftAssignmentsAsync(CancellationToken cancellationToken)
    {
        if (await _context.ShiftAssignments.AnyAsync(cancellationToken))
            return;

        var employees = await _context.Employees.Take(3).ToListAsync(cancellationToken);
        var shift = await _context.Shifts.FirstOrDefaultAsync(cancellationToken);
        
        if (!employees.Any() || shift == null) return;

        var assignments = new List<ShiftAssignment>();

        foreach (var employee in employees)
        {
            assignments.Add(ShiftAssignment.Create(
                employee.Id,
                shift.Id,
                DateTime.Today.AddDays(-30)));
        }

        await _context.ShiftAssignments.AddRangeAsync(assignments, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} shift assignments",
            _context.TenantInfo!.Identifier, assignments.Count);
    }

    /// <summary>
    /// Seeds Philippine holidays for 2025 per Proclamation.
    /// </summary>
    private async Task SeedHolidaysAsync(CancellationToken cancellationToken)
    {
        if (await _context.Holidays.AnyAsync(cancellationToken))
            return;

        var holidays = new[]
        {
            Holiday.Create("New Year's Day", new DateTime(2025, 1, 1))
                .SetDescription("New Year celebration"),
            
            Holiday.Create("EDSA People Power Revolution", new DateTime(2025, 2, 25), false)
                .SetDescription("People Power Anniversary"),
            
            Holiday.Create("Maundy Thursday", new DateTime(2025, 4, 17))
                .SetDescription("Holy Week"),
            
            Holiday.Create("Good Friday", new DateTime(2025, 4, 18))
                .SetDescription("Holy Week"),
            
            Holiday.Create("Araw ng Kagitingan", new DateTime(2025, 4, 9))
                .SetDescription("Day of Valor - Bataan Day"),
            
            Holiday.Create("Labor Day", new DateTime(2025, 5, 1))
                .SetDescription("International Workers Day"),
            
            Holiday.Create("Independence Day", new DateTime(2025, 6, 12))
                .SetDescription("Philippine Independence Day"),
            
            Holiday.Create("Ninoy Aquino Day", new DateTime(2025, 8, 21), false)
                .SetDescription("Ninoy Aquino Day"),
            
            Holiday.Create("National Heroes Day", new DateTime(2025, 8, 25))
                .SetDescription("Last Monday of August"),
            
            Holiday.Create("All Saints Day", new DateTime(2025, 11, 1), false)
                .SetDescription("All Saints Day"),
            
            Holiday.Create("Bonifacio Day", new DateTime(2025, 11, 30))
                .SetDescription("Andres Bonifacio Day"),
            
            Holiday.Create("Immaculate Conception", new DateTime(2025, 12, 8), false)
                .SetDescription("Feast of the Immaculate Conception"),
            
            Holiday.Create("Christmas Day", new DateTime(2025, 12, 25))
                .SetDescription("Christmas"),
            
            Holiday.Create("Rizal Day", new DateTime(2025, 12, 30))
                .SetDescription("Jose Rizal Day"),
            
            Holiday.Create("Last Day of the Year", new DateTime(2025, 12, 31), false)
                .SetDescription("New Year's Eve")
        };

        await _context.Holidays.AddRangeAsync(holidays, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} Philippine holidays for 2025",
            _context.TenantInfo!.Identifier, holidays.Length);
    }

    /// <summary>
    /// Seeds standard leave types per Philippine Labor Code.
    /// </summary>
    private async Task SeedLeaveTypesAsync(CancellationToken cancellationToken)
    {
        if (await _context.LeaveTypes.AnyAsync(cancellationToken))
            return;

        var leaveTypes = new[]
        {
            LeaveType.Create("Service Incentive Leave", 5.0m),
            LeaveType.Create("Sick Leave", 15.0m),
            LeaveType.Create("Vacation Leave", 15.0m),
            LeaveType.Create("Maternity Leave", 105.0m),
            LeaveType.Create("Paternity Leave", 7.0m),
            LeaveType.Create("Solo Parent Leave", 7.0m),
            LeaveType.Create("Bereavement Leave", 3.0m),
            LeaveType.Create("Emergency Leave", 3.0m, false)
        };

        await _context.LeaveTypes.AddRangeAsync(leaveTypes, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} leave types",
            _context.TenantInfo!.Identifier, leaveTypes.Length);
    }

    /// <summary>
    /// Seeds leave balances for regular employees.
    /// </summary>
    private async Task SeedLeaveBalancesAsync(CancellationToken cancellationToken)
    {
        if (await _context.LeaveBalances.AnyAsync(cancellationToken))
            return;

        var employees = await _context.Employees.Take(3).ToListAsync(cancellationToken);
        var leaveTypes = await _context.LeaveTypes.Take(3).ToListAsync(cancellationToken);

        if (!employees.Any() || !leaveTypes.Any()) return;

        var balances = new List<LeaveBalance>();

        foreach (var employee in employees)
        {
            foreach (var leaveType in leaveTypes)
            {
                balances.Add(LeaveBalance.Create(
                    employee.Id,
                    leaveType.Id,
                    2025,
                    leaveType.AnnualAllowance));
            }
        }

        await _context.LeaveBalances.AddRangeAsync(balances, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} leave balances",
            _context.TenantInfo!.Identifier, balances.Count);
    }

    /// <summary>
    /// Seeds sample company benefits.
    /// </summary>
    private async Task SeedBenefitsAsync(CancellationToken cancellationToken)
    {
        if (await _context.Benefits.AnyAsync(cancellationToken))
            return;

        var benefits = new[]
        {
            Benefit.Create("Health Insurance", "Health", 500m, 1000m)
                .SetCoverageDetails("Family", 500000m, "Maxicare")
                .SetAsMandatory(false),

            Benefit.Create("Life Insurance", "Life", 100m, 200m)
                .SetCoverageDetails("Individual", 1000000m, "Sunlife")
                .SetAsMandatory(false),

            Benefit.Create("Rice Subsidy", "Allowance", 0m, 2000m)
                .SetAsMandatory(false)
        };

        await _context.Benefits.AddRangeAsync(benefits, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} benefits",
            _context.TenantInfo!.Identifier, benefits.Length);
    }

    /// <summary>
    /// Seeds sample benefit enrollments.
    /// </summary>
    private async Task SeedBenefitEnrollmentsAsync(CancellationToken cancellationToken)
    {
        if (await _context.BenefitEnrollments.AnyAsync(cancellationToken))
            return;

        var employees = await _context.Employees.Take(2).ToListAsync(cancellationToken);
        var benefit = await _context.Benefits.FirstOrDefaultAsync(cancellationToken);

        if (!employees.Any() || benefit == null) return;

        var enrollments = new List<BenefitEnrollment>();

        foreach (var employee in employees)
        {
            enrollments.Add(BenefitEnrollment.Create(
                employee.Id,
                benefit.Id,
                DateTime.Today.AddDays(-60),
                DateTime.Today.AddDays(-30))
                .SetCoverage("Individual", 500m, 1000m));
        }

        await _context.BenefitEnrollments.AddRangeAsync(enrollments, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} benefit enrollments",
            _context.TenantInfo!.Identifier, enrollments.Count);
    }

    /// <summary>
    /// Seeds sample approved timesheets with timesheet lines.
    /// Follows the accounting invoice/line item pattern.
    /// </summary>
    private async Task SeedTimesheetsAsync(CancellationToken cancellationToken)
    {
        if (await _context.Timesheets.AnyAsync(cancellationToken))
            return;

        var employees = await _context.Employees.Take(2).ToListAsync(cancellationToken);
        if (!employees.Any()) return;

        var timesheets = new List<Timesheet>();

        foreach (var employee in employees)
        {
            // Create timesheet for a two-week period (last two weeks)
            var startDate = DateTime.Today.AddDays(-14);
            var endDate = DateTime.Today.AddDays(-8);
            
            var timesheet = Timesheet.Create(
                employee.Id,
                startDate,
                endDate);

            // Add timesheet lines for each work day (Monday to Friday)
            // Following the invoice.AddLineItem() pattern
            for (int day = 0; day < 5; day++)
            {
                var workDate = startDate.AddDays(day);
                
                // Create timesheet line with regular hours
                var line = TimesheetLine.Create(
                    timesheet.Id,
                    workDate,
                    regularHours: 8.0m,
                    overtimeHours: 0m,
                    projectId: null,
                    taskDescription: "Regular work day");

                // Add line to timesheet (similar to invoice.AddLineItem)
                timesheet.AddLine(line);
            }

            // Submit and approve the timesheet
            timesheet.Submit();
            timesheet.Approve("Approved - Sample data");

            timesheets.Add(timesheet);
        }

        await _context.Timesheets.AddRangeAsync(timesheets, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} timesheets with {LineCount} total lines",
            _context.TenantInfo!.Identifier, 
            timesheets.Count,
            timesheets.Sum(t => t.Lines.Count));
    }

    /// <summary>
    /// Seeds document templates for HR documents.
    /// </summary>
    private async Task SeedDocumentTemplatesAsync(CancellationToken cancellationToken)
    {
        if (await _context.DocumentTemplates.AnyAsync(cancellationToken))
            return;

        var templates = new[]
        {
            DocumentTemplate.Create(
                "Employment Certificate",
                "Certificate",
                @"This is to certify that {{EmployeeName}} has been employed with {{CompanyName}} since {{HireDate}} as {{Designation}}."),

            DocumentTemplate.Create(
                "Certificate of Employment",
                "Certificate",
                @"TO WHOM IT MAY CONCERN:

This is to certify that {{EmployeeName}} is currently employed with {{CompanyName}} as {{Designation}} since {{HireDate}}.

This certification is being issued upon the request of the employee for whatever legal purpose it may serve.

Issued this {{IssueDate}} at {{Location}}."),

            DocumentTemplate.Create(
                "Payslip",
                "Payroll",
                @"PAYSLIP for {{PayPeriod}}
Employee: {{EmployeeName}} ({{EmployeeNumber}})
Basic Pay: {{BasicPay}}
Deductions: {{Deductions}}
Net Pay: {{NetPay}}")
        };

        await _context.DocumentTemplates.AddRangeAsync(templates, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} document templates",
            _context.TenantInfo!.Identifier, templates.Length);
    }
}

