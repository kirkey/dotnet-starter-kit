using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Attendance;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Designations;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DesignationAssignments;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeContacts;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDependents;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDocuments;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Employees;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.OrganizationalUnits;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Shifts;
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
            app.MapShiftsEndpoints();
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

