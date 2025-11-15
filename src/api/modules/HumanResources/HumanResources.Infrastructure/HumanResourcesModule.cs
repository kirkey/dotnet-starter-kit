using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Attendance;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Designations;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DesignationAssignments;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DocumentTemplates;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeContacts;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDependents;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDocuments;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Employees;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeePayComponents;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.GeneratedDocuments;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveBalances;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveTypes;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.OrganizationalUnits;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponentRates;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponents;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollDeductions;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.ShiftAssignments;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Shifts;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TaxBrackets;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TimesheetLines;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Timesheets;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure;

/// <summary>
/// HumanResources module registration and configuration.
/// </summary>
public static class HumanResourcesModule
{
    /// <summary>
    /// Carter module for HumanResources endpoints.
    /// </summary>
    public class Endpoints() : CarterModule("humanresources")
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapOrganizationalUnitsEndpoints();
            app.MapDesignationsEndpoints();
            app.MapEmployeesEndpoints();
            app.MapDesignationAssignmentsEndpoints();
            app.MapEmployeeContactsEndpoints();
            app.MapEmployeeDependentsEndpoints();
            app.MapEmployeeDocumentsEndpoints();
            app.MapAttendanceEndpoints();
            app.MapTimesheetsEndpoints();
            app.MapTimesheetLinesEndpoints();
            app.MapLeaveTypesEndpoints();
            app.MapLeaveBalancesEndpoints();
            app.MapShiftsEndpoints();
            app.MapShiftAssignmentEndpoints();
            app.MapDocumentTemplatesEndpoints();
            app.MapGeneratedDocumentsEndpoints();
            app.MapPayComponentsEndpoints();
            app.MapPayComponentRatesEndpoints();
            app.MapEmployeePayComponentsEndpoints();
            app.MapPayrollDeductionsEndpoints();
            app.MapTaxBracketEndpoints();
        }
    }

    /// <summary>
    /// Registers HumanResources services.
    /// </summary>
    public static WebApplicationBuilder RegisterHumanResourcesServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Register DbContext
        builder.Services.BindDbContext<HumanResourcesDbContext>();

        // Register Database Initializer
        builder.Services.AddScoped<IDbInitializer, HumanResourcesDbInitializer>();

        // Register Repositories with keyed services

        builder.Services.AddKeyedScoped<IRepository<OrganizationalUnit>, HumanResourcesRepository<OrganizationalUnit>>("hr:organizationalunits");
        builder.Services.AddKeyedScoped<IReadRepository<OrganizationalUnit>, HumanResourcesRepository<OrganizationalUnit>>("hr:organizationalunits");

        builder.Services.AddKeyedScoped<IRepository<Designation>, HumanResourcesRepository<Designation>>("hr:designations");
        builder.Services.AddKeyedScoped<IReadRepository<Designation>, HumanResourcesRepository<Designation>>("hr:designations");

        builder.Services.AddKeyedScoped<IRepository<Employee>, HumanResourcesRepository<Employee>>("hr:employees");
        builder.Services.AddKeyedScoped<IReadRepository<Employee>, HumanResourcesRepository<Employee>>("hr:employees");

        builder.Services.AddKeyedScoped<IRepository<DesignationAssignment>, HumanResourcesRepository<DesignationAssignment>>("hr:designationassignments");
        builder.Services.AddKeyedScoped<IReadRepository<DesignationAssignment>, HumanResourcesRepository<DesignationAssignment>>("hr:designationassignments");

        builder.Services.AddKeyedScoped<IRepository<EmployeeContact>, HumanResourcesRepository<EmployeeContact>>("hr:contacts");
        builder.Services.AddKeyedScoped<IReadRepository<EmployeeContact>, HumanResourcesRepository<EmployeeContact>>("hr:contacts");

        builder.Services.AddKeyedScoped<IRepository<EmployeeDependent>, HumanResourcesRepository<EmployeeDependent>>("hr:dependents");
        builder.Services.AddKeyedScoped<IReadRepository<EmployeeDependent>, HumanResourcesRepository<EmployeeDependent>>("hr:dependents");

        builder.Services.AddKeyedScoped<IRepository<EmployeeDocument>, HumanResourcesRepository<EmployeeDocument>>("hr:documents");
        builder.Services.AddKeyedScoped<IReadRepository<EmployeeDocument>, HumanResourcesRepository<EmployeeDocument>>("hr:documents");

        builder.Services.AddKeyedScoped<IRepository<Attendance>, HumanResourcesRepository<Attendance>>("hr:attendance");
        builder.Services.AddKeyedScoped<IReadRepository<Attendance>, HumanResourcesRepository<Attendance>>("hr:attendance");

        builder.Services.AddKeyedScoped<IRepository<Timesheet>, HumanResourcesRepository<Timesheet>>("hr:timesheets");
        builder.Services.AddKeyedScoped<IReadRepository<Timesheet>, HumanResourcesRepository<Timesheet>>("hr:timesheets");

        builder.Services.AddKeyedScoped<IRepository<TimesheetLine>, HumanResourcesRepository<TimesheetLine>>("hr:timesheetlines");
        builder.Services.AddKeyedScoped<IReadRepository<TimesheetLine>, HumanResourcesRepository<TimesheetLine>>("hr:timesheetlines");

        builder.Services.AddKeyedScoped<IRepository<Shift>, HumanResourcesRepository<Shift>>("hr:shifts");
        builder.Services.AddKeyedScoped<IReadRepository<Shift>, HumanResourcesRepository<Shift>>("hr:shifts");

        builder.Services.AddKeyedScoped<IRepository<ShiftAssignment>, HumanResourcesRepository<ShiftAssignment>>("hr:shiftassignments");
        builder.Services.AddKeyedScoped<IReadRepository<ShiftAssignment>, HumanResourcesRepository<ShiftAssignment>>("hr:shiftassignments");

        builder.Services.AddKeyedScoped<IRepository<LeaveType>, HumanResourcesRepository<LeaveType>>("hr:leavetypes");
        builder.Services.AddKeyedScoped<IReadRepository<LeaveType>, HumanResourcesRepository<LeaveType>>("hr:leavetypes");

        builder.Services.AddKeyedScoped<IRepository<LeaveBalance>, HumanResourcesRepository<LeaveBalance>>("hr:leavebalances");
        builder.Services.AddKeyedScoped<IReadRepository<LeaveBalance>, HumanResourcesRepository<LeaveBalance>>("hr:leavebalances");

        builder.Services.AddKeyedScoped<IRepository<LeaveRequest>, HumanResourcesRepository<LeaveRequest>>("hr:leaverequests");
        builder.Services.AddKeyedScoped<IReadRepository<LeaveRequest>, HumanResourcesRepository<LeaveRequest>>("hr:leaverequests");

        builder.Services.AddKeyedScoped<IRepository<Holiday>, HumanResourcesRepository<Holiday>>("hr:holidays");
        builder.Services.AddKeyedScoped<IReadRepository<Holiday>, HumanResourcesRepository<Holiday>>("hr:holidays");

        builder.Services.AddKeyedScoped<IRepository<Payroll>, HumanResourcesRepository<Payroll>>("hr:payrolls");
        builder.Services.AddKeyedScoped<IReadRepository<Payroll>, HumanResourcesRepository<Payroll>>("hr:payrolls");

        builder.Services.AddKeyedScoped<IRepository<PayrollLine>, HumanResourcesRepository<PayrollLine>>("hr:payrolllines");
        builder.Services.AddKeyedScoped<IReadRepository<PayrollLine>, HumanResourcesRepository<PayrollLine>>("hr:payrolllines");

        builder.Services.AddKeyedScoped<IRepository<PayrollDeduction>, HumanResourcesRepository<PayrollDeduction>>("humanresources:payrolldeductions");
        builder.Services.AddKeyedScoped<IReadRepository<PayrollDeduction>, HumanResourcesRepository<PayrollDeduction>>("humanresources:payrolldeductions");

        builder.Services.AddKeyedScoped<IRepository<PayComponent>, HumanResourcesRepository<PayComponent>>("hr:paycomponents");
        builder.Services.AddKeyedScoped<IReadRepository<PayComponent>, HumanResourcesRepository<PayComponent>>("hr:paycomponents");

        builder.Services.AddKeyedScoped<IRepository<PayComponentRate>, HumanResourcesRepository<PayComponentRate>>("hr:paycomponentrates");
        builder.Services.AddKeyedScoped<IReadRepository<PayComponentRate>, HumanResourcesRepository<PayComponentRate>>("hr:paycomponentrates");

        builder.Services.AddKeyedScoped<IRepository<EmployeePayComponent>, HumanResourcesRepository<EmployeePayComponent>>("hr:employeepaycomponents");
        builder.Services.AddKeyedScoped<IReadRepository<EmployeePayComponent>, HumanResourcesRepository<EmployeePayComponent>>("hr:employeepaycomponents");

        builder.Services.AddKeyedScoped<IRepository<TaxBracket>, HumanResourcesRepository<TaxBracket>>("hr:taxbrackets");
        builder.Services.AddKeyedScoped<IReadRepository<TaxBracket>, HumanResourcesRepository<TaxBracket>>("hr:taxbrackets");

        builder.Services.AddKeyedScoped<IRepository<Benefit>, HumanResourcesRepository<Benefit>>("hr:benefits");
        builder.Services.AddKeyedScoped<IReadRepository<Benefit>, HumanResourcesRepository<Benefit>>("hr:benefits");

        builder.Services.AddKeyedScoped<IRepository<BenefitEnrollment>, HumanResourcesRepository<BenefitEnrollment>>("hr:benefitenrollments");
        builder.Services.AddKeyedScoped<IReadRepository<BenefitEnrollment>, HumanResourcesRepository<BenefitEnrollment>>("hr:benefitenrollments");

        builder.Services.AddKeyedScoped<IRepository<DocumentTemplate>, HumanResourcesRepository<DocumentTemplate>>("hr:documenttemplates");
        builder.Services.AddKeyedScoped<IReadRepository<DocumentTemplate>, HumanResourcesRepository<DocumentTemplate>>("hr:documenttemplates");

        builder.Services.AddKeyedScoped<IRepository<GeneratedDocument>, HumanResourcesRepository<GeneratedDocument>>("hr:generateddocuments");
        builder.Services.AddKeyedScoped<IReadRepository<GeneratedDocument>, HumanResourcesRepository<GeneratedDocument>>("hr:generateddocuments");

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
