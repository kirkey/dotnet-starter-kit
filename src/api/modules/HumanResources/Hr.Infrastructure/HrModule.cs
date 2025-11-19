// ensure ArgumentNullException available
using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Attendance;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.AttendanceReports;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BankAccounts;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitAllocations;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitEnrollments;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Benefits; // add benefits endpoints
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Deductions; // add deductions endpoints
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DesignationAssignments;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Designations;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DocumentTemplates;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeContacts;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDashboards;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDocuments;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Employees;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.HRAnalytics;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDependents;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeEducations;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeePayComponents;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.GeneratedDocuments;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Holidays;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveBalances;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveRequests;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveReports;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveTypes;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.OrganizationalUnits;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollReports;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Payrolls;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponentRates;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponents;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PerformanceReviews;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollLines;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollDeductions;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.ShiftAssignments;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Shifts;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Taxes;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TaxBrackets;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TimesheetLines;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Timesheets;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.GeneratedDocuments;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Holidays;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponents;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Shifts;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure;

/// <summary>
/// HumanResources module registration and configuration.
/// </summary>
public static class HrModule
{
    /// <summary>
    /// Carter module for HumanResources endpoints.
    /// </summary>
    public class Endpoints() : CarterModule("humanresources")
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapOrganizationalUnitsEndpoints();
            app.MapPayrollReportsEndpoints();
            app.MapPayrollsEndpoints();
            app.MapEmployeesEndpoints();
            app.MapDesignationsEndpoints();
            app.MapDesignationAssignmentsEndpoints();
            app.MapEmployeeContactsEndpoints();
            app.MapEmployeeDependentsEndpoints();
            app.MapEmployeeDocumentsEndpoints();
            app.MapEmployeeEducationsEndpoints();
            app.MapAttendanceEndpoints();
            app.MapAttendanceReportsEndpoints();
            app.MapBankAccountsEndpoints();
            app.MapBenefitEnrollmentsEndpoints();
            app.MapBenefitAllocationsEndpoints();
            app.MapBenefitEndpoints();
            app.MapDeductionEndpoints();
            app.MapTimesheetsEndpoints();
            app.MapTimesheetLinesEndpoints();
            app.MapLeaveTypesEndpoints();
            app.MapLeaveBalancesEndpoints();
            app.MapLeaveRequestsEndpoints();
            app.MapLeaveReportsEndpoints();
            app.MapShiftAssignmentEndpoints();
            app.MapDocumentTemplatesEndpoints();
            app.MapEmployeeDashboardsEndpoints();
            // app.MapHrAnalyticsEndpoints();
            app.MapPayComponentRatesEndpoints();
            app.MapEmployeePayComponentsEndpoints();
            app.MapPerformanceReviewsEndpoints();
            app.MapPayrollLinesEndpoints();
            app.MapPayrollDeductionsEndpoints();
            app.MapTaxBracketEndpoints();
            app.MapTaxEndpoints();
            app.MapGeneratedDocumentsEndpoints();
            app.MapHolidaysEndpoints();
            app.MapPayComponentsEndpoints();
            app.MapShiftsEndpoints();
        }
    }

    /// <summary>
    /// Registers HumanResources services.
    /// </summary>
    public static WebApplicationBuilder RegisterHumanResourcesServices(this WebApplicationBuilder builder)
    {
        System.ArgumentNullException.ThrowIfNull(builder);

        // Register DbContext
        builder.Services.BindDbContext<HrDbContext>();

        // Register Database Initializer
        builder.Services.AddScoped<IDbInitializer, HrDbInitializer>();

        // Register Repositories with keyed services

        builder.Services.AddKeyedScoped<IRepository<OrganizationalUnit>, HrRepository<OrganizationalUnit>>("hr:organizationalunits");
        builder.Services.AddKeyedScoped<IReadRepository<OrganizationalUnit>, HrRepository<OrganizationalUnit>>("hr:organizationalunits");

        builder.Services.AddKeyedScoped<IRepository<Designation>, HrRepository<Designation>>("hr:designations");
        builder.Services.AddKeyedScoped<IReadRepository<Designation>, HrRepository<Designation>>("hr:designations");

        builder.Services.AddKeyedScoped<IRepository<Employee>, HrRepository<Employee>>("hr:employees");
        builder.Services.AddKeyedScoped<IReadRepository<Employee>, HrRepository<Employee>>("hr:employees");

        builder.Services.AddKeyedScoped<IRepository<DesignationAssignment>, HrRepository<DesignationAssignment>>("hr:designationassignments");
        builder.Services.AddKeyedScoped<IReadRepository<DesignationAssignment>, HrRepository<DesignationAssignment>>("hr:designationassignments");

        builder.Services.AddKeyedScoped<IRepository<EmployeeContact>, HrRepository<EmployeeContact>>("hr:contacts");
        builder.Services.AddKeyedScoped<IReadRepository<EmployeeContact>, HrRepository<EmployeeContact>>("hr:contacts");

        builder.Services.AddKeyedScoped<IRepository<EmployeeContact>, HrRepository<EmployeeContact>>("hr:employeecontacts");
        builder.Services.AddKeyedScoped<IReadRepository<EmployeeContact>, HrRepository<EmployeeContact>>("hr:employeecontacts");

        builder.Services.AddKeyedScoped<IRepository<EmployeeDependent>, HrRepository<EmployeeDependent>>("hr:dependents");
        builder.Services.AddKeyedScoped<IReadRepository<EmployeeDependent>, HrRepository<EmployeeDependent>>("hr:dependents");

        builder.Services.AddKeyedScoped<IRepository<EmployeeDocument>, HrRepository<EmployeeDocument>>("hr:documents");
        builder.Services.AddKeyedScoped<IReadRepository<EmployeeDocument>, HrRepository<EmployeeDocument>>("hr:documents");

        builder.Services.AddKeyedScoped<IRepository<EmployeeEducation>, HrRepository<EmployeeEducation>>("hr:employeeeducations");
        builder.Services.AddKeyedScoped<IReadRepository<EmployeeEducation>, HrRepository<EmployeeEducation>>("hr:employeeeducations");

        builder.Services.AddKeyedScoped<IRepository<Attendance>, HrRepository<Attendance>>("hr:attendance");
        builder.Services.AddKeyedScoped<IReadRepository<Attendance>, HrRepository<Attendance>>("hr:attendance");

        builder.Services.AddKeyedScoped<IRepository<BankAccount>, HrRepository<BankAccount>>("hr:bankaccounts");
        builder.Services.AddKeyedScoped<IReadRepository<BankAccount>, HrRepository<BankAccount>>("hr:bankaccounts");

        builder.Services.AddKeyedScoped<IRepository<Timesheet>, HrRepository<Timesheet>>("hr:timesheets");
        builder.Services.AddKeyedScoped<IReadRepository<Timesheet>, HrRepository<Timesheet>>("hr:timesheets");

        builder.Services.AddKeyedScoped<IRepository<TimesheetLine>, HrRepository<TimesheetLine>>("hr:timesheetlines");
        builder.Services.AddKeyedScoped<IReadRepository<TimesheetLine>, HrRepository<TimesheetLine>>("hr:timesheetlines");

        builder.Services.AddKeyedScoped<IRepository<Shift>, HrRepository<Shift>>("hr:shifts");
        builder.Services.AddKeyedScoped<IReadRepository<Shift>, HrRepository<Shift>>("hr:shifts");

        builder.Services.AddKeyedScoped<IRepository<ShiftAssignment>, HrRepository<ShiftAssignment>>("hr:shiftassignments");
        builder.Services.AddKeyedScoped<IReadRepository<ShiftAssignment>, HrRepository<ShiftAssignment>>("hr:shiftassignments");

        builder.Services.AddKeyedScoped<IRepository<LeaveType>, HrRepository<LeaveType>>("hr:leavetypes");
        builder.Services.AddKeyedScoped<IReadRepository<LeaveType>, HrRepository<LeaveType>>("hr:leavetypes");

        builder.Services.AddKeyedScoped<IRepository<LeaveBalance>, HrRepository<LeaveBalance>>("hr:leavebalances");
        builder.Services.AddKeyedScoped<IReadRepository<LeaveBalance>, HrRepository<LeaveBalance>>("hr:leavebalances");

        builder.Services.AddKeyedScoped<IRepository<LeaveRequest>, HrRepository<LeaveRequest>>("hr:leaverequests");
        builder.Services.AddKeyedScoped<IReadRepository<LeaveRequest>, HrRepository<LeaveRequest>>("hr:leaverequests");

        builder.Services.AddKeyedScoped<IRepository<Holiday>, HrRepository<Holiday>>("hr:holidays");
        builder.Services.AddKeyedScoped<IReadRepository<Holiday>, HrRepository<Holiday>>("hr:holidays");

        builder.Services.AddKeyedScoped<IRepository<Payroll>, HrRepository<Payroll>>("hr:payrolls");
        builder.Services.AddKeyedScoped<IReadRepository<Payroll>, HrRepository<Payroll>>("hr:payrolls");

        builder.Services.AddKeyedScoped<IRepository<PayrollLine>, HrRepository<PayrollLine>>("hr:payrolllines");
        builder.Services.AddKeyedScoped<IReadRepository<PayrollLine>, HrRepository<PayrollLine>>("hr:payrolllines");

        // Fix: align key prefix with convention 'hr:' instead of 'humanresources:'
        builder.Services.AddKeyedScoped<IRepository<PayrollDeduction>, HrRepository<PayrollDeduction>>("hr:payrolldeductions");
        builder.Services.AddKeyedScoped<IReadRepository<PayrollDeduction>, HrRepository<PayrollDeduction>>("hr:payrolldeductions");

        // Add alias for handlers that use humanresources:payrolldeductions key
        builder.Services.AddKeyedScoped<IRepository<PayrollDeduction>, HrRepository<PayrollDeduction>>("humanresources:payrolldeductions");
        builder.Services.AddKeyedScoped<IReadRepository<PayrollDeduction>, HrRepository<PayrollDeduction>>("humanresources:payrolldeductions");

        builder.Services.AddKeyedScoped<IRepository<PayComponent>, HrRepository<PayComponent>>("hr:paycomponents");
        builder.Services.AddKeyedScoped<IReadRepository<PayComponent>, HrRepository<PayComponent>>("hr:paycomponents");

        // New: Deduction repositories to support deductions master endpoints
        builder.Services.AddKeyedScoped<IRepository<Deduction>, HrRepository<Deduction>>("hr:deductions");
        builder.Services.AddKeyedScoped<IReadRepository<Deduction>, HrRepository<Deduction>>("hr:deductions");

        builder.Services.AddKeyedScoped<IRepository<PayComponentRate>, HrRepository<PayComponentRate>>("hr:paycomponentrates");
        builder.Services.AddKeyedScoped<IReadRepository<PayComponentRate>, HrRepository<PayComponentRate>>("hr:paycomponentrates");

        // Add alias for handlers that use humanresources:paycomponentrates key
        builder.Services.AddKeyedScoped<IRepository<PayComponentRate>, HrRepository<PayComponentRate>>("humanresources:paycomponentrates");
        builder.Services.AddKeyedScoped<IReadRepository<PayComponentRate>, HrRepository<PayComponentRate>>("humanresources:paycomponentrates");

        builder.Services.AddKeyedScoped<IRepository<EmployeePayComponent>, HrRepository<EmployeePayComponent>>("hr:employeepaycomponents");
        builder.Services.AddKeyedScoped<IReadRepository<EmployeePayComponent>, HrRepository<EmployeePayComponent>>("hr:employeepaycomponents");

        builder.Services.AddKeyedScoped<IRepository<TaxBracket>, HrRepository<TaxBracket>>("hr:taxbrackets");
        builder.Services.AddKeyedScoped<IReadRepository<TaxBracket>, HrRepository<TaxBracket>>("hr:taxbrackets");

        // Add TaxMaster repositories for new Tax Master Configuration endpoints
        builder.Services.AddKeyedScoped<IRepository<TaxMaster>, HrRepository<TaxMaster>>("hr:taxes");
        builder.Services.AddKeyedScoped<IReadRepository<TaxMaster>, HrRepository<TaxMaster>>("hr:taxes");
        
        builder.Services.AddKeyedScoped<IRepository<Benefit>, HrRepository<Benefit>>("hr:benefits");
        builder.Services.AddKeyedScoped<IReadRepository<Benefit>, HrRepository<Benefit>>("hr:benefits");

        builder.Services.AddKeyedScoped<IRepository<BenefitEnrollment>, HrRepository<BenefitEnrollment>>("hr:benefitenrollments");
        builder.Services.AddKeyedScoped<IReadRepository<BenefitEnrollment>, HrRepository<BenefitEnrollment>>("hr:benefitenrollments");

        // Add alias for Enrollments endpoints that use hr:enrollments key
        builder.Services.AddKeyedScoped<IRepository<BenefitEnrollment>, HrRepository<BenefitEnrollment>>("hr:enrollments");
        builder.Services.AddKeyedScoped<IReadRepository<BenefitEnrollment>, HrRepository<BenefitEnrollment>>("hr:enrollments");

        // New: BenefitAllocation repositories to support endpoints
        builder.Services.AddKeyedScoped<IRepository<BenefitAllocation>, HrRepository<BenefitAllocation>>("hr:benefitallocations");
        builder.Services.AddKeyedScoped<IReadRepository<BenefitAllocation>, HrRepository<BenefitAllocation>>("hr:benefitallocations");


        // New: PayrollReport repositories to support payroll reports endpoints
        builder.Services.AddKeyedScoped<IRepository<PayrollReport>, HrRepository<PayrollReport>>("hr:payrollreports");
        builder.Services.AddKeyedScoped<IReadRepository<PayrollReport>, HrRepository<PayrollReport>>("hr:payrollreports");

        // New: AttendanceReport repositories to support attendance reports endpoints
        builder.Services.AddKeyedScoped<IRepository<AttendanceReport>, HrRepository<AttendanceReport>>("hr:attendancereports");
        builder.Services.AddKeyedScoped<IReadRepository<AttendanceReport>, HrRepository<AttendanceReport>>("hr:attendancereports");

        // New: LeaveReport repositories to support leave reports endpoints
        builder.Services.AddKeyedScoped<IRepository<LeaveReport>, HrRepository<LeaveReport>>("hr:leavereports");
        builder.Services.AddKeyedScoped<IReadRepository<LeaveReport>, HrRepository<LeaveReport>>("hr:leavereports");


        builder.Services.AddKeyedScoped<IRepository<DocumentTemplate>, HrRepository<DocumentTemplate>>("hr:documenttemplates");
        builder.Services.AddKeyedScoped<IReadRepository<DocumentTemplate>, HrRepository<DocumentTemplate>>("hr:documenttemplates");

        builder.Services.AddKeyedScoped<IRepository<GeneratedDocument>, HrRepository<GeneratedDocument>>("hr:generateddocuments");
        builder.Services.AddKeyedScoped<IReadRepository<GeneratedDocument>, HrRepository<GeneratedDocument>>("hr:generateddocuments");

        // New: PerformanceReview repositories to support endpoints
        builder.Services.AddKeyedScoped<IRepository<PerformanceReview>, HrRepository<PerformanceReview>>("hr:performancereviews");
        builder.Services.AddKeyedScoped<IReadRepository<PerformanceReview>, HrRepository<PerformanceReview>>("hr:performancereviews");

        return builder;
    }

    /// <summary>
    /// Configures HumanResources module middleware.
    /// </summary>
    public static WebApplication UseHumanResourcesModule(this WebApplication app)
    {
        return app;
    }
}
