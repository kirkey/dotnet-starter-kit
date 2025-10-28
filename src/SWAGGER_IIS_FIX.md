# Swagger Configuration Fix for IIS Production

## Issues Resolved

1. **Swagger not accessible in Production**: Swagger was only enabled in Development/Docker environments
2. **Missing wwwroot directory**: Application was warning about missing wwwroot folder
3. **Missing files directory**: Application was crashing due to missing files storage directory
4. **Performance logging**: Added detailed startup performance tracking

## Changes Made

### 1. Created SwaggerOptions Configuration Class
**File**: `/api/framework/Infrastructure/OpenApi/SwaggerOptions.cs`
- New configuration class to control Swagger enable/disable in production

### 2. Updated OpenApi Extensions
**File**: `/api/framework/Infrastructure/OpenApi/Extensions.cs`
- Added SwaggerOptions configuration binding
- Modified `UseOpenApi` to check configuration setting
- Swagger now enabled when `SwaggerOptions.Enable = true` in appsettings

### 3. Updated Production Configuration
**File**: `/api/server/appsettings.Production.json`
- Copied all working settings from `appsettings.json`
- Added `SwaggerOptions.Enable = true` to enable Swagger in production

### 4. Enhanced Program.cs with Performance Logging
**File**: `/api/server/Program.cs`
- Added startup time tracking for each initialization phase
- Auto-creates required directories (wwwroot, files) if missing
- Logs detailed performance metrics

## Configuration

### Enable/Disable Swagger in Production

Add to your `appsettings.Production.json`:

```json
{
  "SwaggerOptions": {
    "Enable": true
  }
}
```

Set `Enable: false` to disable Swagger in production for security.

## Deployment Steps

### Option 1: Using Makefile (Recommended)

```bash
# From /src directory
make publish-all

# Or publish individually
make publish-api
make publish-blazor
```

This publishes to `./publishfsh9/api` and `./publishfsh9/blazor`

### Option 2: Manual Publish

```bash
# API
dotnet publish api/server/Server.csproj -c Release -r win-x64 -o ./publish/api

# Blazor
dotnet publish apps/blazor/client/Client.csproj -c Release -r win-x64 -o ./publish/blazor
```

### Deploy to IIS

1. **Copy files to Windows Server**:
   - API to: `D:\www\fsh9\api\`
   - Blazor to: `D:\www\fsh9\blazor\`

2. **Create required directories** (auto-created by code, but good to verify):
   ```
   D:\www\fsh9\api\wwwroot
   D:\www\fsh9\api\files
   D:\www\fsh9\api\logs
   ```

3. **Set IIS App Pool Permissions**:
   - Grant "Modify" permissions to `IIS AppPool\YourAppPoolName` on:
     - `D:\www\fsh9\api\files`
     - `D:\www\fsh9\api\logs`

4. **Update appsettings.Production.json on server**:
   - Update database connection string
   - Update CORS allowed origins to include your Blazor URL
   - Set JWT key for production
   - Configure mail settings

5. **Restart IIS Application Pool**

## Accessing Swagger

Once deployed with `SwaggerOptions.Enable = true`:

- Swagger UI: `https://api.zaneco.ph:7000/swagger/`
- Swagger JSON: `https://api.zaneco.ph:7000/swagger/v1/swagger.json`

## Performance Metrics

The updated `Program.cs` now logs:
- WebApplication.CreateBuilder time
- ConfigureFshFramework time
- RegisterModules time
- app.Build time
- Middleware configuration time
- **Total startup time**

Check logs to identify slow startup components.

## Security Recommendations

For production environments:

1. **Disable Swagger** after initial testing:
   ```json
   "SwaggerOptions": { "Enable": false }
   ```

2. **Or protect Swagger with IP restrictions** in IIS

3. **Use strong JWT keys** (generate new key for production)

4. **Enable HTTPS only** in production

5. **Review CORS settings** to only allow trusted origins

## Troubleshooting

### Swagger shows "authentication failed"
- **Solution**: Set `SwaggerOptions.Enable = true` in `appsettings.Production.json`

### Application crashes on startup
- Check logs at `D:\www\fsh9\api\logs\app-YYYYMMDD.log`
- Verify database connection string
- Ensure files/wwwroot directories exist with proper permissions

### Slow startup (20+ seconds)
- Check performance logs to identify bottleneck
- Consider disabling database seeding in production
- Optimize database connection pooling

## Testing

After deployment:

1. **Check Swagger**: `https://api.zaneco.ph:7000/swagger/`
2. **Check API Health**: `https://api.zaneco.ph:7000/health` (if configured)
3. **Generate API Client** (from your Mac):
   ```bash
   cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src
   make gen-client API_URL=https://api.zaneco.ph:7000
   ```

## Next Steps

1. Deploy the updated code to your IIS server
2. Verify Swagger is accessible
3. Test API endpoints
4. Generate Blazor API client using updated Swagger
5. Consider disabling Swagger in production after testing

