using Carter;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Starter.WebApi.App.Domain;
using FSH.Starter.WebApi.App.Features.Create.v1;
using FSH.Starter.WebApi.App.Features.Delete.v1;
using FSH.Starter.WebApi.App.Features.Get.v1;
using FSH.Starter.WebApi.App.Features.GetList.v1;
using FSH.Starter.WebApi.App.Features.Update.v1;
using FSH.Starter.WebApi.App.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.App;

public static class AppModule
{
    public class Endpoints : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var appGroup = app.MapGroup("Apps").WithTags("Apps");
            appGroup.MapGroupCreationEndpoint();
            appGroup.MapGetAppEndpoint();
            appGroup.MapGetAppListEndpoint();
            appGroup.MapGroupUpdateEndpoint();
            appGroup.MapGroupDeletionEndpoint();
        }
    }

    public static WebApplicationBuilder RegisterAppServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<AppDbContext>();
        builder.Services.AddScoped<IDbInitializer, AppDbInitializer>();
        builder.Services.AddKeyedScoped<IRepository<Group>, AppRepository<Group>>("Group");
        builder.Services.AddKeyedScoped<IReadRepository<Group>, AppRepository<Group>>("Group");
        return builder;
    }

    public static WebApplication UseAppModule(this WebApplication app)
    {
        return app;
    }
}
