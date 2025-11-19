using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence;

/// <summary>
/// Seeds sample/demo data for all HR entities for testing and development.
/// Adds Philippine-compliant sample data when real data is not available.
/// </summary>
internal sealed class HrDemoDataSeeder(
    ILogger<HrDemoDataSeeder> logger,
    HrDbContext context)
{
    /// <summary>
    /// Seeds all HR demo data.
    /// </summary>
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedOrganizationalUnitsAsync(cancellationToken);
        await SeedDesignationsAsync(cancellationToken);
        await SeedEmployeesAsync(cancellationToken);
        await SeedEmployeeContactsAsync(cancellationToken);
        await SeedEmployeeDependentsAsync(cancellationToken);
        await SeedEmployeeEducationsAsync(cancellationToken);
        await SeedBankAccountsAsync(cancellationToken);
        await SeedShiftsAsync(cancellationToken);
        await SeedShiftAssignmentsAsync(cancellationToken);
        await SeedHolidaysAsync(cancellationToken);
        await SeedLeaveTypesAsync(cancellationToken);
        await SeedLeaveBalancesAsync(cancellationToken);
        await SeedLeaveRequestsAsync(cancellationToken);
        await SeedAttendanceAsync(cancellationToken);
        await SeedBenefitsAsync(cancellationToken);
        await SeedBenefitAllocationsAsync(cancellationToken);
        await SeedBenefitEnrollmentsAsync(cancellationToken);
        await SeedTimesheetsAsync(cancellationToken);
        await SeedPayComponentsAsync(cancellationToken);
        await SeedTaxBracketsAsync(cancellationToken);
        await SeedTaxMastersAsync(cancellationToken);
        await SeedPayComponentsAsync(cancellationToken);
        await SeedTaxBracketsAsync(cancellationToken);
        await SeedTaxMastersAsync(cancellationToken);
        await SeedPayComponentRatesAsync(cancellationToken);
        await SeedPayrollDeductionsAsync(cancellationToken);
        await SeedShiftBreaksAsync(cancellationToken);
        await SeedEmployeeDocumentsAsync(cancellationToken);
        await SeedPerformanceReviewsAsync(cancellationToken);
        await SeedPayrollsAsync(cancellationToken);
        await SeedDocumentTemplatesAsync(cancellationToken);
    }

    /// <summary>
    /// Seeds organizational units hierarchy.
    /// </summary>
    private async Task SeedOrganizationalUnitsAsync(CancellationToken cancellationToken)
    {
        if (await context.OrganizationalUnits.AnyAsync(cancellationToken))
            return;

        var company = OrganizationalUnit.Create("COMPANY", "Main Company", OrganizationalUnitType.Company);
        var deptHr = OrganizationalUnit.Create("DEPT-HR", "Human Resources", OrganizationalUnitType.Department, company.Id);
        var deptIt = OrganizationalUnit.Create("DEPT-IT", "Information Technology", OrganizationalUnitType.Department, company.Id);
        var deptFin = OrganizationalUnit.Create("DEPT-FIN", "Finance", OrganizationalUnitType.Department, company.Id);
        var deptOps = OrganizationalUnit.Create("DEPT-OPS", "Operations", OrganizationalUnitType.Department, company.Id);

        var teamHrAd = OrganizationalUnit.Create("TEAM-HR-AD", "HR Admin", OrganizationalUnitType.Section, deptHr.Id);
        var teamHrRc = OrganizationalUnit.Create("TEAM-HR-RC", "Recruitment", OrganizationalUnitType.Section, deptHr.Id);
        var teamItDev = OrganizationalUnit.Create("TEAM-IT-DEV", "Development", OrganizationalUnitType.Section, deptIt.Id);
        var teamItOps = OrganizationalUnit.Create("TEAM-IT-OPS", "IT Operations", OrganizationalUnitType.Section, deptIt.Id);
        var teamFinAc = OrganizationalUnit.Create("TEAM-FIN-AC", "Accounting", OrganizationalUnitType.Section, deptFin.Id);
        var teamOpsMfg = OrganizationalUnit.Create("TEAM-OPS-MFG", "Manufacturing", OrganizationalUnitType.Section, deptOps.Id);

        var units = new[] { company, deptHr, deptIt, deptFin, deptOps, teamHrAd, teamHrRc, teamItDev, teamItOps, teamFinAc, teamOpsMfg };

        await context.OrganizationalUnits.AddRangeAsync(units, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} organizational units", context.TenantInfo!.Identifier, units.Length);
    }

    /// <summary>
    /// Seeds job designations with area-specific salary ranges.
    /// </summary>
    private async Task SeedDesignationsAsync(CancellationToken cancellationToken)
    {
        if (await context.Designations.AnyAsync(cancellationToken))
            return;

        var designations = new[]
        {
            Designation.Create("MGR-001", "Department Manager", "National", "Manager-level position", null, 50000m, 120000m),
            Designation.Create("SR-ENG-001", "Senior Engineer", "National", "Senior engineering professional", null, 45000m, 100000m),
            Designation.Create("ENG-001", "Engineer", "National", "Mid-level engineer", null, 35000m, 75000m),
            Designation.Create("JR-ENG-001", "Junior Engineer", "National", "Entry-level engineer", null, 25000m, 50000m),
            Designation.Create("ANALYST-001", "Business Analyst", "National", "Business analysis", null, 40000m, 85000m),
            Designation.Create("ACCT-001", "Accountant", "National", "Accounting professional", null, 35000m, 70000m),
            Designation.Create("COORD-001", "Coordinator", "National", "Administrative coordinator", null, 25000m, 50000m),
            Designation.Create("ADMIN-001", "Administrative Officer", "National", "Administrative support", null, 22000m, 45000m)
        };

        foreach (var desig in designations)
            desig.Activate();

        await context.Designations.AddRangeAsync(designations, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} designations", context.TenantInfo!.Identifier, designations.Length);
    }

    /// <summary>
    /// Seeds sample employees with proper Philippine data.
    /// </summary>
    private async Task SeedEmployeesAsync(CancellationToken cancellationToken)
    {
        if (await context.Employees.AnyAsync(cancellationToken))
            return;

        var defaultTeam = await context.OrganizationalUnits.FirstAsync(cancellationToken);

        var employees = new[]
        {
            Employee.Create("EMP-001", "Juan", "dela Cruz", defaultTeam.Id, "Santos", "juan.delacruz@company.com", "+639171234567")
                .SetHireDate(new DateTime(2020, 1, 15))
                .SetPersonalInfo(new DateTime(1990, 5, 15), "Male", "Married")
                .SetGovernmentIds("123-456-789-000", "34-1234567-8", "12-345678901-2", "1234-5678-9012")
                .SetBasicSalary(45000m),
            Employee.Create("EMP-002", "Maria", "Reyes", defaultTeam.Id, "Garcia", "maria.reyes@company.com", "+639187654321")
                .SetHireDate(new DateTime(2021, 3, 1))
                .SetPersonalInfo(new DateTime(1992, 8, 20), "Female", "Single")
                .SetGovernmentIds("987-654-321-000", "34-7654321-1", "12-987654321-1", "9876-5432-1098")
                .SetBasicSalary(38000m),
            Employee.Create("EMP-003", "Pedro", "Tan", defaultTeam.Id, "Lim", "pedro.tan@company.com", "+639191234567")
                .SetHireDate(new DateTime(2019, 6, 15))
                .SetPersonalInfo(new DateTime(1988, 12, 10), "Male", "Married")
                .SetGovernmentIds("111-222-333-000", "34-1112223-3", "12-111222333-4", "1112-2233-3444")
                .SetBasicSalary(55000m),
            Employee.Create("EMP-004", "Ana", "Lopez", defaultTeam.Id, "Cruz", "ana.lopez@company.com", "+639177654321")
                .SetHireDate(new DateTime(2022, 9, 1))
                .SetPersonalInfo(new DateTime(1995, 3, 25), "Female", "Single")
                .SetGovernmentIds("444-555-666-000", "34-4445556-6", "12-444555666-7", "4445-5566-6777")
                .SetBasicSalary(35000m),
            Employee.Create("EMP-005", "Roberto", "Martinez", defaultTeam.Id, "Silva", "roberto.martinez@company.com", "+639198765432")
                .SetHireDate(new DateTime(2018, 2, 1))
                .SetPersonalInfo(new DateTime(1985, 7, 30), "Male", "Married")
                .SetGovernmentIds("777-888-999-000", "34-7778889-9", "12-777888999-0", "7778-8899-9000")
                .SetBasicSalary(52000m),
            Employee.Create("EMP-006", "Christine", "Gonzales", defaultTeam.Id, "Mercado", "christine.gonzales@company.com", "+639165432109")
                .SetHireDate(new DateTime(2021, 7, 15))
                .SetPersonalInfo(new DateTime(1994, 2, 14), "Female", "Married")
                .SetGovernmentIds("555-666-777-000", "34-5556667-7", "12-555666777-8", "5556-6677-7888")
                .SetBasicSalary(42000m),
            Employee.Create("EMP-007", "Michael", "Santos", defaultTeam.Id, "Ramos", "michael.santos@company.com", "+639173456789")
                .SetHireDate(new DateTime(2020, 11, 1))
                .SetPersonalInfo(new DateTime(1991, 9, 22), "Male", "Single")
                .SetGovernmentIds("888-999-111-000", "34-8889991-1", "12-888999111-2", "8889-9911-1222")
                .SetBasicSalary(40000m),
            Employee.Create("EMP-008", "Grace", "Villanueva", defaultTeam.Id, "Aquino", "grace.villanueva@company.com", "+639169876543")
                .SetHireDate(new DateTime(2022, 1, 10))
                .SetPersonalInfo(new DateTime(1996, 11, 8), "Female", "Single")
                .SetGovernmentIds("222-333-444-000", "34-2223334-4", "12-222333444-5", "2223-3344-4555")
                .SetBasicSalary(32000m),
            Employee.Create("EMP-009", "Carlos", "Fernandez", defaultTeam.Id, "Mendoza", "carlos.fernandez@company.com", "+639182345678")
                .SetHireDate(new DateTime(2019, 5, 20))
                .SetPersonalInfo(new DateTime(1987, 4, 3), "Male", "Married")
                .SetGovernmentIds("333-444-555-000", "34-3334445-5", "12-333444555-6", "3334-4455-5666")
                .SetBasicSalary(48000m),
            Employee.Create("EMP-010", "Diana", "Castillo", defaultTeam.Id, "Moreno", "diana.castillo@company.com", "+639171112222")
                .SetHireDate(new DateTime(2021, 4, 15))
                .SetPersonalInfo(new DateTime(1993, 6, 7), "Female", "Married")
                .SetGovernmentIds("666-777-888-000", "34-6667778-8", "12-666777888-9", "6667-7788-8999")
                .SetBasicSalary(44000m)
        };

        await context.Employees.AddRangeAsync(employees, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} sample employees", context.TenantInfo!.Identifier, employees.Length);
    }

    /// <summary>
    /// Seeds sample employee emergency contacts.
    /// </summary>
    private async Task SeedEmployeeContactsAsync(CancellationToken cancellationToken)
    {
        if (await context.EmployeeContacts.AnyAsync(cancellationToken))
            return;

        var employees = await context.Employees.ToListAsync(cancellationToken);
        if (employees.Count == 0) return;

        var contacts = new List<EmployeeContact>();

        foreach (var employee in employees)
        {
            contacts.Add(EmployeeContact.Create(employee.Id, "Emergency", "Maria", "dela Cruz", "Spouse", "+639171111111", "emergency@example.com", "Same address").SetPriority(1));
            if (Random.Shared.Next(2) == 0)
                contacts.Add(EmployeeContact.Create(employee.Id, "Alternative", "Parent", employee.LastName, "Parent", "+639172222222", "parent@example.com", "Home address").SetPriority(2));
        }

        await context.EmployeeContacts.AddRangeAsync(contacts, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} employee contacts", context.TenantInfo!.Identifier, contacts.Count);
    }

    /// <summary>
    /// Seeds sample employee dependents.
    /// </summary>
    private async Task SeedEmployeeDependentsAsync(CancellationToken cancellationToken)
    {
        if (await context.EmployeeDependents.AnyAsync(cancellationToken))
            return;

        var employees = await context.Employees.ToListAsync(cancellationToken);
        if (employees.Count == 0) return;

        var dependents = new List<EmployeeDependent>();

        foreach (var employee in employees)
        {
            dependents.Add(EmployeeDependent.Create(employee.Id, "Child", "Child", employee.LastName, new DateTime(2015, 5, 15), "Child").SetAsBeneficiary(true).SetAsClaimableDependent(true));
            if (Random.Shared.Next(2) == 0)
                dependents.Add(EmployeeDependent.Create(employee.Id, "Spouse", "Spouse", "de " + employee.LastName, new DateTime(1990, 3, 20), "Spouse").SetAsBeneficiary(true).SetAsClaimableDependent(true));
        }

        await context.EmployeeDependents.AddRangeAsync(dependents, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} employee dependents", context.TenantInfo!.Identifier, dependents.Count);
    }

    /// <summary>
    /// Seeds employee education history.
    /// </summary>
    private async Task SeedEmployeeEducationsAsync(CancellationToken cancellationToken)
    {
        if (await context.EmployeeEducations.AnyAsync(cancellationToken))
            return;

        var employees = await context.Employees.ToListAsync(cancellationToken);
        if (employees.Count == 0) return;

        var educations = new List<EmployeeEducation>();

        foreach (var employee in employees)
        {
            educations.Add(EmployeeEducation.Create(employee.Id, "Bachelor", "Computer Science", "University of the Philippines", DateTime.Today.AddYears(-8)));
            if (Random.Shared.Next(100) < 30)
                educations.Add(EmployeeEducation.Create(employee.Id, "Master", "Business Administration", "De La Salle University", DateTime.Today.AddYears(-5)));
        }

        await context.EmployeeEducations.AddRangeAsync(educations, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} employee education records", context.TenantInfo!.Identifier, educations.Count);
    }

    /// <summary>
    /// Seeds employee bank accounts for payroll.
    /// </summary>
    private async Task SeedBankAccountsAsync(CancellationToken cancellationToken)
    {
        if (await context.BankAccounts.AnyAsync(cancellationToken))
            return;

        var employees = await context.Employees.ToListAsync(cancellationToken);
        if (employees.Count == 0) return;

        var bankAccounts = new List<BankAccount>();
        var banks = new[] { "BDO", "BPI", "Metrobank", "PNB", "Security Bank", "Maybank", "RCBC" };

        foreach (var employee in employees)
        {
            var bankAccountNumber = Random.Shared.Next(100000000, 999999999).ToString();
            var bank = banks[Random.Shared.Next(banks.Length)];
            var accountHolderName = employee.FirstName + " " + employee.LastName;
            bankAccounts.Add(BankAccount.Create(employee.Id, bank, "Savings", bankAccountNumber, accountHolderName, accountHolderName));
        }

        await context.BankAccounts.AddRangeAsync(bankAccounts, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} bank accounts", context.TenantInfo!.Identifier, bankAccounts.Count);
    }

    /// <summary>
    /// Seeds standard work shifts.
    /// </summary>
    private async Task SeedShiftsAsync(CancellationToken cancellationToken)
    {
        if (await context.Shifts.AnyAsync(cancellationToken))
            return;

        var dayShift = Shift.Create("Day Shift", new TimeSpan(8, 0, 0), new TimeSpan(17, 0, 0), true);
        dayShift.SetDescription("Standard day shift 8AM-5PM");

        var nightShift = Shift.Create("Night Shift", new TimeSpan(22, 0, 0), new TimeSpan(6, 0, 0), true);
        nightShift.SetDescription("Night shift 10PM-6AM with night differential");

        var midShift = Shift.Create("Mid Shift", new TimeSpan(14, 0, 0), new TimeSpan(22, 0, 0), true);
        midShift.SetDescription("Mid shift 2PM-10PM");

        var shifts = new[] { dayShift, nightShift, midShift };

        await context.Shifts.AddRangeAsync(shifts, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} shifts", context.TenantInfo!.Identifier, shifts.Length);
    }

    /// <summary>
    /// Seeds sample shift assignments for all employees.
    /// </summary>
    private async Task SeedShiftAssignmentsAsync(CancellationToken cancellationToken)
    {
        if (await context.ShiftAssignments.AnyAsync(cancellationToken))
            return;

        var employees = await context.Employees.ToListAsync(cancellationToken);
        var shifts = await context.Shifts.ToListAsync(cancellationToken);
        
        if (employees.Count == 0 || shifts.Count == 0) return;

        var assignments = new List<ShiftAssignment>();

        foreach (var employee in employees)
        {
            var shift = shifts[Random.Shared.Next(shifts.Count)];
            assignments.Add(ShiftAssignment.Create(employee.Id, shift.Id, DateTime.Today.AddDays(-60)));

            if (Random.Shared.Next(3) == 0)
            {
                var alternateShift = shifts.FirstOrDefault(s => s.Id != shift.Id);
                if (alternateShift != null)
                    assignments.Add(ShiftAssignment.Create(employee.Id, alternateShift.Id, DateTime.Today.AddDays(-30)));
            }
        }

        await context.ShiftAssignments.AddRangeAsync(assignments, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} shift assignments", context.TenantInfo!.Identifier, assignments.Count);
    }

    /// <summary>
    /// Seeds sample daily attendance records for the last 30 days.
    /// </summary>
    private async Task SeedAttendanceAsync(CancellationToken cancellationToken)
    {
        if (await context.Attendances.AnyAsync(cancellationToken))
            return;

        var employees = await context.Employees.ToListAsync(cancellationToken);
        if (employees.Count == 0) return;

        var attendances = new List<Attendance>();
        var today = DateTime.Today;

        foreach (var employee in employees)
        {
            for (int daysBack = 0; daysBack < 30; daysBack++)
            {
                var date = today.AddDays(-daysBack);
                
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                var isAbsent = Random.Shared.Next(100) < 5;
                var isLate = Random.Shared.Next(100) < 10;
                
                if (isAbsent)
                    attendances.Add(Attendance.Create(employee.Id, date, null, null, "Absent", "Absent - No notification"));
                else if (isLate)
                {
                    var inTime = new TimeSpan(8, Random.Shared.Next(15, 45), 0);
                    var outTime = new TimeSpan(17, Random.Shared.Next(0, 30), 0);
                    attendances.Add(Attendance.Create(employee.Id, date, inTime, outTime, "Late", "Arrived late - traffic"));
                }
                else
                {
                    var inTime = new TimeSpan(8, Random.Shared.Next(0, 10), 0);
                    var outTime = new TimeSpan(17, Random.Shared.Next(0, 15), 0);
                    attendances.Add(Attendance.Create(employee.Id, date, inTime, outTime, "Present", "Regular working day"));
                }
            }
        }

        await context.Attendances.AddRangeAsync(attendances, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} attendance records", context.TenantInfo!.Identifier, attendances.Count);
    }

    /// <summary>
    /// Seeds Philippine holidays for 2025.
    /// </summary>
    private async Task SeedHolidaysAsync(CancellationToken cancellationToken)
    {
        if (await context.Holidays.AnyAsync(cancellationToken))
            return;

        var holidays = new[]
        {
            Holiday.Create("New Year's Day", new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)).SetDescription("New Year celebration"),
            Holiday.Create("EDSA People Power Revolution", new DateTime(2025, 2, 25, 0, 0, 0, DateTimeKind.Utc), false).SetDescription("People Power Anniversary"),
            Holiday.Create("Maundy Thursday", new DateTime(2025, 4, 17, 0, 0, 0, DateTimeKind.Utc)).SetDescription("Holy Week"),
            Holiday.Create("Good Friday", new DateTime(2025, 4, 18, 0, 0, 0, DateTimeKind.Utc)).SetDescription("Holy Week"),
            Holiday.Create("Araw ng Kagitingan", new DateTime(2025, 4, 9, 0, 0, 0, DateTimeKind.Utc)).SetDescription("Day of Valor - Bataan Day"),
            Holiday.Create("Labor Day", new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc)).SetDescription("International Workers Day"),
            Holiday.Create("Independence Day", new DateTime(2025, 6, 12, 0, 0, 0, DateTimeKind.Utc)).SetDescription("Philippine Independence Day"),
            Holiday.Create("Ninoy Aquino Day", new DateTime(2025, 8, 21, 0, 0, 0, DateTimeKind.Utc), false).SetDescription("Ninoy Aquino Day"),
            Holiday.Create("National Heroes Day", new DateTime(2025, 8, 25, 0, 0, 0, DateTimeKind.Utc)).SetDescription("Last Monday of August"),
            Holiday.Create("All Saints Day", new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc), false).SetDescription("All Saints Day"),
            Holiday.Create("Bonifacio Day", new DateTime(2025, 11, 30, 0, 0, 0, DateTimeKind.Utc)).SetDescription("Andres Bonifacio Day"),
            Holiday.Create("Immaculate Conception", new DateTime(2025, 12, 8, 0, 0, 0, DateTimeKind.Utc), false).SetDescription("Feast of the Immaculate Conception"),
            Holiday.Create("Christmas Day", new DateTime(2025, 12, 25, 0, 0, 0, DateTimeKind.Utc)).SetDescription("Christmas"),
            Holiday.Create("Rizal Day", new DateTime(2025, 12, 30, 0, 0, 0, DateTimeKind.Utc)).SetDescription("Jose Rizal Day"),
            Holiday.Create("Last Day of the Year", new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), false).SetDescription("New Year's Eve")
        };

        await context.Holidays.AddRangeAsync(holidays, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} Philippine holidays for 2025", context.TenantInfo!.Identifier, holidays.Length);
    }

    /// <summary>
    /// Seeds standard leave types per Philippine Labor Code.
    /// </summary>
    private async Task SeedLeaveTypesAsync(CancellationToken cancellationToken)
    {
        if (await context.LeaveTypes.AnyAsync(cancellationToken))
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

        await context.LeaveTypes.AddRangeAsync(leaveTypes, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} leave types", context.TenantInfo!.Identifier, leaveTypes.Length);
    }

    /// <summary>
    /// Seeds leave balances for all employees.
    /// </summary>
    private async Task SeedLeaveBalancesAsync(CancellationToken cancellationToken)
    {
        if (await context.LeaveBalances.AnyAsync(cancellationToken))
            return;

        var employees = await context.Employees.ToListAsync(cancellationToken);
        var leaveTypes = await context.LeaveTypes.ToListAsync(cancellationToken);

        if (employees.Count == 0 || leaveTypes.Count == 0) return;

        var balances = new List<LeaveBalance>();

        foreach (var employee in employees)
        {
            foreach (var leaveType in leaveTypes)
            {
                var allowance = leaveType.AnnualAllowance;
                var balance = LeaveBalance.Create(employee.Id, leaveType.Id, 2025, allowance);
                balances.Add(balance);
            }
        }

        await context.LeaveBalances.AddRangeAsync(balances, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} leave balances", context.TenantInfo!.Identifier, balances.Count);
    }

    /// <summary>
    /// Seeds sample leave requests.
    /// </summary>
    private async Task SeedLeaveRequestsAsync(CancellationToken cancellationToken)
    {
        if (await context.LeaveRequests.AnyAsync(cancellationToken))
            return;

        var employees = await context.Employees.Take(5).ToListAsync(cancellationToken);
        var leaveTypes = await context.LeaveTypes.Take(3).ToListAsync(cancellationToken);

        if (employees.Count == 0 || leaveTypes.Count == 0) return;

        var leaveRequests = new List<LeaveRequest>();

        foreach (var employee in employees)
        {
            for (int i = 0; i < 2; i++)
            {
                var leaveType = leaveTypes[i];
                var startDate = DateTime.Today.AddDays(Random.Shared.Next(1, 30));
                var daysRequested = Random.Shared.Next(1, 5);
                var endDate = startDate.AddDays(daysRequested);

                var request = LeaveRequest.Create(employee.Id, leaveType.Id, startDate, endDate, "Personal leave request");

                if (Random.Shared.Next(2) == 0)
                    request.Approve("Approved by HR");

                leaveRequests.Add(request);
            }
        }

        await context.LeaveRequests.AddRangeAsync(leaveRequests, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} leave requests", context.TenantInfo!.Identifier, leaveRequests.Count);
    }

    /// <summary>
    /// Seeds sample company benefits.
    /// </summary>
    private async Task SeedBenefitsAsync(CancellationToken cancellationToken)
    {
        if (await context.Benefits.AnyAsync(cancellationToken))
            return;

        var benefits = new[]
        {
            Benefit.Create("Health Insurance", "Health", 500m, 1000m).SetCoverageDetails("Family", 500000m, "Maxicare").SetAsMandatory(false),
            Benefit.Create("Life Insurance", "Life", 100m, 200m).SetCoverageDetails("Individual", 1000000m, "Sunlife").SetAsMandatory(false),
            Benefit.Create("Dental Insurance", "Health", 50m, 100m).SetCoverageDetails("Family", 100000m, "Dental Plus").SetAsMandatory(false),
            Benefit.Create("Vision Insurance", "Health", 25m, 50m).SetCoverageDetails("Individual", 50000m, "EyeCare").SetAsMandatory(false),
            Benefit.Create("Rice Subsidy", "Allowance", 0m, 2000m).SetAsMandatory(false),
            Benefit.Create("Clothing Allowance", "Allowance", 0m, 3000m).SetAsMandatory(false),
            Benefit.Create("Laundry Allowance", "Allowance", 0m, 1000m).SetAsMandatory(false),
            Benefit.Create("CIMB Insurance", "Insurance", 150m, 300m).SetCoverageDetails("Individual", 500000m, "CIMB").SetAsMandatory(false),
            Benefit.Create("Educational Assistance", "Training", 0m, 50000m).SetAsMandatory(false),
            Benefit.Create("Birthday Gift", "Special", 0m, 5000m).SetAsMandatory(false)
        };

        await context.Benefits.AddRangeAsync(benefits, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} benefits", context.TenantInfo!.Identifier, benefits.Length);
    }

    /// <summary>
    /// Seeds benefit allocations per designation.
    /// </summary>
    private async Task SeedBenefitAllocationsAsync(CancellationToken cancellationToken)
    {
        if (await context.BenefitAllocations.AnyAsync(cancellationToken))
            return;

        var designations = await context.Designations.ToListAsync(cancellationToken);

        if (designations.Count == 0) return;

        var allocations = new List<BenefitAllocation>();

        foreach (var designation in designations)
        {
            var allocation = BenefitAllocation.Create(designation.Id, DateTime.Today.Date, 10000m, "Standard Benefit Allocation");
            allocations.Add(allocation);
        }

        await context.BenefitAllocations.AddRangeAsync(allocations, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} benefit allocations", context.TenantInfo!.Identifier, allocations.Count);
    }

    /// <summary>
    /// Seeds sample benefit enrollments.
    /// </summary>
    private async Task SeedBenefitEnrollmentsAsync(CancellationToken cancellationToken)
    {
        if (await context.BenefitEnrollments.AnyAsync(cancellationToken))
            return;

        var employees = await context.Employees.ToListAsync(cancellationToken);
        var benefits = await context.Benefits.ToListAsync(cancellationToken);

        if (employees.Count == 0 || benefits.Count == 0) return;

        var enrollments = new List<BenefitEnrollment>();

        foreach (var employee in employees)
        {
            var enrollmentCount = Random.Shared.Next(3, 6);
            var selectedBenefits = benefits.OrderBy(_ => Random.Shared.Next()).Take(enrollmentCount).ToList();

            foreach (var benefit in selectedBenefits)
            {
                var enrollmentStartDate = employee.HireDate ?? DateTime.Today.AddYears(-3);
                var enrollmentEndDate = enrollmentStartDate.AddYears(1);

                var enrollment = BenefitEnrollment.Create(employee.Id, benefit.Id, enrollmentStartDate, enrollmentEndDate);

                if (benefit.BenefitType == "Health" || benefit.BenefitType == "Insurance")
                    enrollment.SetCoverage("Family", benefit.EmployeeContribution, benefit.EmployerContribution);
                else
                    enrollment.SetCoverage("Individual", benefit.EmployeeContribution, benefit.EmployerContribution);

                enrollments.Add(enrollment);
            }
        }

        await context.BenefitEnrollments.AddRangeAsync(enrollments, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} benefit enrollments", context.TenantInfo!.Identifier, enrollments.Count);
    }

    /// <summary>
    /// Seeds sample timesheets.
    /// </summary>
    private async Task SeedTimesheetsAsync(CancellationToken cancellationToken)
    {
        if (await context.Timesheets.AnyAsync(cancellationToken))
            return;

        var employees = await context.Employees.ToListAsync(cancellationToken);
        if (employees.Count == 0) return;

        var timesheets = new List<Timesheet>();

        foreach (var employee in employees)
        {
            for (int weekOffset = 0; weekOffset < 3; weekOffset++)
            {
                var startDate = DateTime.Today.AddDays(-7 * (3 - weekOffset));
                var endDate = startDate.AddDays(6);
                
                var timesheet = Timesheet.Create(employee.Id, startDate, endDate);

                for (int day = 0; day < 5; day++)
                {
                    var workDate = startDate.AddDays(day);
                    
                    if (workDate.DayOfWeek == DayOfWeek.Saturday || workDate.DayOfWeek == DayOfWeek.Sunday)
                        continue;

                    var regularHours = 8.0m;
                    var overtimeHours = Random.Shared.Next(0, 3);
                    
                    var line = TimesheetLine.Create(timesheet.Id, workDate, regularHours: regularHours, overtimeHours: (decimal)overtimeHours, projectId: null, taskDescription: $"Work on project - Day {day + 1}");
                    timesheet.AddLine(line);
                }

                timesheet.Submit();
                if (Random.Shared.Next(2) == 0)
                    timesheet.Approve("Approved - Sample data");

                timesheets.Add(timesheet);
            }
        }

        await context.Timesheets.AddRangeAsync(timesheets, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} timesheets with {LineCount} total lines", context.TenantInfo!.Identifier, timesheets.Count, timesheets.Sum(t => t.Lines.Count));
    }

    /// <summary>
    /// Seeds pay components.
    /// </summary>
    private async Task SeedPayComponentsAsync(CancellationToken cancellationToken)
    {
        if (await context.PayComponents.AnyAsync(x => x.Code == "BASIC_PAY_DEMO", cancellationToken))
            return;

        var components = new[]
        {
            PayComponent.Create("BASIC_PAY_DEMO", "Basic Pay", "Earnings", "Manual", "6100").Update("Regular salary", displayOrder: 1).SetAutoCalculated(false).SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false).SetPayImpact(affectsGrossPay: true, affectsNetPay: true),
            PayComponent.Create("ALLOWANCE_DEMO", "Allowances", "Earnings", "Manual", "6101").Update("Meal, transportation allowances", displayOrder: 2).SetAutoCalculated(false).SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false).SetPayImpact(affectsGrossPay: true, affectsNetPay: true),
            PayComponent.Create("BONUS_DEMO", "Performance Bonus", "Earnings", "Manual", "6102").Update("Performance-based bonus", displayOrder: 3).SetAutoCalculated(false).SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false).SetPayImpact(affectsGrossPay: true, affectsNetPay: true)
        };

        await context.PayComponents.AddRangeAsync(components, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} pay components", context.TenantInfo!.Identifier, components.Length);
    }

    /// <summary>
    /// Seeds sample payroll records.
    /// </summary>
    private async Task SeedPayrollsAsync(CancellationToken cancellationToken)
    {
        if (await context.Payrolls.AnyAsync(cancellationToken))
            return;

        var employees = await context.Employees.ToListAsync(cancellationToken);
        var payComponents = await context.PayComponents.Where(x => x.Code.Contains("DEMO")).ToListAsync(cancellationToken);

        if (employees.Count == 0 || payComponents.Count == 0) return;

        var payrolls = new List<Payroll>();

        for (int monthOffset = 0; monthOffset < 2; monthOffset++)
        {
            var now = DateTime.Today;
            var paymentDate = new DateTime(now.Year, now.Month, 15, 0, 0, 0, DateTimeKind.Utc).AddMonths(-monthOffset);
            var startDate = new DateTime(paymentDate.Year, paymentDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            foreach (var employee in employees)
            {
                var salary = 40000m;
                var payroll = Payroll.Create(startDate, paymentDate, $"Payroll-{paymentDate:yyyy-MM}");

                var basicPayComponent = payComponents.FirstOrDefault(x => x.Code == "BASIC_PAY_DEMO");
                if (basicPayComponent != null)
                {
                    var payrollLine = PayrollLine.Create(payroll.Id, basicPayComponent.Id, 8, salary);
                    payroll.AddLine(payrollLine);
                }

                if (Random.Shared.Next(100) < 30)
                {
                    var bonusComponent = payComponents.FirstOrDefault(x => x.Code == "BONUS_DEMO");
                    if (bonusComponent != null)
                    {
                        var bonus = salary * 0.15m;
                        var payrollLine = PayrollLine.Create(payroll.Id, bonusComponent.Id, 8, bonus);
                        payroll.AddLine(payrollLine);
                    }
                }

                payrolls.Add(payroll);
            }
        }

        await context.Payrolls.AddRangeAsync(payrolls, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} payroll records", context.TenantInfo!.Identifier, payrolls.Count);
    }

    /// <summary>
    /// Seeds document templates.
    /// </summary>
    private async Task SeedDocumentTemplatesAsync(CancellationToken cancellationToken)
    {
        if (await context.DocumentTemplates.AnyAsync(cancellationToken))
            return;

        var templates = new[]
        {
            DocumentTemplate.Create("Employment Certificate", "Certificate", @"This is to certify that {{EmployeeName}} has been employed with {{CompanyName}} since {{HireDate}} as {{Designation}}."),
            DocumentTemplate.Create("Certificate of Employment", "Certificate", @"TO WHOM IT MAY CONCERN:

This is to certify that {{EmployeeName}} is currently employed with {{CompanyName}} as {{Designation}} since {{HireDate}}.

Issued this {{IssueDate}} at {{Location}}."),
            DocumentTemplate.Create("Payslip", "Payroll", @"PAYSLIP for {{PayPeriod}}
Employee: {{EmployeeName}} ({{EmployeeNumber}})
Basic Pay: {{BasicPay}}
Deductions: {{Deductions}}
Net Pay: {{NetPay}}")
        };

        await context.DocumentTemplates.AddRangeAsync(templates, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} document templates", context.TenantInfo!.Identifier, templates.Length);
    }

    /// <summary>
    /// Seeds Philippine tax brackets for 2025.
    /// </summary>
    private async Task SeedTaxBracketsAsync(CancellationToken cancellationToken)
    {
        if (await context.TaxBrackets.AnyAsync(cancellationToken))
            return;

        var taxBrackets = new[]
        {
            TaxBracket.Create("Individual Income Tax", 2025, 0m, 250000m, 0m),
            TaxBracket.Create("Individual Income Tax", 2025, 250000m, 400000m, 0.15m),
            TaxBracket.Create("Individual Income Tax", 2025, 400000m, 800000m, 0.20m),
            TaxBracket.Create("Individual Income Tax", 2025, 800000m, 2000000m, 0.25m),
            TaxBracket.Create("Individual Income Tax", 2025, 2000000m, 8000000m, 0.30m),
            TaxBracket.Create("Individual Income Tax", 2025, 8000000m, decimal.MaxValue, 0.35m)
        };

        await context.TaxBrackets.AddRangeAsync(taxBrackets, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} tax brackets", context.TenantInfo!.Identifier, taxBrackets.Length);
    }

    /// <summary>
    /// Seeds Philippine tax master data (SSS, PhilHealth, Pag-IBIG).
    /// </summary>
    private async Task SeedTaxMastersAsync(CancellationToken cancellationToken)
    {
        if (await context.TaxMasters.AnyAsync(cancellationToken))
            return;

        var taxMasters = new[]
        {
            TaxMaster.Create("SSS_EE_2025", "Social Security System (SSS) - Employee", "Withholding", 0.0363m, DefaultIdType.Empty),
            TaxMaster.Create("PHILHEALTH_EE_2025", "PhilHealth - Employee", "Withholding", 0.0275m, DefaultIdType.Empty),
            TaxMaster.Create("PAGIBIG_EE_2025", "Pag-IBIG (HDMF) - Employee", "Withholding", 0.0100m, DefaultIdType.Empty),
            TaxMaster.Create("BIR_WITHHOLDING_2025", "Bureau of Internal Revenue (BIR)", "Withholding", 0m, DefaultIdType.Empty)
        };

        await context.TaxMasters.AddRangeAsync(taxMasters, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} tax masters", context.TenantInfo!.Identifier, taxMasters.Length);
    }

    /// <summary>
    /// Seeds pay component rates for tax and allowance calculations.
    /// </summary>
    private async Task SeedPayComponentRatesAsync(CancellationToken cancellationToken)
    {
        if (await context.PayComponentRates.AnyAsync(cancellationToken))
            return;

        var payComponents = await context.PayComponents.Where(x => x.Code.Contains("DEMO")).ToListAsync(cancellationToken);
        if (payComponents.Count == 0) return;

        var payComponentRates = new List<PayComponentRate>();
        var startDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Get the basic pay component
        var basicPayComponent = payComponents.FirstOrDefault(x => x.Code == "BASIC_PAY_DEMO");
        if (basicPayComponent != null)
        {
            payComponentRates.Add(PayComponentRate.Create(basicPayComponent.Id, 0m, 1000000m, startDate));
        }

        // Get the allowance component
        var allowanceComponent = payComponents.FirstOrDefault(x => x.Code == "ALLOWANCE_DEMO");
        if (allowanceComponent != null)
        {
            payComponentRates.Add(PayComponentRate.Create(allowanceComponent.Id, 0m, 1000000m, startDate));
        }

        // Get the bonus component
        var bonusComponent = payComponents.FirstOrDefault(x => x.Code == "BONUS_DEMO");
        if (bonusComponent != null)
        {
            payComponentRates.Add(PayComponentRate.Create(bonusComponent.Id, 0m, 1000000m, startDate));
        }

        if (payComponentRates.Count > 0)
        {
            await context.PayComponentRates.AddRangeAsync(payComponentRates, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        logger.LogInformation("[{Tenant}] seeded {Count} pay component rates", context.TenantInfo!.Identifier, payComponentRates.Count);
    }

    /// <summary>
    /// Seeds payroll deduction configurations.
    /// </summary>
    private async Task SeedPayrollDeductionsAsync(CancellationToken cancellationToken)
    {
        if (await context.PayrollDeductions.AnyAsync(cancellationToken))
            return;

        var deductions = new List<PayrollDeduction>();

        // SSS deduction
        deductions.Add(PayrollDeduction.Create(DefaultIdType.Empty, "SSS Contribution", 0m, 0.0363m));

        // PhilHealth deduction
        deductions.Add(PayrollDeduction.Create(DefaultIdType.Empty, "PhilHealth Contribution", 0m, 0.0275m));

        // Pag-IBIG deduction
        deductions.Add(PayrollDeduction.Create(DefaultIdType.Empty, "Pag-IBIG Contribution", 0m, 0.0100m));

        // Loan deduction
        deductions.Add(PayrollDeduction.Create(DefaultIdType.Empty, "Employee Loan", 2000m, 0m));

        // Health insurance deduction
        deductions.Add(PayrollDeduction.Create(DefaultIdType.Empty, "Health Insurance", 1000m, 0m));

        await context.PayrollDeductions.AddRangeAsync(deductions, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} payroll deductions", context.TenantInfo!.Identifier, deductions.Count);
    }

    /// <summary>
    /// Seeds shift break configurations.
    /// </summary>
    private async Task SeedShiftBreaksAsync(CancellationToken cancellationToken)
    {
        if (await context.ShiftBreaks.AnyAsync(cancellationToken))
            return;

        var shifts = await context.Shifts.ToListAsync(cancellationToken);
        if (shifts.Count == 0) return;

        var breaks = new List<ShiftBreak>();

        foreach (var shift in shifts)
        {
            // Add one break per shift (typically at midday)
            breaks.Add(ShiftBreak.Create(shift.Id, new TimeSpan(12, 0, 0), new TimeSpan(13, 0, 0), "Lunch Break"));

            // Add optional second break for longer shifts
            if (shift.EndTime.Subtract(shift.StartTime).TotalHours >= 8)
            {
                breaks.Add(ShiftBreak.Create(shift.Id, new TimeSpan(15, 30, 0), new TimeSpan(15, 45, 0), "Afternoon Break"));
            }
        }

        await context.ShiftBreaks.AddRangeAsync(breaks, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} shift breaks", context.TenantInfo!.Identifier, breaks.Count);
    }

    /// <summary>
    /// Seeds sample employee documents.
    /// </summary>
    private async Task SeedEmployeeDocumentsAsync(CancellationToken cancellationToken)
    {
        if (await context.EmployeeDocuments.AnyAsync(cancellationToken))
            return;

        var employees = await context.Employees.ToListAsync(cancellationToken);
        if (employees.Count == 0) return;

        var documents = new List<EmployeeDocument>();

        foreach (var employee in employees)
        {
            // Employment contract
            documents.Add(EmployeeDocument.Create(
                employee.Id,
                "Contract",
                "Employment Contract",
                $"contract_{employee.Id}.pdf",
                $"/documents/contracts/{employee.Id}.pdf",
                50000,
                null));

            // Government ID copy
            documents.Add(EmployeeDocument.Create(
                employee.Id,
                "Identity",
                "Government ID - SSS",
                $"sss_id_{employee.Id}.pdf",
                $"/documents/ids/{employee.Id}_sss.pdf",
                35000,
                DateTime.Today.AddYears(3)));

            // Professional certification
            if (Random.Shared.Next(3) == 0)
            {
                documents.Add(EmployeeDocument.Create(
                    employee.Id,
                    "Certification",
                    "Professional Certification",
                    $"cert_{employee.Id}.pdf",
                    $"/documents/certifications/{employee.Id}.pdf",
                    45000,
                    DateTime.Today.AddYears(2)));
            }
        }

        await context.EmployeeDocuments.AddRangeAsync(documents, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} employee documents", context.TenantInfo!.Identifier, documents.Count);
    }

    /// <summary>
    /// Seeds sample performance reviews.
    /// </summary>
    private async Task SeedPerformanceReviewsAsync(CancellationToken cancellationToken)
    {
        if (await context.PerformanceReviews.AnyAsync(cancellationToken))
            return;

        var employees = await context.Employees.ToListAsync(cancellationToken);
        if (employees.Count < 2) return;

        var reviews = new List<PerformanceReview>();

        // Get some employees to act as reviewers (managers)
        var reviewers = employees.Take(3).ToList();
        var reviewees = employees.Skip(1).ToList();

        foreach (var reviewee in reviewees)
        {
            // Assign a random reviewer
            var reviewer = reviewers[Random.Shared.Next(reviewers.Count)];

            // Create review for H1 2025
            var review = PerformanceReview.Create(
                reviewee.Id,
                reviewer.Id,
                new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2025, 6, 30, 0, 0, 0, DateTimeKind.Utc),
                "Half-Year Review");

            reviews.Add(review);
        }

        await context.PerformanceReviews.AddRangeAsync(reviews, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} performance reviews", context.TenantInfo!.Identifier, reviews.Count);
    }
}

