# FSH Starter Kit - IIS Deployment Architecture

## System Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                         Client Browser                          │
│                     (User's Web Browser)                        │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       │ HTTPS
                       ▼
┌─────────────────────────────────────────────────────────────────┐
│                      IIS Web Server                             │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │               Blazor WebAssembly Site                    │  │
│  │  Site: https://app.yourdomain.com                        │  │
│  │  App Pool: FSH.Starter.Blazor.Pool                       │  │
│  │  Path: C:\inetpub\wwwroot\FSH.Starter.Blazor            │  │
│  │  ┌─────────────────────────────────────────────────┐    │  │
│  │  │  Static Files:                                  │    │  │
│  │  │  • index.html                                   │    │  │
│  │  │  • *.js, *.css                                  │    │  │
│  │  │  • _framework/ (WebAssembly files)             │    │  │
│  │  │  • appsettings.json (API endpoint config)      │    │  │
│  │  │  • web.config (IIS configuration)              │    │  │
│  │  └─────────────────────────────────────────────────┘    │  │
│  └──────────────────────────────────────────────────────────┘  │
│                              │                                  │
│                              │ API Calls (HTTPS)                │
│                              ▼                                  │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │                   ASP.NET Core API Site                  │  │
│  │  Site: https://api.yourdomain.com                        │  │
│  │  App Pool: FSH.Starter.API.Pool                          │  │
│  │  Path: C:\inetpub\wwwroot\FSH.Starter.API               │  │
│  │  ┌─────────────────────────────────────────────────┐    │  │
│  │  │  Application Files:                             │    │  │
│  │  │  • FSH.Starter.WebApi.Host.dll                 │    │  │
│  │  │  • Dependencies (*.dll)                         │    │  │
│  │  │  • appsettings.json                            │    │  │
│  │  │  • web.config (IIS configuration)              │    │  │
│  │  │  • logs/ (Application logs)                    │    │  │
│  │  └─────────────────────────────────────────────────┘    │  │
│  └──────────────────────────────────────────────────────────┘  │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       │ Database Connection
                       ▼
┌─────────────────────────────────────────────────────────────────┐
│                       Database Server                           │
│              (SQL Server / PostgreSQL / MySQL)                  │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  • FSHStarterDB                                          │  │
│  │  • User authentication                                   │  │
│  │  • Application data                                      │  │
│  └──────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

## Deployment Flow

```
┌──────────────────┐
│  Developer       │
│  Machine         │
│  (Your Mac)      │
└────────┬─────────┘
         │
         │ 1. Build & Publish
         │    ./publish-for-iis.sh
         ▼
┌──────────────────┐
│  Deploy Folder   │
│  • API.zip       │
│  • Blazor.zip    │
└────────┬─────────┘
         │
         │ 2. Transfer files
         │    (FTP/RDP/Network)
         ▼
┌──────────────────┐
│  IIS Server      │
│  (Windows)       │
└────────┬─────────┘
         │
         │ 3. Deploy
         │    .\deploy-to-iis.ps1
         ▼
┌──────────────────┐
│  IIS Sites       │
│  • API Site      │
│  • Blazor Site   │
└────────┬─────────┘
         │
         │ 4. Test
         │    Browse to URLs
         ▼
┌──────────────────┐
│  Production      │
│  Environment     │
└──────────────────┘
```

## Network Flow

```
User Browser
    │
    │ 1. Request https://app.yourdomain.com
    ▼
IIS Blazor Site
    │
    │ 2. Serve index.html & WebAssembly files
    ▼
User Browser (Blazor App Running)
    │
    │ 3. API Call: GET https://api.yourdomain.com/api/users
    ▼
IIS API Site
    │
    │ 4. Process request
    │    • Authentication
    │    • Authorization
    │    • Business logic
    ▼
Database Server
    │
    │ 5. Query data
    ▼
IIS API Site
    │
    │ 6. Return JSON response
    ▼
User Browser (Blazor App)
    │
    │ 7. Display data
    ▼
User sees results
```

## File Structure on IIS Server

```
C:\inetpub\wwwroot\
├── FSH.Starter.API\
│   ├── FSH.Starter.WebApi.Host.dll
│   ├── FSH.Starter.WebApi.Host.exe
│   ├── appsettings.json
│   ├── appsettings.Production.json
│   ├── web.config
│   ├── wwwroot\
│   ├── logs\
│   │   └── stdout_[timestamp].log
│   └── [Dependencies]\
│       ├── Accounting.Application.dll
│       ├── Catalog.Infrastructure.dll
│       ├── Store.Application.dll
│       └── [Other DLLs]
│
└── FSH.Starter.Blazor\
    ├── index.html
    ├── appsettings.json
    ├── web.config
    ├── css\
    ├── _framework\
    │   ├── blazor.webassembly.js
    │   ├── FSH.Starter.Blazor.Client.dll
    │   ├── dotnet.wasm
    │   └── [Other WebAssembly files]
    └── service-worker.js
```

## IIS Application Pool Configuration

```
┌────────────────────────────────────────┐
│  FSH.Starter.API.Pool                  │
├────────────────────────────────────────┤
│  .NET CLR Version: No Managed Code     │
│  Managed Pipeline: Integrated          │
│  Identity: ApplicationPoolIdentity     │
│  Start Mode: AlwaysRunning (optional)  │
│  Idle Timeout: 20 minutes             │
└────────────────────────────────────────┘

┌────────────────────────────────────────┐
│  FSH.Starter.Blazor.Pool               │
├────────────────────────────────────────┤
│  .NET CLR Version: No Managed Code     │
│  Managed Pipeline: Integrated          │
│  Identity: ApplicationPoolIdentity     │
│  Start Mode: OnDemand                  │
└────────────────────────────────────────┘
```

## Security Layers

```
┌─────────────────────────────────────┐
│  Network Layer                      │
│  • Firewall (ports 80, 443)        │
│  • DDoS protection                  │
└──────────────┬──────────────────────┘
               ▼
┌─────────────────────────────────────┐
│  IIS Layer                          │
│  • SSL/TLS certificates             │
│  • Request filtering                │
│  • IP restrictions (optional)       │
└──────────────┬──────────────────────┘
               ▼
┌─────────────────────────────────────┐
│  Application Layer                  │
│  • JWT authentication               │
│  • Role-based authorization         │
│  • CORS policies                    │
│  • Input validation                 │
└──────────────┬──────────────────────┘
               ▼
┌─────────────────────────────────────┐
│  Data Layer                         │
│  • Encrypted connections            │
│  • Parameterized queries            │
│  • Database firewall                │
└─────────────────────────────────────┘
```

## Monitoring & Logging

```
┌──────────────────────────────────────────────────────────┐
│                    Application Logs                      │
│  Location: C:\inetpub\wwwroot\FSH.Starter.API\logs      │
│  • stdout_[timestamp].log                                │
│  • Application events                                    │
│  • Errors and warnings                                   │
└──────────────────────────────────────────────────────────┘
                            │
                            ▼
┌──────────────────────────────────────────────────────────┐
│                       IIS Logs                           │
│  Location: C:\inetpub\logs\LogFiles\W3SVC*\            │
│  • Request logs                                          │
│  • Response codes                                        │
│  • Performance metrics                                   │
└──────────────────────────────────────────────────────────┘
                            │
                            ▼
┌──────────────────────────────────────────────────────────┐
│                Windows Event Viewer                      │
│  • Application log                                       │
│  • System log                                            │
│  • Security log                                          │
└──────────────────────────────────────────────────────────┘
```

## Scaling Options

### Vertical Scaling (Single Server)
```
┌──────────────────────────┐
│  Increase server specs   │
│  • More CPU cores        │
│  • More RAM              │
│  • Faster disk (SSD)     │
└──────────────────────────┘
```

### Horizontal Scaling (Multiple Servers)
```
               ┌──────────────────┐
               │  Load Balancer   │
               └────────┬─────────┘
                        │
        ┌───────────────┼───────────────┐
        ▼               ▼               ▼
   ┌─────────┐    ┌─────────┐    ┌─────────┐
   │ IIS #1  │    │ IIS #2  │    │ IIS #3  │
   └────┬────┘    └────┬────┘    └────┬────┘
        │              │              │
        └──────────────┼──────────────┘
                       ▼
            ┌──────────────────┐
            │  Database Server │
            └──────────────────┘
```

## Backup Strategy

```
┌──────────────────────────────────────┐
│  1. Application Files Backup         │
│     • C:\inetpub\wwwroot\*          │
│     • Frequency: Before each deploy  │
└──────────────┬───────────────────────┘
               ▼
┌──────────────────────────────────────┐
│  2. Configuration Backup             │
│     • appsettings.json              │
│     • web.config                     │
│     • IIS bindings                   │
│     • Frequency: Weekly              │
└──────────────┬───────────────────────┘
               ▼
┌──────────────────────────────────────┐
│  3. Database Backup                  │
│     • Full backup: Daily             │
│     • Transaction log: Hourly        │
│     • Retention: 30 days             │
└──────────────────────────────────────┘
```

## Update Process

```
┌──────────────────┐
│ 1. Maintenance   │
│    Window        │
│    Notification  │
└────────┬─────────┘
         ▼
┌──────────────────┐
│ 2. Backup        │
│    Current       │
│    Version       │
└────────┬─────────┘
         ▼
┌──────────────────┐
│ 3. Stop App      │
│    Pool          │
└────────┬─────────┘
         ▼
┌──────────────────┐
│ 4. Deploy New    │
│    Version       │
└────────┬─────────┘
         ▼
┌──────────────────┐
│ 5. Run           │
│    Migrations    │
│    (if needed)   │
└────────┬─────────┘
         ▼
┌──────────────────┐
│ 6. Start App     │
│    Pool          │
└────────┬─────────┘
         ▼
┌──────────────────┐
│ 7. Verify        │
│    Deployment    │
└────────┬─────────┘
         ▼
┌──────────────────┐
│ 8. Monitor       │
│    Logs          │
└──────────────────┘
```

---

This architecture provides:
- ✅ Separation of concerns (API vs UI)
- ✅ Scalability options
- ✅ Security best practices
- ✅ Comprehensive logging
- ✅ Easy maintenance
- ✅ Backup and recovery procedures

