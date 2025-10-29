namespace FSH.Starter.Blazor.Client.Services;

/// <summary>
/// Service for checking application version updates and managing version state.
/// Implements version checking logic to notify users when new versions are available.
/// </summary>
public interface IVersionCheckService
{
    /// <summary>
    /// Checks if a new version is available on the server
    /// </summary>
    /// <returns>Version check result containing version information</returns>
    Task<VersionCheckResult> CheckForNewVersionAsync();

    /// <summary>
    /// Gets the current stored version from the client
    /// </summary>
    /// <returns>Current version string or null</returns>
    Task<string?> GetCurrentVersionAsync();

    /// <summary>
    /// Updates the stored version to the new version
    /// </summary>
    /// <param name="version">Version to store</param>
    Task UpdateStoredVersionAsync(string version);

    /// <summary>
    /// Reloads the application page with cache clearing
    /// </summary>
    /// <param name="hardReload">Whether to clear cache before reloading</param>
    Task ReloadApplicationAsync(bool hardReload = true);

    /// <summary>
    /// Initializes version checking on application startup
    /// </summary>
    Task<VersionCheckResult> InitializeVersionCheckAsync();
}

/// <summary>
/// Result of a version check operation
/// </summary>
public class VersionCheckResult
{
    /// <summary>
    /// Indicates if the version check was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Indicates if a new version is available
    /// </summary>
    public bool IsNewVersion { get; set; }

    /// <summary>
    /// The current version stored on the client
    /// </summary>
    public string? CurrentVersion { get; set; }

    /// <summary>
    /// The latest version available on the server
    /// </summary>
    public string? ServerVersion { get; set; }

    /// <summary>
    /// Error or status message
    /// </summary>
    public string? Message { get; set; }
}

