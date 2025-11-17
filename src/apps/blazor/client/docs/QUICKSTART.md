# Quick Start Guide - Version Notification System

## ✅ Installation Complete

The version notification system has been successfully installed in your Blazor WebAssembly application!

## 📋 What Was Added

### New Files Created:

1. **JavaScript Interop**
   - `wwwroot/js/versionCheck.js` - Client-side version checking logic

2. **Version Configuration**
   - `wwwroot/version.json` - Current version information

3. **C# Services**
   - `Services/IVersionCheckService.cs` - Service interface
   - `Services/VersionCheckService.cs` - Service implementation

4. **Blazor Components**
   - `Components/Common/VersionNotification.razor` - UI component
   - `Components/Common/NotificationPosition.cs` - Position enum

5. **Deployment Scripts**
   - `update-version.sh` - Bash script for version updates
   - `update-version.ps1` - PowerShell script for version updates

6. **Documentation**
   - `VERSION_NOTIFICATION_README.md` - Comprehensive documentation

### Modified Files:

1. **`wwwroot/index.html`**
   - Added versionCheck.js script reference

2. **`Layout/BaseLayout.razor`**
   - Added VersionNotification component

3. **`Program.cs`**
   - Registered VersionCheckService in DI container

## 🚀 How to Use

### 1. Test the Current Setup

The notification component is now active and will check for updates every 5 minutes. To test:

```bash
# Update the version number
./update-version.sh -v 1.0.1

# Or on Windows:
.\update-version.ps1 -Version 1.0.1
```

### 2. Run Your Application

```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/apps/blazor/client
dotnet run
```

The version notification banner will appear when a new version is detected!

### 3. Customize the Check Interval

Edit `Layout/BaseLayout.razor` to change the check frequency:

```razor
@* Check every 2 minutes instead of 5 *@
<VersionNotification CheckIntervalMs="120000" />
```

### 4. Change Notification Position

```razor
@* Display at bottom instead of top *@
<VersionNotification Position="NotificationPosition.Bottom" />
```

## 📦 Deployment Workflow

When deploying a new version:

### Using the Script (Recommended)

**On macOS/Linux:**
```bash
./update-version.sh -v 1.2.3 -d "Bug fixes and improvements"
```

**On Windows:**
```powershell
.\update-version.ps1 -Version 1.2.3 -Description "Bug fixes and improvements"
```

### Manual Update

Edit `wwwroot/version.json`:
```json
{
  "version": "1.2.3",
  "buildDate": "2025-10-29T12:00:00Z",
  "description": "Bug fixes and improvements"
}
```

### Build and Deploy

```bash
dotnet publish -c Release
```

The new version.json will be included in the published output.

## 🔍 Testing the Feature

### Test Scenario 1: First Load
1. Open your application in a browser
2. Check browser localStorage - you'll see `app-version` stored
3. No notification should appear (same version)

### Test Scenario 2: New Version Available
1. With the app running, update version.json to a new version
2. Wait for the check interval (default 5 minutes) or reload the page
3. The notification banner should appear

### Test Scenario 3: Update Process
1. When notification appears, click "Update Now"
2. Page will reload with cache cleared
3. Fresh files are downloaded
4. Notification disappears (versions now match)

### Test Scenario 4: Defer Update
1. When notification appears, click "Later"
2. Notification dismisses
3. Will reappear on next check cycle

## 🐛 Troubleshooting

### Notification Not Appearing?

1. **Check browser console** for errors
2. **Verify version.json** is accessible at `/version.json`
3. **Confirm versionCheck.js** loaded successfully
4. **Check localStorage** for `app-version` entry

### Quick Debug Commands

```javascript
// In browser console:

// Check current stored version
localStorage.getItem('app-version')

// Fetch server version
fetch('/version.json').then(r => r.json()).then(console.log)

// Clear stored version (will trigger notification)
localStorage.removeItem('app-version')
```

## 🎨 Customization Tips

### Change Notification Colors

Edit the CSS in `Components/Common/VersionNotification.razor`:

```css
.version-notification-banner {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    /* Change to your brand colors */
}
```

### Modify Notification Text

Edit the text in `VersionNotification.razor`:

```razor
<MudText Typo="Typo.h6">
    <strong>Update Available! 🎉</strong>
</MudText>
```

### Change Check Frequency Globally

Edit the default in `VersionNotification.razor`:

```csharp
[Parameter]
public int CheckIntervalMs { get; set; } = 180000; // 3 minutes
```

## 📊 Monitoring

The system provides detailed logging. Check logs for:

```
[Information] Checking for new version...
[Information] Version check completed. Current: 1.0.0, Server: 1.0.1, IsNew: True
[Information] User initiated application update
[Information] User deferred application update
```

## 🔗 Integration with CI/CD

### GitHub Actions Example

```yaml
- name: Update Version
  run: |
    chmod +x ./update-version.sh
    ./update-version.sh -v ${{ github.run_number }}

- name: Build
  run: dotnet publish -c Release
```

### Azure DevOps Example

```yaml
- script: |
    ./update-version.sh -v $(Build.BuildNumber)
  displayName: 'Update Version'

- task: DotNetCoreCLI@2
  inputs:
    command: 'publish'
    publishWebProjects: true
```

## 🎯 Next Steps

1. ✅ Test the feature in development
2. ✅ Customize the appearance to match your brand
3. ✅ Integrate version updates into your deployment pipeline
4. ✅ Monitor logs to ensure proper operation
5. ✅ Inform users about the new auto-update feature

## 📚 Additional Resources

- Full Documentation: `VERSION_NOTIFICATION_README.md`
- Service Interface: `Services/IVersionCheckService.cs`
- Component Code: `Components/Common/VersionNotification.razor`

## 💡 Tips

- Use semantic versioning (MAJOR.MINOR.PATCH)
- Update version.json before each deployment
- Test the update process in staging first
- Consider time zones for build dates
- Monitor update adoption rates via analytics

## 🎉 You're All Set!

The version notification system is ready to use. Users will now be automatically notified when updates are available!

For questions or issues, refer to the comprehensive documentation in `VERSION_NOTIFICATION_README.md`.

