namespace FSH.Framework.Infrastructure.SignalR;

/// <summary>
/// Extension methods for configuring SignalR in the application.
/// </summary>
public static class SignalRExtensions
{
    /// <summary>
    /// Registers SignalR services with the application.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The configuration.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSignalRInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(config);

        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = config.GetValue<bool>("SignalR:EnableDetailedErrors");
            options.KeepAliveInterval = TimeSpan.FromSeconds(config.GetValue<int>("SignalR:KeepAliveInterval", 15));
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(config.GetValue<int>("SignalR:ClientTimeoutInterval", 30));
            options.MaximumReceiveMessageSize = config.GetValue<long?>("SignalR:MaximumReceiveMessageSize");
        });

        services.AddSingleton<IConnectionTracker, ConnectionTracker>();

        return services;
    }
}
