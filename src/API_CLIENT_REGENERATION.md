# API Client Regeneration Guide

## Issue
The Blazor API client (`Client.cs`) needs to be regenerated to include the new `Image` and `ImageUrl` properties added to the Item and Supplier commands on the backend.

## Prerequisites
- The API server must be running on https://localhost:7000
- The backend changes (Item and Supplier commands with Image properties) must be compiled

## Steps to Regenerate

### Step 1: Start the API Server
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/server
dotnet run
```

Keep this terminal running. The API should be accessible at https://localhost:7000

### Step 2: Regenerate the Client (in a new terminal)
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/apps/blazor/infrastructure
dotnet build /t:NSwag
```

This will:
1. Fetch the Swagger/OpenAPI spec from https://localhost:7000/swagger/v1/swagger.json
2. Generate the Client.cs file with updated DTOs and commands
3. Include the new Image and ImageUrl properties

### Step 3: Stop the API Server
Return to the first terminal and press Ctrl+C to stop the server.

### Step 4: Build the Blazor Project
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src
dotnet build apps/blazor/client/Client.csproj
```

## What Will Change
After regeneration, the following types will include `ImageUrl` and `Image` properties:

- `UpdateItemCommand` - will have `Image` and `ImageUrl` properties
- `CreateItemCommand` - will have `Image` and `ImageUrl` properties  
- `ItemResponse` - will have `ImageUrl` property
- `UpdateSupplierCommand` - will have `Image` and `ImageUrl` properties
- `CreateSupplierCommand` - will have `Image` and `ImageUrl` properties
- `SupplierResponse` - will have `ImageUrl` property

## Temporary Workaround (Currently Implemented)
Until the API client is regenerated, we've created partial class extensions:
- `ItemResponseExtension.cs` - adds `ImageUrl` to `ItemResponse`
- `SupplierResponseExtension.cs` - adds `ImageUrl` to `SupplierResponse`
- `ItemViewModel.cs` - extends `UpdateItemCommand` with Image support
- `SupplierViewModel.cs` - extends `UpdateSupplierCommand` with Image support

**These extension files can be removed after successful API client regeneration.**

## Verification
After regeneration:
1. ✅ No compilation errors in the Blazor project
2. ✅ The `*Extension.cs` files can be removed
3. ✅ The ViewModel files can be simplified to just inherit from UpdateCommand (like CategoryViewModel)
4. ✅ Image upload works in both Items and Suppliers pages
5. ✅ Images display correctly in the tables

## Troubleshooting
- **Error: Connection refused** - Make sure the API server is running on https://localhost:7000
- **Error: Swagger endpoint not found** - Check that the API server has Swagger enabled
- **Build errors after regeneration** - Clean and rebuild: `dotnet clean && dotnet build`

