using Serilog.Core;

namespace FSH.Framework.Infrastructure.Logging.Serilog;

/// <summary>
/// Static logger for early initialization before the full Serilog configuration is loaded.
/// This ensures startup errors are captured even when running in IIS.
/// </summary>
public static class StaticLogger
{
    /// <summary>
    /// Ensures the logger is initialized with console and file output for startup diagnostics.
    /// </summary>
    public static void EnsureInitialized()
    {
        if (Log.Logger is not Logger)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    path: "logs/startup-.log",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10485760,
                    retainedFileCountLimit: 7,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .WriteTo.OpenTelemetry()
                .CreateLogger();
        }
    }
}
