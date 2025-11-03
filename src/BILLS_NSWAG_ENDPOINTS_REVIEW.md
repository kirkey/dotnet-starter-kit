# Bills and Bill Line Items NSwag Endpoints Review

**Date:** November 3, 2025  
**Status:** ✅ All Endpoints Are Generated Correctly

## Summary

All Bills and Bill Line Items endpoints are **already included** in the generated NSwag client (`/apps/blazor/infrastructure/Api/Client.cs`). The endpoints are properly registered in the backend and are being generated correctly.

## Issue Identified

The Blazor UI code (`Bills.razor.cs`) is using **incorrect method names** that don't match the generated NSwag client. This is causing compilation or runtime errors.

---

## Bills Endpoints

### ✅ Backend Endpoints (Properly Registered)

Location: `/api/modules/Accounting/Accounting.Infrastructure/Endpoints/Bills/v1/`

1. **BillCreateEndpoint** - POST `/accounting/v1/bills/`
2. **BillUpdateEndpoint** - PUT `/accounting/v1/bills/{id}`
3. **DeleteBillEndpoint** - DELETE `/accounting/v1/bills/{id}`
4. **GetBillEndpoint** - GET `/accounting/v1/bills/{id}`
5. **SearchBillsEndpoint** - POST `/accounting/v1/bills/search`
6. **ApproveBillEndpoint** - POST `/accounting/v1/bills/{id}/approve`
7. **RejectBillEndpoint** - POST `/accounting/v1/bills/{id}/reject`
8. **PostBillEndpoint** - POST `/accounting/v1/bills/{id}/post`
9. **MarkBillAsPaidEndpoint** - POST `/accounting/v1/bills/{id}/mark-as-paid`
10. **VoidBillEndpoint** - POST `/accounting/v1/bills/{id}/void`

### ✅ Generated NSwag Client Methods

All methods are in: `/apps/blazor/infrastructure/Api/Client.cs`

| Endpoint | Generated Method Name | Parameters |
|----------|----------------------|------------|
| Create | `BillCreateEndpointAsync` | `string version, BillCreateCommand body` |
| Update | `BillUpdateEndpointAsync` | `string version, Guid id, BillUpdateCommand body` |
| Delete | `DeleteBillEndpointAsync` | `string version, Guid id` |
| Get | `GetBillEndpointAsync` | `string version, Guid id` |
| Search | `SearchBillsEndpointAsync` | `string version, SearchBillsCommand body` |
| Approve | `ApproveBillEndpointAsync` | `string version, Guid id, ApproveBillRequest body` |
| Reject | `RejectBillEndpointAsync` | `string version, Guid id, RejectBillRequest body` |
| Post | `PostBillEndpointAsync` | `string version, Guid id` |
| Mark as Paid | `MarkBillAsPaidEndpointAsync` | `string version, Guid id, MarkBillAsPaidRequest body` |
| Void | `VoidBillEndpointAsync` | `string version, Guid id, VoidBillRequest body` |

### ❌ Incorrect Names Used in Blazor Code

The `Bills.razor.cs` file is calling methods with **different names** than what's generated:

| Incorrect Name (Used) | Correct Name (Generated) |
|----------------------|-------------------------|
| `BillsSearchEndpointAsync` | `SearchBillsEndpointAsync` |
| `BillsCreateEndpointAsync` | `BillCreateEndpointAsync` |
| `BillsUpdateEndpointAsync` | `BillUpdateEndpointAsync` |
| `BillsDeleteEndpointAsync` | `DeleteBillEndpointAsync` |
| `BillsGetByIdEndpointAsync` | `GetBillEndpointAsync` |
| `BillsApproveEndpointAsync` | `ApproveBillEndpointAsync` |
| `BillsRejectEndpointAsync` | `RejectBillEndpointAsync` |
| `BillsPostEndpointAsync` | `PostBillEndpointAsync` |
| `BillsMarkAsPaidEndpointAsync` | `MarkBillAsPaidEndpointAsync` |
| `BillsVoidEndpointAsync` | `VoidBillEndpointAsync` |

---

## Bill Line Items Endpoints

### ✅ Backend Endpoints (Properly Registered)

Location: `/api/modules/Accounting/Accounting.Infrastructure/Endpoints/Bills/LineItems/v1/`

1. **AddBillLineItemEndpoint** - POST `/accounting/v1/bills/{billId}/line-items`
2. **GetBillLineItemsEndpoint** - GET `/accounting/v1/bills/{billId}/line-items`
3. **GetBillLineItemEndpoint** - GET `/accounting/v1/bills/{billId}/line-items/{lineItemId}`
4. **UpdateBillLineItemEndpoint** - PUT `/accounting/v1/bills/{billId}/line-items/{lineItemId}`
5. **DeleteBillLineItemEndpoint** - DELETE `/accounting/v1/bills/{billId}/line-items/{lineItemId}`

### ✅ Generated NSwag Client Methods

| Endpoint | Generated Method Name | Parameters |
|----------|----------------------|------------|
| Add | `AddBillLineItemEndpointAsync` | `string version, Guid billId, AddBillLineItemCommand body` |
| Get List | `GetBillLineItemsEndpointAsync` | `string version, Guid billId` |
| Get Single | `GetBillLineItemEndpointAsync` | `string version, Guid billId, Guid lineItemId` |
| Update | `UpdateBillLineItemEndpointAsync` | `string version, Guid billId, Guid lineItemId, UpdateBillLineItemCommand body` |
| Delete | `DeleteBillLineItemEndpointAsync` | `string version, Guid billId, Guid lineItemId` |

---

## Response/Request DTOs Generated

All the following types are properly generated in the NSwag client:

### Bills
- `BillCreateCommand`
- `BillCreateResponse`
- `BillUpdateCommand`
- `UpdateBillResponse`
- `DeleteBillResponse`
- `BillResponse`
- `BillResponsePagedList`
- `SearchBillsCommand`
- `ApproveBillRequest`
- `ApproveBillResponse`
- `RejectBillRequest`
- `RejectBillResponse`
- `PostBillResponse`
- `MarkBillAsPaidRequest`
- `MarkBillAsPaidResponse`
- `VoidBillRequest`
- `VoidBillResponse`

### Bill Line Items
- `AddBillLineItemCommand`
- `AddBillLineItemResponse`
- `BillLineItemResponse`
- `UpdateBillLineItemCommand`
- `UpdateBillLineItemResponse`
- `DeleteBillLineItemResponse`

---

## Action Required

### 1. Fix Bills.razor.cs
Update all method calls to use the **correct generated method names** as shown in the table above.

### 2. No NSwag Regeneration Needed
The NSwag client is already up-to-date with all Bills and Bill Line Items endpoints. **Do not regenerate** unless other endpoints are missing.

### 3. Verify Endpoint Registration
The endpoints are properly registered in:
- `AccountingModule.cs` → calls `MapBillsEndpoints()`
- `BillsEndpoints.cs` → registers all Bill and Line Item endpoints

---

## NSwag Configuration

**NSwag Config File:** `/apps/blazor/infrastructure/Api/nswag.json`  
**Generated Client:** `/apps/blazor/infrastructure/Api/Client.cs`  
**Scripts Location:** `/apps/blazor/scripts/`

To regenerate (if needed in the future):
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/apps/blazor/scripts
./nswag-regen.sh
```

---

## Verification Checklist

- [x] Bills endpoints registered in backend
- [x] Bill Line Items endpoints registered in backend
- [x] All endpoints included in AccountingModule
- [x] NSwag client contains all Bill methods
- [x] NSwag client contains all Bill Line Item methods
- [x] All request/response DTOs generated
- [ ] Bills.razor.cs updated to use correct method names *(Action Required)*

---

## Conclusion

✅ **All Bills and Bill Line Items endpoints are properly included in the NSwag client.**  

The only issue is that the Blazor UI code is using incorrect method names. This will be fixed by updating the Bills.razor.cs file to match the generated client method names.

No NSwag regeneration is required.

