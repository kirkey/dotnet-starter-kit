# 🏗️ Blazor WebAssembly IIS Deployment Architecture

## 📊 Deployment Overview

```
┌─────────────────────────────────────────────────────────────────────┐
│                        YOUR MAC (Development)                       │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/             │
│  │                                                                  │
│  ├─ apps/blazor/client/          (Source Code)                    │
│  │   ├─ Pages/                                                     │
│  │   ├─ Components/                                                │
│  │   └─ Client.csproj                                              │
│  │                                                                  │
│  │   📦 PUBLISH PROCESS                                            │
│  │   └─> make publish-blazor                                       │
│  │                                                                  │
│  └─ publishfsh9/blazor/          (Published Output)               │
│      ├─ index.html                                                 │
│      ├─ appsettings.json         ⚠️ Configure API URL here!       │
│      ├─ web.config               (IIS configuration)              │
│      └─ _framework/              (105 .wasm files = Your DLLs!)   │
│                                                                     │
│  📦 CREATE ZIP                                                      │
│  └─> make create-deployment-packages                               │
│                                                                     │
│  📤 OUTPUT: FSH.Starter.Blazor.zip (42 MB → 15-20 MB compressed)   │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
                              │
                              │ Transfer via:
                              │ • RDP (Copy/Paste)
                              │ • FTP/SFTP
                              │ • OneDrive/Cloud
                              │ • Network Share
                              ▼
┌─────────────────────────────────────────────────────────────────────┐
│                      WINDOWS SERVER (Production)                    │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  C:\inetpub\wwwroot\fsh-blazor\   (IIS wwwroot)                    │
│  │                                                                  │
│  ├─ index.html                    ← Entry point                    │
│  ├─ appsettings.json              ← API URL configuration          │
│  ├─ web.config                    ← IIS URL rewrite rules          │
│  ├─ _framework\                   ← 300+ files                     │
│  │   ├─ blazor.webassembly.js    ← Blazor loader                  │
│  │   ├─ dotnet.native.*.wasm     ← .NET Runtime (2.7 MB)          │
│  │   └─ *.wasm                    ← Your app assemblies (105)      │
│  ├─ _content\                     ← Library assets                 │
│  ├─ css\                                                            │
│  └─ js\                                                             │
│                                                                     │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ IIS (Internet Information Services)                          │ │
│  ├───────────────────────────────────────────────────────────────┤ │
│  │                                                               │ │
│  │  Application Pool: FSH.Blazor.AppPool                        │ │
│  │  └─ .NET CLR: No Managed Code (Static files only!)          │ │
│  │                                                               │ │
│  │  Website: FSH Blazor Client                                  │ │
│  │  ├─ Physical Path: C:\inetpub\wwwroot\fsh-blazor\           │ │
│  │  ├─ Binding: http://*:80 or https://*:443                   │ │
│  │  ├─ URL Rewrite: SPA routing enabled                        │ │
│  │  └─ Compression: Brotli + Gzip enabled                      │ │
│  │                                                               │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                              │                                      │
│                              │ Serves static files                  │
│                              ▼                                      │
└─────────────────────────────────────────────────────────────────────┘
                              │
                              │ HTTP/HTTPS
                              ▼
┌─────────────────────────────────────────────────────────────────────┐
│                         CLIENT BROWSER                              │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  1. Browser requests: http://your-server.com/                      │
│     └─> IIS serves: index.html                                     │
│                                                                     │
│  2. index.html loads: blazor.webassembly.js                        │
│                                                                     │
│  3. blazor.webassembly.js downloads:                               │
│     ├─ blazor.boot.json         (manifest)                        │
│     ├─ dotnet.native.*.wasm     (.NET runtime)                    │
│     └─ All app .wasm files      (your DLLs)                       │
│                                                                     │
│  4. WebAssembly Runtime Starts                                     │
│     └─> Loads .NET 9 runtime in browser                           │
│                                                                     │
│  5. Your C# Code Executes CLIENT-SIDE! 🎉                         │
│     └─> Blazor app running in browser                             │
│                                                                     │
│  6. App makes API calls to:                                        │
│     └─> https://api.yourdomain.com/                               │
│         (configured in appsettings.json)                           │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

---

## 🔄 Request Flow Diagram

### Initial Page Load

```
User enters URL
    │
    ▼
┌───────────────────────────────────────┐
│ Browser: GET http://blazor.domain.com │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ Windows Server: IIS receives request │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ IIS: Serves index.html                │
│ (Static file from wwwroot)            │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ Browser: Parses index.html            │
│ Finds: <script> blazor.webassembly.js │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ Browser: Downloads blazor.webassembly │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ Blazor Loader: Downloads boot.json   │
│ (Manifest of all .wasm files needed)  │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ Blazor: Downloads ALL .wasm files     │
│ • dotnet.native.wasm (runtime)        │
│ • Your app assemblies (105 files)     │
│ Total: ~17 MB (Brotli compressed)     │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ WebAssembly Runtime: Initializes      │
│ .NET 9 runtime loads in browser!      │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ Blazor App: Your C# code executes     │
│ CLIENT-SIDE in the browser            │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ App Ready! User can interact          │
└───────────────────────────────────────┘
```

---

### Navigation / Routing

```
User clicks link: /products
    │
    ▼
┌───────────────────────────────────────┐
│ Blazor Router: CLIENT-SIDE routing    │
│ No server request needed!             │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ Renders Products page component       │
│ (Already loaded in .wasm files)       │
└───────────────────────────────────────┘

User presses F5 (refresh) on /products
    │
    ▼
┌───────────────────────────────────────┐
│ Browser: GET /products                │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ IIS: URL Rewrite Module kicks in      │
│ Rewrites /products → index.html       │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ Serves index.html again               │
│ Blazor loads, router navigates to     │
│ /products automatically               │
└───────────────────────────────────────┘
```

---

### API Call Flow

```
User logs in or fetches data
    │
    ▼
┌───────────────────────────────────────┐
│ Blazor C# Code (in browser):          │
│ var response = await httpClient       │
│   .GetAsync("/api/products");         │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ HttpClient uses ApiBaseUrl from       │
│ appsettings.json                      │
│ https://api.yourdomain.com            │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ Browser: Makes HTTP request to API    │
│ GET https://api.yourdomain.com/       │
│     api/products                      │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ API Server (could be same or          │
│ different server):                    │
│ • Validates request                   │
│ • Checks CORS (must allow Blazor URL) │
│ • Processes request                   │
│ • Returns JSON response               │
└───────────────────────────────────────┘
    │
    ▼
┌───────────────────────────────────────┐
│ Blazor receives response               │
│ Updates UI with data                   │
└───────────────────────────────────────┘
```

---

## 🎯 Key Architecture Points

### 1. **Static File Hosting**
```
IIS Role: Static File Server
├─ NO .NET Runtime execution on server
├─ NO server-side code execution
├─ Just serves files like Apache/Nginx
└─ URL Rewrite for SPA routing
```

### 2. **Client-Side Execution**
```
Browser Role: Application Host
├─ Downloads .wasm files (~17 MB compressed)
├─ Loads .NET Runtime in WebAssembly
├─ Executes your C# code
└─ Makes API calls as needed
```

### 3. **File Types**
```
Published Folder Contents:
├─ .html files      → Browser renders
├─ .css files       → Browser styles
├─ .js files        → Browser executes
├─ .wasm files      → WebAssembly executes
├─ .json files      → Configuration/data
└─ .br/.gz files    → Compressed versions
```

### 4. **Compression Strategy**
```
Each file has 3 versions:
├─ myfile.wasm      (original, 1.0 MB)
├─ myfile.wasm.br   (Brotli, 0.4 MB) ← IIS serves this first
└─ myfile.wasm.gz   (Gzip, 0.5 MB)   ← Fallback
```

---

## 🔧 Configuration Files Explained

### appsettings.json (Client Configuration)
```json
{
  "ApiBaseUrl": "https://api.yourdomain.com/"
}
```
**Purpose**: Tells Blazor app where to find the API
**Location**: `C:\inetpub\wwwroot\fsh-blazor\appsettings.json`
**Downloaded**: YES, by browser (public file!)
**Important**: Must be correct for API calls to work

### web.config (IIS Configuration)
```xml
<configuration>
  <system.webServer>
    <!-- MIME types for .wasm files -->
    <staticContent>
      <mimeMap fileExtension=".wasm" mimeType="application/wasm" />
    </staticContent>
    
    <!-- URL Rewrite for SPA routing -->
    <rewrite>
      <rules>
        <rule name="SPA fallback routing">
          <!-- /products → index.html -->
        </rule>
      </rules>
    </rewrite>
    
    <!-- Compression -->
    <httpCompression>
      <!-- Serve .br and .gz files -->
    </httpCompression>
  </system.webServer>
</configuration>
```
**Purpose**: Configures IIS behavior
**Location**: `C:\inetpub\wwwroot\fsh-blazor\web.config`
**Read by**: IIS (not downloaded by browser)

### blazor.boot.json (Boot Manifest)
```json
{
  "mainAssemblyName": "FSH.Starter.Blazor.Client",
  "resources": {
    "assembly": {
      "FSH.Starter.Blazor.Client.*.wasm": "...",
      "MudBlazor.*.wasm": "...",
      /* ... 105 assemblies ... */
    }
  }
}
```
**Purpose**: Tells Blazor loader which .wasm files to download
**Location**: `_framework/blazor.boot.json`
**Downloaded**: YES, by blazor.webassembly.js
**Auto-generated**: Created during publish

---

## 🆚 Blazor Server vs Blazor WebAssembly

### Blazor Server (NOT what you're deploying)
```
Browser                    Server
  │                          │
  ├──────── SignalR ─────────┤
  │      (WebSocket)          │
  │                          │
  │ UI Events              Execute
  │ ────────────────────────> C# Code
  │                          │
  │                       Update
  │ <──────────────────────  UI
  │                          │
```
**Pros**: Small download, SEO friendly
**Cons**: Needs persistent connection, server load

### Blazor WebAssembly (What you're deploying!)
```
Browser                    Server (IIS)
  │                          │
  │ GET index.html           │
  │ ────────────────────────>│
  │ <────────────────────────│
  │                          │
  │ Download .wasm files     │
  │ ────────────────────────>│
  │ <────────────────────────│
  │                          │
  │ Execute C# in browser!   │
  │ (No server needed)       │
  │                          │
  │ API calls only           │
  │ ────────────────────────>│ API Server
  │ <────────────────────────│
```
**Pros**: Offline capable, scales easily, no server load
**Cons**: Large initial download, no SEO (without pre-rendering)

---

## 📊 Performance Characteristics

### First Load (Cache Empty)
```
Timeline:
0s    - Request index.html
0.1s  - HTML loads, starts downloading .js
0.5s  - blazor.webassembly.js executes
1s    - Downloads blazor.boot.json
1-3s  - Downloads all .wasm files (parallel)
3-5s  - WebAssembly runtime initializes
5s    - App ready! 🎉

Total Download: ~17 MB (Brotli compressed)
Time: 3-5 seconds (on good connection)
```

### Subsequent Loads (Cached)
```
Timeline:
0s    - Check cache
0.1s  - All files cached, nothing to download
0.2s  - App ready! 🎉

Total Download: < 1 KB (just checks)
Time: < 1 second
```

### Navigation (After Load)
```
Timeline:
0s    - User clicks link
0s    - Instant! (Client-side routing)

No server roundtrip!
```

---

## ✅ Deployment Success Indicators

### In IIS Manager
```
✅ Site Status: Started (green)
✅ Application Pool: Running
✅ Bindings: Configured (port 80/443)
✅ Physical Path: Points to correct folder
```

### In Browser
```
✅ Page loads without errors
✅ Console: No red errors (F12)
✅ Network tab: .wasm.br files served (compressed)
✅ Application tab: Service Worker registered (PWA)
✅ Routes work (/products, /login, etc.)
✅ Refresh works (no 404)
```

### In Windows Server
```
✅ Files extracted: 390 files in wwwroot
✅ URL Rewrite Module: Installed
✅ Firewall: Port 80/443 open
✅ Permissions: IIS_IUSRS has Read access
```

---

**Architecture Review Date**: October 28, 2025
**Technology**: Blazor WebAssembly + IIS
**Status**: ✅ Ready for Production Deployment

This architecture provides:
- ✅ Zero server-side execution
- ✅ High scalability (static files only)
- ✅ Offline capable (PWA)
- ✅ Fast after initial load (caching)
- ✅ Simple deployment (just files!)

