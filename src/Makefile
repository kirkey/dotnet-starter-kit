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
#   apps/blazor/infrastructure/Api/ApiClient.cs

.PHONY: help install-nswag dev-certs swagger-check gen-client gen-client-direct openapi-download api-run print-swagger-url clean-client nswag-version

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
	@echo "  make clean-client         Remove the generated ApiClient.cs file."

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
	@echo "Client generated. See apps/blazor/infrastructure/Api/ApiClient.cs"

# Fallback: generate via CLI with explicit switches (kept close to existing config)
# You can override output with: make gen-client-direct OUTPUT=path/to/File.cs
OUTPUT?=apps/blazor/infrastructure/Api/ApiClient.cs
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
	rm -f apps/blazor/infrastructure/Api/ApiClient.cs
