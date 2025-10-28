StaticLogger.EnsureInitialized();
var startTime = DateTime.UtcNow;
Log.Information("Server booting up...");
Log.Information("Environment: {Environment}", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production");
Log.Information("Current Directory: {Directory}", Directory.GetCurrentDirectory());
Log.Information("Is User Interactive: {IsInteractive}", Environment.UserInteractive);

try
{
    // Ensure required directories exist
    var currentDir = Directory.GetCurrentDirectory();
    var wwwrootPath = Path.Combine(currentDir, "wwwroot");
    var filesPath = Path.Combine(currentDir, "files");
    
    if (!Directory.Exists(wwwrootPath))
    {
        Log.Information("Creating wwwroot directory: {Path}", wwwrootPath);
        Directory.CreateDirectory(wwwrootPath);
    }
    
    if (!Directory.Exists(filesPath))
    {
        Log.Information("Creating files directory: {Path}", filesPath);
        Directory.CreateDirectory(filesPath);
    }

    var builderStart = DateTime.UtcNow;
    var builder = WebApplication.CreateBuilder(args);
    Log.Information("WebApplication.CreateBuilder took {Duration}ms", (DateTime.UtcNow - builderStart).TotalMilliseconds);

    var configStart = DateTime.UtcNow;
    builder.ConfigureFshFramework();
    Log.Information("ConfigureFshFramework took {Duration}ms", (DateTime.UtcNow - configStart).TotalMilliseconds);

    var moduleStart = DateTime.UtcNow;
    builder.RegisterModules();
    Log.Information("RegisterModules took {Duration}ms", (DateTime.UtcNow - moduleStart).TotalMilliseconds);

    var buildStart = DateTime.UtcNow;
    var app = builder.Build();
    Log.Information("app.Build took {Duration}ms", (DateTime.UtcNow - buildStart).TotalMilliseconds);

    var middlewareStart = DateTime.UtcNow;
    app.UseFshFramework();
    app.UseModules();
    Log.Information("Middleware configuration took {Duration}ms", (DateTime.UtcNow - middlewareStart).TotalMilliseconds);
    
    var totalStartupTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
    Log.Information("Total startup time: {Duration}ms ({Seconds} seconds)", totalStartupTime, totalStartupTime / 1000);
    Log.Information("Application configured successfully. Starting web host...");
    await app.RunAsync().ConfigureAwait(false);
}
catch (Exception ex) when (!ex.GetType().Name.Equals("HostAbortedException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception occurred during application startup or execution");
    Log.Fatal("Exception Type: {ExceptionType}", ex.GetType().FullName);
    Log.Fatal("Exception Message: {Message}", ex.Message);
    Log.Fatal("Stack Trace: {StackTrace}", ex.StackTrace);
    
    if (ex.InnerException != null)
    {
        Log.Fatal("Inner Exception Type: {InnerExceptionType}", ex.InnerException.GetType().FullName);
        Log.Fatal("Inner Exception Message: {InnerMessage}", ex.InnerException.Message);
        Log.Fatal("Inner Stack Trace: {InnerStackTrace}", ex.InnerException.StackTrace);
    }
    
    // Ensure logs are flushed before exit
    await Log.CloseAndFlushAsync().ConfigureAwait(false);
    
    // For console/interactive mode, pause to show error
    if (Environment.UserInteractive)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n" + "=".PadRight(80, '='));
        Console.WriteLine("CRITICAL ERROR - Application failed to start");
        Console.WriteLine("=".PadRight(80, '='));
        Console.ResetColor();
        Console.WriteLine($"\nError: {ex.Message}");
        Console.WriteLine($"\nCheck the log files in the 'logs' directory for detailed information.");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nPress any key to exit...");
        Console.ResetColor();
        Console.ReadKey();
    }
    
    throw;
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server shutting down...");
    Log.Information("Shutdown completed at: {Time}", DateTime.UtcNow);
    await Log.CloseAndFlushAsync().ConfigureAwait(false);
}
