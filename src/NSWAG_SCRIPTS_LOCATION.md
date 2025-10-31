# NSwag Client Generation - Quick Reference

## Location
`src/apps/blazor/scripts/`

## Available Scripts

| Script | Type | Use Case |
|--------|------|----------|
| **nswag-regen.sh** | Bash (Interactive) | Development with prompts |
| **nswag-quick.sh** | Bash (Non-interactive) | Quick regeneration, CI/CD |
| **nswag-regen.ps1** | PowerShell | Windows/cross-platform |

## Quick Start

### macOS/Linux
```bash
cd src/apps/blazor/scripts
./nswag-quick.sh
```

### Windows (PowerShell)
```powershell
cd src/apps/blazor/scripts
pwsh ./nswag-regen.ps1
```

### From Project Root
```bash
./apps/blazor/scripts/nswag-quick.sh
```

## Prerequisites
1. Start API server: `cd src/api/server && dotnet run`
2. Verify Swagger at: https://localhost:7000/swagger/v1/swagger.json
3. Run regeneration script

## What It Does
Generates API client files in `src/apps/blazor/infrastructure/Api/`:
- Client.Core.cs
- Client.Accounting.cs
- Client.Store.cs

## Documentation
See `src/apps/blazor/scripts/README.md` for detailed information.

## Troubleshooting

**Permission denied?**
```bash
chmod +x src/apps/blazor/scripts/*.sh
```

**API not running?**
Start the server first: `cd src/api/server && dotnet run`

**Need to regenerate after API changes?**
Always run the appropriate script after modifying API endpoints.

