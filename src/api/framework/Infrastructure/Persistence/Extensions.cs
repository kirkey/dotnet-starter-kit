using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace FSH.Framework.Infrastructure.Persistence;
public static class Extensions
{
    private static readonly ILogger Logger = Log.ForContext(typeof(Extensions));
    internal static DbContextOptionsBuilder ConfigureDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
    {
        builder.ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning));
        return dbProvider.ToUpperInvariant() switch
        {
            DbProviders.PostgreSQL => builder.UseNpgsql(
                connectionString, optionsBuilder => optionsBuilder
                        .MigrationsAssembly("FSH.Starter.WebApi.Migrations.PostgreSQL")).EnableSensitiveDataLogging(),
                // .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),

            DbProviders.MySQL => builder.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    optionsBuilder => optionsBuilder
                            .MigrationsAssembly("FSH.Starter.WebApi.Migrations.MySQL")
                            .SchemaBehavior(Pomelo.EntityFrameworkCore.MySql.Infrastructure.MySqlSchemaBehavior.Ignore))
                // .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableSensitiveDataLogging(),

            DbProviders.MSSQL => builder.UseSqlServer(connectionString, optionsBuilder =>
                                optionsBuilder.MigrationsAssembly("FSH.Starter.WebApi.Migrations.MSSQL")),
                // .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),

            _ => throw new InvalidOperationException($"DB Provider {dbProvider} is not supported."),
        };
    }

    public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddOptions<DatabaseOptions>()
            .BindConfiguration(nameof(DatabaseOptions))
            .ValidateDataAnnotations()
            .PostConfigure(config =>
            {
                Logger.Information("current db provider: {DatabaseProvider}", config.Provider);
                Logger.Information("for documentations and guides, visit https://www.fullstackhero.net");
                Logger.Information("to sponsor this project, visit https://opencollective.com/fullstackhero");
            });
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditInterceptor>();
        return builder;
    }

    public static IServiceCollection BindDbContext<TContext>(this IServiceCollection services, string? moduleKey = null)
        where TContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddDbContext<TContext>((sp, options) =>
        {
            var dbConfig = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;

            // Default to global provider/connection
            var provider = dbConfig.Provider;
            var conn = dbConfig.ConnectionString;

            // If a module key is provided and an override exists, use it
            if (!string.IsNullOrEmpty(moduleKey)
                && dbConfig.ModuleConnectionStrings.TryGetValue(moduleKey, out var moduleOptions)
                && !string.IsNullOrEmpty(moduleOptions.ConnectionString))
            {
                provider = string.IsNullOrEmpty(moduleOptions.Provider) ? provider : moduleOptions.Provider;
                conn = moduleOptions.ConnectionString;
            }

            options.ConfigureDatabase(provider, conn);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });
        return services;
    }
}
