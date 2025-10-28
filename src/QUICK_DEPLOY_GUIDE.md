# Quick Deployment Guide for IIS

## Summary of Changes

✅ **Swagger enabled in Production** - Added `SwaggerOptions` configuration
✅ **Performance logging** - Added startup time tracking  
✅ **Auto-create directories** - wwwroot and files folders created automatically
✅ **Production configuration** - appsettings.Production.json now has all required settings

## Deploy to IIS (Windows Server)

### Step 1: Publish the API

From your Mac, run:

```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src

# Publish API
make publish-api
```

This creates the deployment package in `./publishfsh9/api/`

### Step 2: Copy to Windows Server

Copy the entire `publishfsh9/api` folder to your Windows Server at:
```
D:\www\fsh9\api\
```

### Step 3: Restart IIS

On Windows Server, restart the IIS Application Pool:

```powershell
# In PowerShell as Administrator
Restart-WebAppPool -Name "YourAppPoolName"
```

Or restart in IIS Manager GUI.

### Step 4: Verify Deployment

1. **Check Swagger**: https://api.zaneco.ph:7000/swagger/
2. **Check logs**: `D:\www\fsh9\api\logs\app-YYYYMMDD.log`

You should see performance metrics in the logs:
```
[18:39:44 INF] WebApplication.CreateBuilder took 123ms
[18:39:44 INF] ConfigureFshFramework took 456ms
[18:39:44 INF] RegisterModules took 789ms
[18:39:44 INF] app.Build took 234ms
[18:39:44 INF] Middleware configuration took 567ms
[18:39:44 INF] Total startup time: 2169ms (2.169 seconds)
```

## Troubleshooting

### Swagger still shows "authentication failed"

1. Check `D:\www\fsh9\api\appsettings.Production.json` has:
   ```json
   "SwaggerOptions": {
     "Enable": true
   }
   ```

2. Restart IIS Application Pool

### Slow Startup (20+ seconds)

Check the logs for which phase is slow:
- Database seeding: Consider disabling in production
- Database connection: Check network/connection string
- Module registration: Normal on first startup

### Server crashes on startup

1. Check `D:\www\fsh9\api\logs\app-YYYYMMDD.log`
2. Verify database connection string is correct
3. Ensure database is accessible from Windows Server
4. Check file permissions on `D:\www\fsh9\api\` folder

## Configuration

### Disable Swagger in Production (After Testing)

Edit `D:\www\fsh9\api\appsettings.Production.json`:

```json
{
  "SwaggerOptions": {
    "Enable": false
  }
}
```

Restart IIS Application Pool.

### Update CORS for Your Blazor Client

Add your Blazor URL to the allowed origins:

```json
{
  "CorsOptions": {
    "AllowedOrigins": [
      "https://your-blazor-domain.com",
      "http://localhost:7100"
    ]
  }
}
```

## Files Modified

1. `/api/framework/Infrastructure/OpenApi/SwaggerOptions.cs` - NEW
2. `/api/framework/Infrastructure/OpenApi/Extensions.cs` - UPDATED
3. `/api/server/Program.cs` - UPDATED (performance logging)
4. `/api/server/appsettings.Production.json` - UPDATED (full configuration)

## Next Steps

1. ✅ Deploy to IIS
2. ✅ Verify Swagger works: https://api.zaneco.ph:7000/swagger/
3. ✅ Test API endpoints
4. 📝 Generate Blazor client from Swagger
5. 🔒 Consider disabling Swagger in production after testing

---

**Need Help?**

Check the detailed guide: [SWAGGER_IIS_FIX.md](./SWAGGER_IIS_FIX.md)

