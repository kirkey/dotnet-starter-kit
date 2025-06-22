using Accounting.Domain;
using Accounting.Infrastructure.Endpoints.ChartOfAccount.v1;
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
            var chartOfAccount = app.MapGroup("ChartOfAccounts").WithTags("ChartOfAccounts");
            chartOfAccount.MapChartOfAccountCreateEndpoint();
            chartOfAccount.MapChartOfAccountGetEndpoint();
            chartOfAccount.MapChartOfAccountSearchEndpoint();
            chartOfAccount.MapChartOfAccountUpdateEndpoint();
            chartOfAccount.MapChartOfAccountDeleteEndpoint();
            
            // var payee = app.MapGroup("Payees").WithTags("Payees");
            // payee.MapPayeeCreateEndpoint();
            // payee.MapPayeeGetEndpoint();
            // payee.MapPayeeSearchEndpoint();
            // payee.MapPayeeUpdateEndpoint();
            // payee.MapPayeeDeleteEndpoint();
        }
    }

    public static WebApplicationBuilder RegisterAccountingServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<AccountingDbContext>();
        builder.Services.AddScoped<IDbInitializer, AccountingDbInitializer>();
        
        builder.Services.AddKeyedScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:ChartOfAccounts");
        builder.Services.AddKeyedScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:ChartOfAccounts");
        
        builder.Services.AddKeyedScoped<IRepository<Payee>, AccountingRepository<Payee>>("accounting:Payees");
        builder.Services.AddKeyedScoped<IReadRepository<Payee>, AccountingRepository<Payee>>("accounting:Payees");
        
        MappingConfiguration.RegisterMappings();
        
        return builder;
    }

    public static WebApplication UseAccountingModule(this WebApplication app)
    {
        return app;
    }
}
