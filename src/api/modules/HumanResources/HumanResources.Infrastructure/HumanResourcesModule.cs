using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Designations;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DesignationAssignments;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Employees;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.OrganizationalUnits;
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

