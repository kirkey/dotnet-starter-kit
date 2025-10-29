using System.Text.Json;

namespace FSH.Starter.Blazor.Client.Services;

/// <summary>
/// Implementation of version checking service that interacts with JavaScript interop
/// to detect and manage application version updates
/// </summary>
public class VersionCheckService : IVersionCheckService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<VersionCheckService> _logger;

    /// <summary>
    /// Initializes a new instance of the VersionCheckService
    /// </summary>
    /// <param name="jsRuntime">JavaScript runtime for interop calls</param>
    /// <param name="logger">Logger for diagnostic information</param>
    public VersionCheckService(IJSRuntime jsRuntime, ILogger<VersionCheckService> logger)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Checks if a new version is available on the server
    /// </summary>
    /// <returns>Version check result containing version information</returns>
    public async Task<VersionCheckResult> CheckForNewVersionAsync()
    {
        try
        {
            _logger.LogInformation("Checking for new version...");

            var serverVersion = await InvokeJsAsync<string?>("versionCheck.fetchServerVersion");
            if (string.IsNullOrEmpty(serverVersion))
            {
                _logger.LogWarning("Unable to fetch server version");
                return new VersionCheckResult
                {
                    Success = false,
                    Message = "Unable to fetch server version"
                };
            }

            var currentVersion = await GetCurrentVersionAsync();
            var isNewVersion = !string.IsNullOrEmpty(currentVersion) && currentVersion != serverVersion;

            _logger.LogInformation(
                "Version check completed. Current: {CurrentVersion}, Server: {ServerVersion}, IsNew: {IsNew}",
                currentVersion ?? "null",
                serverVersion,
                isNewVersion);

            return new VersionCheckResult
            {
                Success = true,
                IsNewVersion = isNewVersion,
                CurrentVersion = currentVersion,
                ServerVersion = serverVersion
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking for new version");
            return new VersionCheckResult
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Gets the current stored version from the client
    /// </summary>
    /// <returns>Current version string or null</returns>
    public async Task<string?> GetCurrentVersionAsync()
    {
        try
        {
            return await InvokeJsAsync<string?>("versionCheck.getCurrentVersion");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current version");
            return null;
        }
    }

    /// <summary>
    /// Updates the stored version to the new version
    /// </summary>
    /// <param name="version">Version to store</param>
    public async Task UpdateStoredVersionAsync(string version)
    {
        try
        {
            if (string.IsNullOrEmpty(version))
            {
                _logger.LogWarning("Attempted to store null or empty version");
                return;
            }

            var success = await InvokeJsAsync<bool>("versionCheck.setCurrentVersion", version);
            if (success)
            {
                _logger.LogInformation("Version updated to {Version}", version);
            }
            else
            {
                _logger.LogWarning("Failed to update version to {Version}", version);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating stored version to {Version}", version);
        }
    }

    /// <summary>
    /// Reloads the application page with cache clearing
    /// </summary>
    /// <param name="hardReload">Whether to clear cache before reloading</param>
    public async Task ReloadApplicationAsync(bool hardReload = true)
    {
        try
        {
            _logger.LogInformation("Reloading application (hard reload: {HardReload})", hardReload);
            await InvokeJsVoidAsync("versionCheck.reloadPage", hardReload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reloading application");
        }
    }

    /// <summary>
    /// Initializes version checking on application startup
    /// </summary>
    public async Task<VersionCheckResult> InitializeVersionCheckAsync()
    {
        try
        {
            _logger.LogInformation("Initializing version check...");

            var result = await InvokeJsAsync<JsonElement>("versionCheck.initializeVersionCheck");

            if (!result.TryGetProperty("success", out var successProp) || !successProp.GetBoolean())
            {
                var message = result.TryGetProperty("message", out var msgProp) 
                    ? msgProp.GetString() 
                    : "Unknown error";
                
                _logger.LogWarning("Version check initialization failed: {Message}", message);
                
                return new VersionCheckResult
                {
                    Success = false,
                    Message = message
                };
            }

            var isNewVersion = result.TryGetProperty("isNewVersion", out var newVerProp) && newVerProp.GetBoolean();
            var currentVersion = result.TryGetProperty("currentVersion", out var currProp) ? currProp.GetString() : null;
            var serverVersion = result.TryGetProperty("serverVersion", out var servProp) ? servProp.GetString() : null;

            _logger.LogInformation(
                "Version check initialized. Current: {CurrentVersion}, Server: {ServerVersion}, IsNew: {IsNew}",
                currentVersion ?? "null",
                serverVersion ?? "null",
                isNewVersion);

            return new VersionCheckResult
            {
                Success = true,
                IsNewVersion = isNewVersion,
                CurrentVersion = currentVersion,
                ServerVersion = serverVersion
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing version check");
            return new VersionCheckResult
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Helper method to invoke JavaScript functions with return value
    /// </summary>
    private async Task<T?> InvokeJsAsync<T>(string identifier, params object[] args)
    {
        try
        {
            return await _jsRuntime.InvokeAsync<T>(identifier, args);
        }
        catch (JSDisconnectedException)
        {
            _logger.LogWarning("JavaScript runtime disconnected during {Identifier}", identifier);
            return default;
        }
        catch (JSException ex)
        {
            _logger.LogError(ex, "JavaScript error during {Identifier}", identifier);
            return default;
        }
    }

    /// <summary>
    /// Helper method to invoke JavaScript functions without return value
    /// </summary>
    private async Task InvokeJsVoidAsync(string identifier, params object[] args)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync(identifier, args);
        }
        catch (JSDisconnectedException)
        {
            _logger.LogWarning("JavaScript runtime disconnected during {Identifier}", identifier);
        }
        catch (JSException ex)
        {
            _logger.LogError(ex, "JavaScript error during {Identifier}", identifier);
        }
    }
}

