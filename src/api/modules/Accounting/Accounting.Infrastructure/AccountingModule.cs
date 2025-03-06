using Carter;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using Accounting.Domain;
using Accounting.Infrastructure.Endpoints.v1;
using Accounting.Infrastructure.Persistence;
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
            var accountGroup = app.MapGroup("accounts").WithTags("accounts");
            accountGroup.MapAccountCreationEndpoint();
            accountGroup.MapGetAccountEndpoint();
            accountGroup.MapGetAccountSearchEndpoint();
            accountGroup.MapAccountUpdateEndpoint();
            accountGroup.MapAccountDeleteEndpoint();
        }
    }

    public static WebApplicationBuilder RegisterAccountingServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<AccountingDbContext>();
        builder.Services.AddScoped<IDbInitializer, AccountingDbInitializer>();
        builder.Services.AddKeyedScoped<IRepository<Account>, AccountingRepository<Account>>("accounting:accounts");
        builder.Services.AddKeyedScoped<IReadRepository<Account>, AccountingRepository<Account>>("accounting:accounts");
        return builder;
    }

    public static WebApplication UseAccountingModule(this WebApplication app)
    {
        return app;
    }
}
