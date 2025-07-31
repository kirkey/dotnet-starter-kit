using Accounting.Domain;
using Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;
using Accounting.Infrastructure.Endpoints.Payees.v1;
using Accounting.Infrastructure.Endpoints.Vendors.v1;
using Accounting.Infrastructure.Persistence;
using Accounting.Infrastructure.Persistence.Configurations;
using Carter;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Infrastructure;

public static class AccountingModule
{
    public class Endpoints() : CarterModule("accounting")
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var account = app.MapGroup("accounts").WithTags("accounts");
            account.MapChartOfAccountCreateEndpoint();
            account.MapChartOfAccountGetEndpoint();
            account.MapChartOfAccountSearchEndpoint();
            account.MapChartOfAccountUpdateEndpoint();
            account.MapChartOfAccountDeleteEndpoint();
            
            var payee = app.MapGroup("payees").WithTags("payees");
            payee.MapPayeeCreateEndpoint();
            payee.MapPayeeGetEndpoint();
            payee.MapPayeeSearchEndpoint();
            payee.MapPayeeUpdateEndpoint();
            payee.MapPayeeDeleteEndpoint();
            
            var vendor = app.MapGroup("vendors").WithTags("vendors");
            vendor.MapVendorCreateEndpoint();
            vendor.MapVendorGetEndpoint();
            vendor.MapVendorSearchEndpoint();
            vendor.MapVendorUpdateEndpoint();
            vendor.MapVendorDeleteEndpoint();
        }
    }

    public static WebApplicationBuilder RegisterAccountingServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<AccountingDbContext>();
        builder.Services.AddScoped<IDbInitializer, AccountingDbInitializer>();
    
        // Register without keys
        builder.Services.AddScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>();
        builder.Services.AddScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>();
        builder.Services.AddScoped<IRepository<Payee>, AccountingRepository<Payee>>();
        builder.Services.AddScoped<IReadRepository<Payee>, AccountingRepository<Payee>>();
        builder.Services.AddScoped<IRepository<Vendor>, AccountingRepository<Vendor>>();
        builder.Services.AddScoped<IReadRepository<Vendor>, AccountingRepository<Vendor>>();
    
        // Register with keys
        builder.Services.AddKeyedScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:accounts");
        builder.Services.AddKeyedScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:accounts");
        builder.Services.AddKeyedScoped<IRepository<Payee>, AccountingRepository<Payee>>("accounting:payees");
        builder.Services.AddKeyedScoped<IReadRepository<Payee>, AccountingRepository<Payee>>("accounting:payees");
        builder.Services.AddKeyedScoped<IRepository<Vendor>, AccountingRepository<Vendor>>("accounting:vendors");
        builder.Services.AddKeyedScoped<IReadRepository<Vendor>, AccountingRepository<Vendor>>("accounting:vendors");
    
        MappingConfiguration.RegisterMappings();
    
        return builder;
    }

    public static WebApplication UseAccountingModule(this WebApplication app)
    {
        return app;
    }
}
