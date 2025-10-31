# NSwag API Client Generation Scripts

This directory contains scripts to regenerate the NSwag API clients for the Blazor application.

## Available Scripts

### 1. `nswag-regen.sh` (Interactive)
**Bash shell script** - Interactive version with prompts and detailed output.

```bash
# Make executable (first time only)
chmod +x ./nswag-regen.sh

# Run the script
./nswag-regen.sh
```

**Features:**
- Interactive prompts to ensure API is running
- Detailed progress messages
- Cross-platform (Linux/macOS)
- Equivalent to the PowerShell version

### 2. `nswag-quick.sh` (Non-Interactive)
**Bash shell script** - Quick, non-interactive version for automation.

```bash
# Make executable (first time only)
chmod +x ./nswag-quick.sh

# Run the script
./nswag-quick.sh
```

**Features:**
- No interactive prompts
- Faster execution
- Suitable for CI/CD pipelines
- Clean, concise output

### 3. `nswag-regen.ps1` (PowerShell)
**PowerShell script** - Cross-platform PowerShell version.

```powershell
# Run from any directory
pwsh ./nswag-regen.ps1

# Or from project root
pwsh apps/blazor/scripts/nswag-regen.ps1
```

**Features:**
- Works on Windows, Linux, macOS
- Interactive prompts
- Official PowerShell implementation

## Prerequisites

Before running any script, ensure:

1. **API Server is Running**
   - Start the WebAPI project
   - Should be accessible at `https://localhost:7000`
   - Swagger endpoint should be available at `https://localhost:7000/swagger/v1/swagger.json`

2. **.NET SDK Installed**
   - .NET 9.0 or later
   - Verify: `dotnet --version`

3. **NSwag CLI Installed** (optional, for direct nswag commands)
   ```bash
   dotnet tool install -g NSwag.ConsoleCore
   ```

## What Gets Generated

The scripts generate three API client files in `src/apps/blazor/infrastructure/Api/`:

1. **Client.Core.cs** - Core/Identity endpoints
2. **Client.Accounting.cs** - Accounting module endpoints
3. **Client.Store.cs** - Store module endpoints

These files are auto-generated from the OpenAPI/Swagger specification and should **not be manually edited**.

## Configuration

The NSwag configuration is defined in:
```
src/apps/blazor/infrastructure/Api/nswag.json
```

Key settings:
- **Runtime**: .NET 9.0
- **Swagger URL**: `https://localhost:7000/swagger/v1/swagger.json`
- **Operation Mode**: Multiple clients from operation ID
- **Client Class**: `Client` (partial classes)
- **Namespace Pattern**: `FSH.Starter.Blazor.Infrastructure.Api.{Module}`

## Usage Examples

### Regenerate after API changes:
```bash
# Start the API server first
cd src/api/server
dotnet run

# In a new terminal, regenerate clients
cd src/apps/blazor/scripts
./nswag-quick.sh
```

### From project root:
```bash
# Using relative path
./apps/blazor/scripts/nswag-quick.sh

# Or using full path
bash /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/apps/blazor/scripts/nswag-quick.sh
```

### Automated in build process:
The Infrastructure.csproj includes a custom MSBuild target that runs NSwag:
```bash
dotnet build -t:NSwag src/apps/blazor/infrastructure/Infrastructure.csproj
```

## Troubleshooting

### "Connection refused" error
- Ensure the API server is running
- Check if the API is accessible at `https://localhost:7000`
- Verify the Swagger endpoint loads in browser

### "nswag command not found"
- The scripts use `dotnet build -t:NSwag` which doesn't require NSwag CLI
- If using direct nswag commands, install: `dotnet tool install -g NSwag.ConsoleCore`

### Permission denied
```bash
chmod +x ./nswag-regen.sh
chmod +x ./nswag-quick.sh
```

### Git repository not found
- Ensure you're running the script from within the git repository
- The scripts use `git rev-parse --show-toplevel` to find the root directory

## Integration with IDE

### Visual Studio Code
Add to `.vscode/tasks.json`:
```json
{
  "label": "Regenerate NSwag Clients",
  "type": "shell",
  "command": "${workspaceFolder}/apps/blazor/scripts/nswag-quick.sh",
  "problemMatcher": []
}
```

### JetBrains Rider
- Right-click on the script file
- Select "Run 'nswag-quick.sh'"
- Or configure as an External Tool

## Manual Regeneration

If you prefer manual control:

```bash
cd src/apps/blazor/infrastructure/Api
nswag run nswag.json
```

Or using environment variables:
```bash
export ASPNETCORE_ENVIRONMENT=Development
nswag run nswag.json /variables:Configuration=Debug
```

## Related Documentation

- [NSwag Documentation](https://github.com/RicoSuter/NSwag)
- [OpenAPI Specification](https://swagger.io/specification/)
- Project-specific docs:
  - `NSWAG_SPLIT_CLIENTS_TROUBLESHOOTING.md`
  - `README_NSWAG_SPLIT_CLIENT.md`
  - `SPLIT_API_CLIENT_ARCHITECTURE.md`

## Notes

- **Auto-generated files**: Never manually edit Client.*.cs files
- **Version control**: The generated files are committed to git
- **Build integration**: NSwag runs automatically during infrastructure project build
- **Development workflow**: Regenerate clients after any API contract changes

