# Version Notification System

## Overview

The Version Notification System provides automatic detection and notification of application updates for Blazor WebAssembly clients. When a new version is deployed to the server, connected clients are notified with an interactive banner prompting them to update.

## Features

- **Automatic Version Detection**: Periodically checks for new versions on the server
- **User-Friendly Notifications**: Displays a stylish banner with update and later options
- **Cache Clearing**: Ensures fresh download of all client files on update
- **Configurable Check Intervals**: Customize how often to check for updates
- **Position Flexibility**: Display notification at top or bottom of screen
- **Graceful Error Handling**: Robust logging and error recovery

## Architecture

### Components

1. **JavaScript Interop (`versionCheck.js`)**
   - Fetches version information from server
   - Manages localStorage for version tracking
   - Handles page reload with cache clearing

2. **Version Check Service (`VersionCheckService.cs`)**
   - C# service implementing `IVersionCheckService`
   - Provides version checking and management APIs
   - Integrates with JavaScript interop layer

3. **Version Notification Component (`VersionNotification.razor`)**
   - Blazor UI component displaying update notification
   - Periodic background version checking
   - User interaction handling (Update/Later buttons)

4. **Version Configuration (`version.json`)**
   - Server-side version descriptor
   - Updated during deployment

## Implementation

### 1. Service Registration

The `VersionCheckService` is registered in `Program.cs`:

```csharp
builder.Services.AddScoped<IVersionCheckService, VersionCheckService>();
```

### 2. Component Integration

Add the `VersionNotification` component to your layout (e.g., `BaseLayout.razor`):

```razor
<VersionNotification />
```

### 3. Version Configuration

Update `wwwroot/version.json` with each deployment:

```json
{
  "version": "1.0.0",
  "buildDate": "2025-10-29T00:00:00Z",
  "description": "Application version information"
}
```

### 4. Script Reference

Ensure `versionCheck.js` is referenced in `index.html`:

```html
<script src="js/versionCheck.js"></script>
```

## Usage

### Basic Usage

```razor
@* Add to your main layout *@
<VersionNotification />
```

### Advanced Configuration

```razor
@* Check every 2 minutes, display at bottom *@
<VersionNotification 
    CheckIntervalMs="120000" 
    Position="NotificationPosition.Bottom"
    AutoCheckOnInit="true" />
```

### Component Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `CheckIntervalMs` | `int` | `300000` | Interval for version checks in milliseconds (5 minutes) |
| `Position` | `NotificationPosition` | `Top` | Banner position (Top/Bottom) |
| `AutoCheckOnInit` | `bool` | `true` | Check for updates on component initialization |

## Service API

### IVersionCheckService Methods

```csharp
// Check for new version
Task<VersionCheckResult> CheckForNewVersionAsync();

// Get currently stored version
Task<string?> GetCurrentVersionAsync();

// Update stored version
Task UpdateStoredVersionAsync(string version);

// Reload application with cache clearing
Task ReloadApplicationAsync(bool hardReload = true);

// Initialize version checking
Task<VersionCheckResult> InitializeVersionCheckAsync();
```

### VersionCheckResult Properties

```csharp
public class VersionCheckResult
{
    public bool Success { get; set; }
    public bool IsNewVersion { get; set; }
    public string? CurrentVersion { get; set; }
    public string? ServerVersion { get; set; }
    public string? Message { get; set; }
}
```

## Deployment Workflow

### 1. Update Version File

Before deploying, update `wwwroot/version.json`:

```json
{
  "version": "1.0.1",
  "buildDate": "2025-10-29T12:00:00Z",
  "description": "Bug fixes and improvements"
}
```

### 2. Build and Deploy

Build and deploy your Blazor application as usual. The version file will be included in the deployment.

### 3. Client Detection

Connected clients will:
1. Detect the version change during periodic checks
2. Display the update notification banner
3. Prompt users to update or defer

### 4. User Update Flow

When users click "Update Now":
1. Local version is updated to match server version
2. Service worker caches are cleared
3. Page performs a hard reload
4. Fresh client files are downloaded

## Customization

### Styling

Modify the CSS in `VersionNotification.razor`:

```css
.version-notification-banner {
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
}
```

### Notification Message

Edit the notification text in `VersionNotification.razor`:

```razor
<MudText Typo="Typo.h6" Class="mb-1">
    <strong>New Version Available!</strong>
</MudText>
<MudText Typo="Typo.body2" Class="text-muted">
    A new version (@_serverVersion) is available. Update now to get the latest features and improvements.
</MudText>
```

### Check Frequency

Adjust the default check interval by modifying the `CheckIntervalMs` parameter:

```razor
@* Check every 10 minutes *@
<VersionNotification CheckIntervalMs="600000" />
```

## Best Practices

### 1. Semantic Versioning

Use semantic versioning (MAJOR.MINOR.PATCH) for version numbers:
- **MAJOR**: Breaking changes
- **MINOR**: New features (backward compatible)
- **PATCH**: Bug fixes

### 2. Build Automation

Automate version file updates in your CI/CD pipeline:

```bash
# Example: Update version during build
echo "{\"version\": \"$VERSION\", \"buildDate\": \"$(date -u +%Y-%m-%dT%H:%M:%SZ)\"}" > wwwroot/version.json
```

### 3. Cache Busting

The version check includes cache-busting query parameters to ensure fresh version data:

```javascript
const response = await fetch(`/version.json?t=${timestamp}`, {
    cache: 'no-cache'
});
```

### 4. Graceful Degradation

The system gracefully handles errors:
- Network failures don't crash the application
- Missing version.json is logged but not fatal
- JavaScript errors are caught and logged

### 5. User Experience

- **Non-intrusive**: Notification appears only when updates are available
- **User choice**: Users can defer updates with the "Later" button
- **Visual feedback**: "Updating..." state shown during reload
- **Fast reloads**: Cache clearing ensures fresh content

## Troubleshooting

### Version Not Detected

**Problem**: Clients don't detect new versions

**Solutions**:
- Verify `version.json` is deployed and accessible
- Check browser console for JavaScript errors
- Ensure `versionCheck.js` is loaded
- Verify service registration in `Program.cs`

### Notification Not Appearing

**Problem**: Notification banner doesn't display

**Solutions**:
- Check that `VersionNotification` component is added to layout
- Verify MudBlazor is properly configured
- Check browser console for component errors
- Ensure version numbers are different

### Cache Not Clearing

**Problem**: Old files still loaded after update

**Solutions**:
- Verify service worker cache clearing logic
- Check browser's cache settings
- Test with hard reload (Ctrl+Shift+R)
- Review service worker implementation

### Frequent False Positives

**Problem**: Update notifications appear too often

**Solutions**:
- Verify version.json is static (not dynamically generated)
- Check for cache issues serving version.json
- Ensure version number format is consistent
- Review localStorage implementation

## Logging

The system provides comprehensive logging:

```csharp
// Service logs
_logger.LogInformation("Checking for new version...");
_logger.LogWarning("Unable to fetch server version");
_logger.LogError(ex, "Error checking for new version");

// Component logs
Logger.LogInformation("User initiated application update");
Logger.LogInformation("User deferred application update");
```

Enable detailed logging in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "FSH.Starter.Blazor.Client.Services.VersionCheckService": "Debug",
      "FSH.Starter.Blazor.Client.Components.Common.VersionNotification": "Debug"
    }
  }
}
```

## Security Considerations

1. **No Sensitive Data**: Version information is public
2. **No Authentication Required**: Version.json is accessible to all
3. **No User Data**: Only version strings stored in localStorage
4. **XSS Protection**: All user-facing text is properly escaped

## Browser Compatibility

- **Modern Browsers**: Full support (Chrome, Firefox, Edge, Safari)
- **Service Workers**: Required for cache clearing
- **LocalStorage**: Required for version tracking
- **ES6+**: Modern JavaScript features used

## Performance Impact

- **Minimal**: Periodic checks use lightweight HTTP requests
- **Async**: All operations run asynchronously
- **Efficient**: Cached results prevent redundant checks
- **Non-blocking**: UI remains responsive during checks

## Future Enhancements

Potential improvements:

1. **Release Notes**: Display changelog in notification
2. **Auto-Update**: Optional automatic updates without user interaction
3. **Update Scheduling**: Schedule updates for off-hours
4. **Progressive Updates**: Staged rollouts to user subsets
5. **Version Analytics**: Track update adoption rates
6. **Rollback Support**: Detect and handle version rollbacks

## Support

For issues or questions:
- Check application logs
- Review browser console
- Verify configuration
- Test with fresh browser session

## License

This implementation follows the same license as the FSH Starter Kit project.

