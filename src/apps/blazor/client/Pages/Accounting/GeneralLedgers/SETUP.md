# General Ledger UI - Setup and API Client Generation Guide

## 🚀 Quick Setup

The General Ledger UI has been implemented but requires API client regeneration to work properly.

### Prerequisites

1. ✅ API endpoints exist in `Accounting.Infrastructure/Endpoints/GeneralLedger/`
2. ✅ API handlers exist in `Accounting.Application/GeneralLedgers/`
3. ✅ Blazor UI pages created in `apps/blazor/client/Pages/Accounting/GeneralLedgers/`
4. ⚠️ **API client needs to be regenerated** using NSwag

---

## Step 1: Start the API Server

Before regenerating the API client, ensure the API server is running:

```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/server
dotnet run
```

The API should be accessible at: `https://localhost:7000`

Verify it's running by navigating to: `https://localhost:7000/swagger`

---

## Step 2: Regenerate the API Client

Once the API is running, regenerate the Blazor API client:

### Option A: Using the Script (Recommended)

```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src
./apps/blazor/scripts/nswag-regen.sh
```

### Option B: Manual Build Command

```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/apps/blazor/client
dotnet build -t:NSwag ../infrastructure/Infrastructure.csproj
```

This will:
- Connect to the running API at `https://localhost:7000/swagger/v1/swagger.json`
- Generate the C# API client with all endpoints
- Update `/apps/blazor/infrastructure/Api/Client.cs`

---

## Step 3: Verify API Client Generation

After regeneration, verify the following methods exist in `Client.cs`:

```csharp
// In IClient interface:
Task<PagedList<GeneralLedgerSearchResponse>> GeneralLedgerSearchEndpointAsync(string version, GeneralLedgerSearchQuery query, CancellationToken cancellationToken = default);

Task<GeneralLedgerGetResponse> GeneralLedgerGetEndpointAsync(string version, DefaultIdType id, CancellationToken cancellationToken = default);

Task<DefaultIdType> GeneralLedgerUpdateEndpointAsync(string version, DefaultIdType id, GeneralLedgerUpdateCommand command, CancellationToken cancellationToken = default);
```

---

## Step 4: Uncomment UI Code

After API client regeneration, uncomment the following line in `GeneralLedgers.razor.cs`:

Find this line (around line 48):
```csharp
// new EntityField<GeneralLedgerSearchResponse>(response => response.IsPosted, "Posted", "IsPosted", typeof(bool)),
```

Uncomment it:
```csharp
new EntityField<GeneralLedgerSearchResponse>(response => response.IsPosted, "Posted", "IsPosted", typeof(bool)),
```

And update this line (around line 113):
```csharp
canUpdateEntityFunc: entity => true, // !entity.IsPosted after regeneration
```

To:
```csharp
canUpdateEntityFunc: entity => !entity.IsPosted,
```

---

## Step 5: Build and Test

Build the Blazor application:

```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/apps/blazor/client
dotnet build
```

If there are no errors, run the application:

```bash
dotnet run
```

Navigate to: `https://localhost:5001/accounting/general-ledger`

---

## Troubleshooting

### Error: "Cannot resolve symbol 'GeneralLedgerSearchEndpointAsync'"

**Cause:** API client hasn't been regenerated yet.

**Solution:** Follow Step 2 above to regenerate the API client.

---

### Error: "Unable to connect to https://localhost:7000"

**Cause:** API server is not running.

**Solution:** 
1. Start the API server (see Step 1)
2. Verify it's accessible at `https://localhost:7000/swagger`
3. Then regenerate the API client

---

### Error: "Cannot resolve symbol 'IsPosted' in GeneralLedgerSearchResponse"

**Cause:** API changes haven't propagated to the generated client yet.

**Solution:**
1. Verify the API server has the latest code with IsPosted field
2. Restart the API server
3. Regenerate the API client
4. Rebuild the Blazor project

---

### NSwag Generation Fails

If NSwag generation fails, check:

1. **API is running** at `https://localhost:7000`
2. **Swagger endpoint is accessible** at `https://localhost:7000/swagger/v1/swagger.json`
3. **NSwag.AspNetCore package** is installed in Infrastructure project
4. **nswag.json** configuration file exists in `apps/blazor/infrastructure/Api/`

---

## API Endpoint Verification

To verify the endpoints are registered correctly, check the AccountingModule.cs:

```csharp
// In Accounting.Infrastructure/AccountingModule.cs
public static IEndpointRouteBuilder MapAccountingEndpoints(this IEndpointRouteBuilder app)
{
    // ... other endpoints
    app.MapGeneralLedgerEndpoints();
    // ... other endpoints
    
    return app;
}
```

You can also verify in Swagger UI that the following endpoints exist:
- `POST /api/v1/general-ledger/search`
- `GET /api/v1/general-ledger/{id}`
- `PUT /api/v1/general-ledger/{id}`

---

## Files Created

### Blazor UI Files:
- ✅ `GeneralLedgers.razor` - Main page
- ✅ `GeneralLedgers.razor.cs` - Code-behind
- ✅ `GeneralLedgerViewModel.cs` - View model
- ✅ `GeneralLedgerDetailsDialog.razor` - Details dialog
- ✅ `README.md` - Documentation

### API Files (Already Exist):
- ✅ `GeneralLedgerSearchQuery.cs`
- ✅ `GeneralLedgerSearchResponse.cs` - **Updated with IsPosted, Source, SourceId**
- ✅ `GeneralLedgerSearchHandler.cs` - **Updated to include new fields**
- ✅ `GeneralLedgerGetResponse.cs` - **Updated with posting and source fields**
- ✅ `GeneralLedgerGetHandler.cs` - **Updated to map new fields**
- ✅ `GeneralLedgerUpdateCommand.cs`
- ✅ `GeneralLedgerSearchEndpoint.cs`
- ✅ `GeneralLedgerGetEndpoint.cs`
- ✅ `GeneralLedgerUpdateEndpoint.cs`

---

## Next Steps After Setup

Once the API client is generated and the application builds successfully:

1. **Test the page** at `/accounting/general-ledger`
2. **Post some journal entries** to populate the general ledger
3. **Test search and filtering** functionality
4. **Test the details dialog**
5. **Test edit functionality** on unposted entries
6. **Verify navigation** to journal entries works

---

## Integration with Navigation Menu

To add General Ledger to the navigation menu, add to the AccountingNavigation component:

```razor
<MudNavLink Href="/accounting/general-ledger" Icon="@Icons.Material.Filled.AccountBalance">
    General Ledger
</MudNavLink>
```

---

## Summary Checklist

- [ ] API server running at `https://localhost:7000`
- [ ] Swagger accessible at `https://localhost:7000/swagger`
- [ ] NSwag regeneration script executed successfully
- [ ] API client methods exist in `Client.cs`
- [ ] IsPosted field uncommented in `GeneralLedgers.razor.cs`
- [ ] canUpdateEntityFunc uses `!entity.IsPosted`
- [ ] Blazor app builds without errors
- [ ] Page accessible at `/accounting/general-ledger`
- [ ] Search functionality working
- [ ] Details dialog working
- [ ] Edit functionality working (unposted only)
- [ ] Navigation to journal entries working

---

**Last Updated:** November 8, 2025  
**Status:** Setup guide - follow steps above to complete implementation

