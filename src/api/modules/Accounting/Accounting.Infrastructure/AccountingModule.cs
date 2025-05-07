using Carter;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using Accounting.Domain;
using Accounting.Infrastructure.Endpoints.v1;
using Accounting.Infrastructure.Persistence;
using Accounting.Infrastructure.Persistence.Configurations;
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
            chartOfAccount.MapAccountCreateEndpoint();
            chartOfAccount.MapAccountGetEndpoint();
            chartOfAccount.MapAccountSearchEndpoint();
            chartOfAccount.MapAccountUpdateEndpoint();
            chartOfAccount.MapAccountDeleteEndpoint();
        }
    }

    public static WebApplicationBuilder RegisterAccountingServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<AccountingDbContext>();
        builder.Services.AddScoped<IDbInitializer, AccountingDbInitializer>();
        builder.Services.AddKeyedScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:ChartOfAccounts");
        builder.Services.AddKeyedScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:ChartOfAccounts");
        
        MappingConfiguration.RegisterMappings();
        
        return builder;
    }

    public static WebApplication UseAccountingModule(this WebApplication app)
    {
        return app;
    }
}
