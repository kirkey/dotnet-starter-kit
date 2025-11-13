using Carter;
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1;
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
            var companyGroup = app.MapGroup("companies").WithTags("companies");
            companyGroup.MapCompanyCreateEndpoint();
            companyGroup.MapCompanyGetEndpoint();
            companyGroup.MapCompaniesSearchEndpoint();
            companyGroup.MapCompanyUpdateEndpoint();
            companyGroup.MapCompanyDeleteEndpoint();

            var orgUnitGroup = app.MapGroup("organizational-units").WithTags("organizational-units");
            orgUnitGroup.MapOrganizationalUnitCreateEndpoint();
            orgUnitGroup.MapOrganizationalUnitGetEndpoint();
            orgUnitGroup.MapOrganizationalUnitsSearchEndpoint();
            orgUnitGroup.MapOrganizationalUnitUpdateEndpoint();
            orgUnitGroup.MapOrganizationalUnitDeleteEndpoint();
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
        builder.Services.AddKeyedScoped<IRepository<Company>, HumanResourcesRepository<Company>>("hr:companies");
        builder.Services.AddKeyedScoped<IReadRepository<Company>, HumanResourcesRepository<Company>>("hr:companies");

        builder.Services.AddKeyedScoped<IRepository<OrganizationalUnit>, HumanResourcesRepository<OrganizationalUnit>>("hr:organizationalunits");
        builder.Services.AddKeyedScoped<IReadRepository<OrganizationalUnit>, HumanResourcesRepository<OrganizationalUnit>>("hr:organizationalunits");

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

