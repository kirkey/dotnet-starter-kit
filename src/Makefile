# NSwag client generation Makefile
# Location: repo root (src)

# Ensure global dotnet tools (e.g., nswag) are on PATH for Make recipes (best effort)
export PATH := $(HOME)/.dotnet/tools:$(PATH)

# Config
# Use explicit path to the global NSwag tool to avoid PATH propagation issues in some shells
NSWAG_BIN?=$(HOME)/.dotnet/tools/nswag
# NSwag runtime to use when running a config file (must match nswag.json "runtime")
NSWAG_RUNTIME?=Net80
# Path to the existing NSwag config (relative to this Makefile at repo root)
NSWAG_JSON:=apps/blazor/infrastructure/Api/nswag.json
API_URL?=https://localhost:7000
SWAGGER_PATH:=/swagger/v1/swagger.json
SWAGGER_URL:=$(API_URL)$(SWAGGER_PATH)

# Output path used by the config is relative to NSWAG_JSON directory:
#   apps/blazor/infrastructure/Api/Client.cs

.PHONY: help install-nswag dev-certs swagger-check gen-client gen-client-direct openapi-download api-run print-swagger-url clean-client nswag-version publish-all publish-api publish-blazor create-deployment-packages clean-publish clean-all

# Publish configuration
PUBLISH_OUTPUT?=./publishfsh9
API_PUBLISH_PATH=$(PUBLISH_OUTPUT)/api
BLAZOR_PUBLISH_PATH=$(PUBLISH_OUTPUT)/blazor
PUBLISH_CONFIG?=Release

help:
	@echo "NSwag client generation targets:"
	@echo "  make install-nswag        Install/Update NSwag CLI (dotnet tool)."
	@echo "  make dev-certs            Trust local HTTPS dev certificate (macOS)."
	@echo "  make swagger-check        Verify the API's swagger is reachable at $(SWAGGER_URL)."
	@echo "  make gen-client           Generate client via existing NSwag config: $(NSWAG_JSON)."
	@echo "  make gen-client-direct    Generate client directly from $(SWAGGER_URL) with CLI options (fallback)."
	@echo "  make openapi-download     Download swagger.json locally for inspection."
	@echo "  make api-run              Run the API using the Kestrel profile (foreground)."
	@echo "  make print-swagger-url    Print the effective swagger URL used by these targets."
	@echo "  make nswag-version        Print NSwag binary path and version."
	@echo "  make clean-client         Remove the generated Client.cs file."
	@echo "  make clean-all            Remove all bin and obj folders recursively."
	@echo ""
	@echo "Publish targets (Windows Server IIS):"
	@echo "  make publish-all          Publish both API and Blazor client + create ZIP packages."
	@echo "  make publish-api          Publish API only to $(API_PUBLISH_PATH)."
	@echo "  make publish-blazor       Publish Blazor client only to $(BLAZOR_PUBLISH_PATH)."
	@echo "  make create-deployment-packages  Create ZIP files from published output."
	@echo "  make clean-publish        Remove the publish output folder."
	@echo "  Variables: PUBLISH_OUTPUT (default: ./publishfsh9), PUBLISH_CONFIG (default: Release)"

install-nswag:
	dotnet tool update -g Nswag.ConsoleCore

# macOS/Windows: trust the local HTTPS development certificate if curl/NSwag fails due to SSL
# Safe to re-run; no effect if already trusted.
dev-certs:
	dotnet dev-certs https --trust

print-swagger-url:
	@echo $(SWAGGER_URL)

nswag-version:
	@echo "NSwag bin: $(NSWAG_BIN)" && $(NSWAG_BIN) version | head -n 1

swagger-check:
	@echo "Checking $(SWAGGER_URL) ..." && curl -fsSLo /dev/null $(SWAGGER_URL) && echo "OK"

# Preferred: use consistent options and the configured Swagger URL
# Delegate to the direct generator to avoid runtime mismatches with `nswag run`
gen-client: gen-client-direct
	@echo "Client generated. See apps/blazor/infrastructure/Api/Client.cs"

# Fallback: generate via CLI with explicit switches (kept close to existing config)
# You can override output with: make gen-client-direct OUTPUT=path/to/File.cs
OUTPUT?=apps/blazor/infrastructure/Api/Client.cs
NAMESPACE?=FSH.Starter.Blazor.Infrastructure.Api
CLASSNAME?=Client

gen-client-direct: install-nswag swagger-check
	$(NSWAG_BIN) openapi2csclient \
	  /input:$(SWAGGER_URL) \
	  /output:$(OUTPUT) \
	  /namespace:$(NAMESPACE) \
	  /classname:$(CLASSNAME) \
	  /UseBaseUrl:false \
	  /GenerateClientInterfaces:true \
	  /InjectHttpClient:true \
	  /DisposeHttpClient:false \
	  /GenerateNullableReferenceTypes:true \
	  /ParameterDateTimeFormat:s \
	  /ParameterDateFormat:yyyy-MM-dd \
	  /DateType:System.DateTimeOffset \
	  /DateTimeType:System.DateTime \
	  /TimeType:System.TimeSpan \
	  /ArrayType:System.Collections.Generic.ICollection \
	  /ArrayInstanceType:System.Collections.ObjectModel.Collection \
	  /DictionaryType:System.Collections.Generic.IDictionary \
	  /DictionaryInstanceType:System.Collections.Generic.Dictionary \
	  /ClassStyle:Poco \
	  /JsonLibrary:SystemTextJson \
	  /GenerateDefaultValues:true \
	  /GenerateDataAnnotations:true \
	  /RequiredPropertiesMustBeDefined:true
	@echo "Client generated at $(OUTPUT)"

openapi-download:
	curl -fsSLo swagger.v1.json $(SWAGGER_URL)
	@echo "Saved to ./swagger.v1.json"

# Convenience: run the API using the Kestrel profile declared in launchSettings.json
api-run:
	dotnet run --project api/server/Server.csproj --launch-profile "Kestrel"

clean-client:
	rm -f apps/blazor/infrastructure/Api/Client.cs

# ============================================================================
# PUBLISH TARGETS FOR IIS DEPLOYMENT (Windows Server)
# ============================================================================

publish-all: publish-api publish-blazor create-deployment-packages
	@echo ""
	@echo "============================================"
	@echo "✅ Both projects published successfully!"
	@echo "============================================"
	@echo ""
	@echo "Published Artifacts:"
	@echo "  API:           $(API_PUBLISH_PATH)"
	@echo "  Blazor Client: $(BLAZOR_PUBLISH_PATH)"
	@echo ""
	@echo "Deployment Packages (Ready for Transfer):"
	@echo "  API:           $(PUBLISH_OUTPUT)/FSH.Starter.API.zip"
	@echo "  Blazor Client: $(PUBLISH_OUTPUT)/FSH.Starter.Blazor.zip"
	@echo ""
	@echo "📋 WINDOWS SERVER IIS DEPLOYMENT CHECKLIST:"
	@echo "─────────────────────────────────────────"
	@echo ""
	@echo "✓ PRE-DEPLOYMENT (On IIS Server):"
	@echo "  1. Install .NET 9 Hosting Bundle"
	@echo "     → Download from: https://dotnet.microsoft.com/download"
	@echo "  2. Install IIS URL Rewrite Module"
	@echo "  3. Ensure IIS Application Development features are enabled"
	@echo ""
	@echo "✓ API DEPLOYMENT:"
	@echo "  1. Transfer API ZIP: $(PUBLISH_OUTPUT)/FSH.Starter.API.zip"
	@echo "  2. Extract to: C:\\inetpub\\wwwroot\\api"
	@echo "  3. Create App Pool: Set .NET version to 'No Managed Code'"
	@echo "  4. Create IIS Application with:"
	@echo "     - Physical path: C:\\inetpub\\wwwroot\\api"
	@echo "     - App Pool: YourAppPoolName"
	@echo "     - Application entry point: web.config"
	@echo "  5. Edit appsettings.Production.json:"
	@echo "     - Database connection string"
	@echo "     - CORS settings (Blazor client URL)"
	@echo "     - Logging paths (ensure directory exists)"
	@echo "     - Authentication secrets"
	@echo ""
	@echo "✓ BLAZOR CLIENT DEPLOYMENT:"
	@echo "  1. Transfer Blazor ZIP: $(PUBLISH_OUTPUT)/FSH.Starter.Blazor.zip"
	@echo "  2. Extract to: C:\\inetpub\\wwwroot\\blazor"
	@echo "  3. Create IIS Static Content Application"
	@echo "  4. Update appsettings.json with API URL"
	@echo "  5. Enable compression in IIS (for better performance)"
	@echo ""
	@echo "✓ IIS CONFIGURATION:"
	@echo "  1. Set File Permissions:"
	@echo "     - Right-click folder → Properties → Security"
	@echo "     - Add IIS AppPool\\YourAppPoolName with Modify rights"
	@echo "  2. Create web.config files if missing"
	@echo "  3. Enable HTTPS (SSL Certificate)"
	@echo "  4. Configure CORS headers in API"
	@echo ""
	@echo "✓ VERIFICATION:"
	@echo "  1. Check IIS Application Event Logs"
	@echo "  2. Test API: https://yourserver/api/health"
	@echo "  3. Test Blazor: https://yourserver/blazor"
	@echo "  4. Check network requests (F12 → Network tab)"
	@echo "  5. Verify no 500 errors in browser console"
	@echo ""
	@echo "⚠️  IMPORTANT NOTES:"
	@echo "  - Ensure .NET Runtime is compatible (v9.0 required)"
	@echo "  - Database must be accessible from IIS server"
	@echo "  - Firewall rules may need adjustment"
	@echo "  - Consider fail-over/load balancing for production"
	@echo ""
	@echo "============================================"

publish-api:
	@echo ""
	@echo "============================================"
	@echo "Publishing API for Windows Server IIS"
	@echo "Configuration: $(PUBLISH_CONFIG)"
	@echo "Target: $(API_PUBLISH_PATH)"
	@echo "============================================"
	@echo ""
	@# Clean previous publish if exists
	@if [ -d "$(API_PUBLISH_PATH)" ]; then \
		rm -rf $(API_PUBLISH_PATH); \
		echo "Cleaned previous publish output"; \
	fi
	@mkdir -p $(API_PUBLISH_PATH)
	@echo "Publishing .NET application..."
	dotnet publish api/server/Server.csproj \
		-c $(PUBLISH_CONFIG) \
		-o $(API_PUBLISH_PATH) \
		--no-self-contained \
		/p:PublishProfile="" \
		/p:EnableSdkContainerSupport=false \
		/p:PublishSingleFile=false \
		/p:PublishReadyToRun=false \
		/p:PublishTrimmed=false \
		/p:DebugType=none \
		/p:DebugSymbols=false
	@echo ""
	@echo "✅ API published successfully"
	@echo "📁 Location: $(API_PUBLISH_PATH)"
	@echo ""
	@echo "IIS Deployment Instructions:"
	@echo "────────────────────────────"
	@echo "1. Copy all files from: $(API_PUBLISH_PATH)"
	@echo "2. To IIS wwwroot folder (e.g., C:\\inetpub\\wwwroot\\api)"
	@echo ""
	@echo "3. Edit appsettings.Production.json with:"
	@echo "   - Database connection strings"
	@echo "   - CORS settings for Blazor client URL"
	@echo "   - Logging configuration"
	@echo "   - JWT secret and settings"
	@echo ""
	@echo "4. In IIS Manager:"
	@echo "   - Create Application Pool: .NET v9.0 (No Managed Code)"
	@echo "   - Create Website/Application pointing to the copied files"
	@echo "   - Bind HTTPS certificate (recommended for production)"
	@echo "   - Enable URL Rewrite Module (install if needed)"
	@echo ""
	@echo "5. File Permissions (on Windows):"
	@echo "   - IIS AppPool user needs read/write access to:"
	@echo "     * Application directory"
	@echo "     * Logs folder (if applicable)"
	@echo "     * Temp folder for app cache"
	@echo ""
	@echo "6. Restart IIS:"
	@echo "   - Run: iisreset /restart"
	@echo ""
	@echo "7. Verify:"
	@echo "   - Check IIS application event logs"
	@echo "   - Test API endpoint: https://yourserver/api/health"
	@echo "============================================"

publish-blazor:
	@echo "Publishing Blazor WebAssembly for IIS ($(PUBLISH_CONFIG))..."
	@echo "Target: $(BLAZOR_PUBLISH_PATH)"
	@mkdir -p $(BLAZOR_PUBLISH_PATH)
	@# Publish to temp folder first
	dotnet publish apps/blazor/client/Client.csproj \
		-c $(PUBLISH_CONFIG) \
		-o $(BLAZOR_PUBLISH_PATH)/temp
	@# Extract wwwroot content (IIS needs only the static files)
	@echo "Extracting wwwroot content for IIS..."
	@if [ -d "$(BLAZOR_PUBLISH_PATH)/temp/wwwroot" ]; then \
		cp -r $(BLAZOR_PUBLISH_PATH)/temp/wwwroot/* $(BLAZOR_PUBLISH_PATH)/ && \
		rm -rf $(BLAZOR_PUBLISH_PATH)/temp && \
		echo "✅ Blazor client published successfully"; \
	else \
		echo "❌ Error: wwwroot folder not found in publish output"; \
		exit 1; \
	fi
	@echo "📁 Files: $(BLAZOR_PUBLISH_PATH)"

create-deployment-packages:
	@echo ""
	@echo "Creating deployment packages (ZIP files)..."
	@# Create API package
	@cd $(API_PUBLISH_PATH) && zip -r -q ../FSH.Starter.API.zip . && cd - > /dev/null
	@echo "✅ API package: $(PUBLISH_OUTPUT)/FSH.Starter.API.zip"
	@# Create Blazor package
	@cd $(BLAZOR_PUBLISH_PATH) && zip -r -q ../FSH.Starter.Blazor.zip . && cd - > /dev/null
	@echo "✅ Blazor package: $(PUBLISH_OUTPUT)/FSH.Starter.Blazor.zip"

clean-publish:
	@echo "Cleaning publish output..."
	@rm -rf $(PUBLISH_OUTPUT)
	@echo "✅ Publish folder cleaned"

# ============================================================================
# CLEAN TARGETS
# ============================================================================

clean-all:
	@echo "Removing all bin and obj folders recursively..."
	find . -type d \( -name 'bin' -o -name 'obj' \) -exec rm -rf {} +
	@echo "✅ All bin and obj folders deleted."

