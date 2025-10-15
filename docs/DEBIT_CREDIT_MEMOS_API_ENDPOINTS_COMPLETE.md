# Accounting API Endpoints - Debit & Credit Memos Implementation

## Overview
Complete implementation of all API endpoints for Debit Memos and Credit Memos in the Accounting module. These endpoints provide full CRUD operations plus specialized workflow actions for managing receivables and payables adjustments.

---

## Files Created

### Debit Memos API Infrastructure (8 files)

#### Endpoints Configuration
1. **`DebitMemosEndpoints.cs`** - Main endpoint registration file

#### CRUD Endpoints (v1)
2. **`DebitMemoCreateEndpoint.cs`** - POST `/accounting/debit-memos`
3. **`DebitMemoUpdateEndpoint.cs`** - PUT `/accounting/debit-memos/{id}`
4. **`DebitMemoGetEndpoint.cs`** - GET `/accounting/debit-memos/{id}`
5. **`DebitMemoDeleteEndpoint.cs`** - DELETE `/accounting/debit-memos/{id}`
6. **`DebitMemoSearchEndpoint.cs`** - POST `/accounting/debit-memos/search`

#### Specialized Operation Endpoints (v1)
7. **`DebitMemoApproveEndpoint.cs`** - POST `/accounting/debit-memos/{id}/approve`
8. **`DebitMemoApplyEndpoint.cs`** - POST `/accounting/debit-memos/{id}/apply`
9. **`DebitMemoVoidEndpoint.cs`** - POST `/accounting/debit-memos/{id}/void`

### Debit Memos Application Layer (5 files)

10. **`SearchDebitMemosQuery.cs`** - Search query with 9 filters
11. **`SearchDebitMemosHandler.cs`** - Search request handler
12. **`SearchDebitMemosSpec.cs`** - Search specification with filtering
13. **`DeleteDebitMemoCommand.cs`** - Delete command
14. **`DeleteDebitMemoHandler.cs`** - Delete handler with validation

---

### Credit Memos API Infrastructure (9 files)

#### Endpoints Configuration
15. **`CreditMemosEndpoints.cs`** - Main endpoint registration file

#### CRUD Endpoints (v1)
16. **`CreditMemoCreateEndpoint.cs`** - POST `/accounting/credit-memos`
17. **`CreditMemoUpdateEndpoint.cs`** - PUT `/accounting/credit-memos/{id}`
18. **`CreditMemoGetEndpoint.cs`** - GET `/accounting/credit-memos/{id}`
19. **`CreditMemoDeleteEndpoint.cs`** - DELETE `/accounting/credit-memos/{id}`
20. **`CreditMemoSearchEndpoint.cs`** - POST `/accounting/credit-memos/search`

#### Specialized Operation Endpoints (v1)
21. **`CreditMemoApproveEndpoint.cs`** - POST `/accounting/credit-memos/{id}/approve`
22. **`CreditMemoApplyEndpoint.cs`** - POST `/accounting/credit-memos/{id}/apply`
23. **`CreditMemoRefundEndpoint.cs`** - POST `/accounting/credit-memos/{id}/refund`
24. **`CreditMemoVoidEndpoint.cs`** - POST `/accounting/credit-memos/{id}/void`

### Credit Memos Application Layer (5 files)

25. **`SearchCreditMemosQuery.cs`** - Search query with 9 filters
26. **`SearchCreditMemosHandler.cs`** - Search request handler
27. **`SearchCreditMemosSpec.cs`** - Search specification with filtering
28. **`DeleteCreditMemoCommand.cs`** - Delete command
29. **`DeleteCreditMemoHandler.cs`** - Delete handler with validation

### Module Registration (1 file)

30. **`AccountingModule.cs`** - Updated with endpoint registrations

---

## API Endpoints Reference

### Debit Memos Endpoints

#### CRUD Operations

**1. Create Debit Memo**
```http
POST /accounting/debit-memos
Content-Type: application/json
Authorization: Bearer {token}
```
**Request Body:**
```json
{
  "memoNumber": "DM-2025-001",
  "memoDate": "2025-10-15",
  "amount": 500.00,
  "referenceType": "Customer",
  "referenceId": "guid",
  "originalDocumentId": "guid (optional)",
  "reason": "Additional charges for emergency service",
  "description": "Emergency service call on 10/14",
  "notes": "Internal notes"
}
```
**Response:** `200 OK` with Guid ID
**Permission:** `Permissions.Accounting.Create`

---

**2. Update Debit Memo**
```http
PUT /accounting/debit-memos/{id}
Content-Type: application/json
```
**Request Body:**
```json
{
  "id": "guid",
  "memoDate": "2025-10-15",
  "amount": 550.00,
  "reason": "Updated reason",
  "description": "Updated description",
  "notes": "Updated notes"
}
```
**Response:** `200 OK`
**Permission:** `Permissions.Accounting.Update`
**Notes:** Only draft memos can be updated

---

**3. Get Debit Memo**
```http
GET /accounting/debit-memos/{id}
```
**Response:** `200 OK` with DebitMemoResponse
**Permission:** `Permissions.Accounting.View`

---

**4. Delete Debit Memo**
```http
DELETE /accounting/debit-memos/{id}
```
**Response:** `204 No Content`
**Permission:** `Permissions.Accounting.Delete`
**Notes:** Only draft memos can be deleted

---

**5. Search Debit Memos**
```http
POST /accounting/debit-memos/search
Content-Type: application/json
```
**Request Body:**
```json
{
  "pageNumber": 1,
  "pageSize": 10,
  "memoNumber": "DM-",
  "referenceType": "Customer",
  "referenceId": "guid (optional)",
  "status": "Draft",
  "approvalStatus": "Pending",
  "amountFrom": 100.00,
  "amountTo": 1000.00,
  "memoDateFrom": "2025-01-01",
  "memoDateTo": "2025-12-31",
  "hasUnappliedAmount": true,
  "orderBy": ["memoDate desc"]
}
```
**Response:** `200 OK` with PagedList<DebitMemoResponse>
**Permission:** `Permissions.Accounting.View`

---

#### Specialized Operations

**6. Approve Debit Memo**
```http
POST /accounting/debit-memos/{id}/approve
Content-Type: application/json
```
**Request Body:**
```json
{
  "id": "guid",
  "approvedBy": "John Manager",
  "approvedDate": "2025-10-15",
  "comments": "Approved for processing"
}
```
**Response:** `200 OK`
**Permission:** `Permissions.Accounting.Approve`

---

**7. Apply Debit Memo**
```http
POST /accounting/debit-memos/{id}/apply
Content-Type: application/json
```
**Request Body:**
```json
{
  "id": "guid",
  "documentId": "invoice-guid",
  "amountToApply": 250.00,
  "appliedDate": "2025-10-15"
}
```
**Response:** `200 OK`
**Permission:** `Permissions.Accounting.Update`
**Notes:** Memo must be approved first

---

**8. Void Debit Memo**
```http
POST /accounting/debit-memos/{id}/void
Content-Type: application/json
```
**Request Body:**
```json
{
  "id": "guid",
  "voidReason": "Entered in error"
}
```
**Response:** `200 OK`
**Permission:** `Permissions.Accounting.Delete`
**Notes:** Reverses any applications

---

### Credit Memos Endpoints

#### CRUD Operations

**1. Create Credit Memo**
```http
POST /accounting/credit-memos
Content-Type: application/json
```
**Request Body:**
```json
{
  "memoNumber": "CM-2025-001",
  "memoDate": "2025-10-15",
  "amount": 300.00,
  "referenceType": "Customer",
  "referenceId": "guid",
  "originalDocumentId": "guid (optional)",
  "reason": "Product return - damaged goods",
  "description": "Customer returned damaged items",
  "notes": "Internal notes"
}
```
**Response:** `200 OK` with Guid ID
**Permission:** `Permissions.Accounting.Create`

---

**2. Update Credit Memo**
```http
PUT /accounting/credit-memos/{id}
Content-Type: application/json
```
**Request Body:**
```json
{
  "id": "guid",
  "memoDate": "2025-10-15",
  "amount": 350.00,
  "reason": "Updated reason",
  "description": "Updated description",
  "notes": "Updated notes"
}
```
**Response:** `200 OK`
**Permission:** `Permissions.Accounting.Update`
**Notes:** Only draft memos can be updated

---

**3. Get Credit Memo**
```http
GET /accounting/credit-memos/{id}
```
**Response:** `200 OK` with CreditMemoResponse
**Permission:** `Permissions.Accounting.View`

---

**4. Delete Credit Memo**
```http
DELETE /accounting/credit-memos/{id}
```
**Response:** `204 No Content`
**Permission:** `Permissions.Accounting.Delete`
**Notes:** Only draft memos can be deleted

---

**5. Search Credit Memos**
```http
POST /accounting/credit-memos/search
Content-Type: application/json
```
**Request Body:**
```json
{
  "pageNumber": 1,
  "pageSize": 10,
  "memoNumber": "CM-",
  "referenceType": "Customer",
  "referenceId": "guid (optional)",
  "status": "Draft",
  "approvalStatus": "Pending",
  "amountFrom": 100.00,
  "amountTo": 1000.00,
  "memoDateFrom": "2025-01-01",
  "memoDateTo": "2025-12-31",
  "hasUnappliedAmount": true,
  "orderBy": ["memoDate desc"]
}
```
**Response:** `200 OK` with PagedList<CreditMemoResponse>
**Permission:** `Permissions.Accounting.View`

---

#### Specialized Operations

**6. Approve Credit Memo**
```http
POST /accounting/credit-memos/{id}/approve
Content-Type: application/json
```
**Request Body:**
```json
{
  "id": "guid",
  "approvedBy": "Jane Manager",
  "approvedDate": "2025-10-15",
  "comments": "Approved for processing"
}
```
**Response:** `200 OK`
**Permission:** `Permissions.Accounting.Approve`

---

**7. Apply Credit Memo**
```http
POST /accounting/credit-memos/{id}/apply
Content-Type: application/json
```
**Request Body:**
```json
{
  "id": "guid",
  "documentId": "invoice-guid",
  "amountToApply": 150.00,
  "appliedDate": "2025-10-15"
}
```
**Response:** `200 OK`
**Permission:** `Permissions.Accounting.Update`
**Notes:** Memo must be approved first

---

**8. Refund Credit Memo**
```http
POST /accounting/credit-memos/{id}/refund
Content-Type: application/json
```
**Request Body:**
```json
{
  "id": "guid",
  "refundAmount": 200.00,
  "refundDate": "2025-10-15",
  "refundMethod": "Check",
  "refundReference": "CHK-12345"
}
```
**Response:** `200 OK`
**Permission:** `Permissions.Accounting.Update`
**Notes:** Memo must be approved first

---

**9. Void Credit Memo**
```http
POST /accounting/credit-memos/{id}/void
Content-Type: application/json
```
**Request Body:**
```json
{
  "id": "guid",
  "voidReason": "Duplicate entry"
}
```
**Response:** `200 OK`
**Permission:** `Permissions.Accounting.Delete`
**Notes:** Reverses any applications and refunds

---

## Search Filters

Both Debit and Credit Memos support the following filters:

| Filter | Type | Description |
|--------|------|-------------|
| `memoNumber` | string | Partial match on memo number |
| `referenceType` | string | Customer or Vendor |
| `referenceId` | Guid | Specific customer or vendor ID |
| `status` | string | Draft, Approved, Applied, [Refunded], Voided |
| `approvalStatus` | string | Pending, Approved, Rejected |
| `amountFrom` | decimal | Minimum amount |
| `amountTo` | decimal | Maximum amount |
| `memoDateFrom` | DateTime | Start date |
| `memoDateTo` | DateTime | End date |
| `hasUnappliedAmount` | bool | Only memos with unapplied balance |

---

## Permissions

All endpoints require authentication and specific permissions:

| Permission | Description | Endpoints |
|------------|-------------|-----------|
| `Permissions.Accounting.View` | View memos | GET, Search |
| `Permissions.Accounting.Create` | Create memos | POST (create) |
| `Permissions.Accounting.Update` | Update/Apply/Refund memos | PUT, Apply, Refund |
| `Permissions.Accounting.Approve` | Approve memos | Approve |
| `Permissions.Accounting.Delete` | Delete/Void memos | DELETE, Void |

---

## Response Models

### DebitMemoResponse
```json
{
  "id": "guid",
  "memoNumber": "DM-2025-001",
  "memoDate": "2025-10-15",
  "amount": 500.00,
  "appliedAmount": 250.00,
  "unappliedAmount": 250.00,
  "referenceType": "Customer",
  "referenceId": "guid",
  "originalDocumentId": "guid",
  "reason": "Additional charges",
  "status": "Applied",
  "isApplied": true,
  "appliedDate": "2025-10-16",
  "approvalStatus": "Approved",
  "approvedBy": "John Manager",
  "approvedDate": "2025-10-15",
  "description": "Description text",
  "notes": "Internal notes",
  "createdOn": "2025-10-15T10:00:00Z",
  "createdBy": "user-guid",
  "lastModifiedOn": "2025-10-16T14:30:00Z",
  "lastModifiedBy": "user-guid"
}
```

### CreditMemoResponse
```json
{
  "id": "guid",
  "memoNumber": "CM-2025-001",
  "memoDate": "2025-10-15",
  "amount": 300.00,
  "appliedAmount": 100.00,
  "refundedAmount": 150.00,
  "unappliedAmount": 50.00,
  "referenceType": "Customer",
  "referenceId": "guid",
  "originalDocumentId": "guid",
  "reason": "Product return",
  "status": "Refunded",
  "isApplied": true,
  "appliedDate": "2025-10-16",
  "approvalStatus": "Approved",
  "approvedBy": "Jane Manager",
  "approvedDate": "2025-10-15",
  "description": "Description text",
  "notes": "Internal notes",
  "createdOn": "2025-10-15T10:00:00Z",
  "createdBy": "user-guid",
  "lastModifiedOn": "2025-10-16T14:30:00Z",
  "lastModifiedBy": "user-guid"
}
```

---

## Module Registration

Both endpoint groups are registered in `AccountingModule.cs`:

```csharp
accountingGroup.MapCreditMemosEndpoints();
accountingGroup.MapDebitMemosEndpoints();
```

Routes are automatically versioned and tagged:
- **Tags:** `Debit-Memos`, `Credit-Memos`
- **Base Path:** `/accounting/debit-memos`, `/accounting/credit-memos`
- **Version:** API v1

---

## Error Handling

All endpoints include proper error handling:

### Common HTTP Status Codes
- `200 OK` - Successful operation
- `204 No Content` - Successful deletion
- `400 Bad Request` - Invalid request or ID mismatch
- `401 Unauthorized` - Missing or invalid authentication
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Memo not found
- `422 Unprocessable Entity` - Business rule violation

### Business Rule Exceptions
- `DebitMemoNotFoundException` - Memo ID not found
- `DebitMemoCannotBeModifiedException` - Cannot modify approved/applied memo
- `DebitMemoAlreadyApprovedException` - Already approved
- `DebitMemoAlreadyVoidedException` - Already voided
- `DebitMemoNotApprovedException` - Not approved for application
- `DebitMemoInsufficientBalanceException` - Insufficient unapplied balance
- (Same exceptions for CreditMemo)

---

## Testing Guide

### Test Scenarios

**Debit Memos:**
1. ✓ Create draft debit memo
2. ✓ Search with multiple filters
3. ✓ Update draft memo
4. ✓ Delete draft memo
5. ✓ Approve draft memo
6. ✓ Apply approved memo (full and partial)
7. ✓ Void memo (reverses applications)
8. ✗ Try to update approved memo (should fail)
9. ✗ Try to apply unapproved memo (should fail)
10. ✗ Try to apply amount exceeding balance (should fail)

**Credit Memos:**
1. ✓ Create draft credit memo
2. ✓ Search with multiple filters
3. ✓ Update draft memo
4. ✓ Delete draft memo
5. ✓ Approve draft memo
6. ✓ Apply approved memo (full and partial)
7. ✓ Refund approved memo (full and partial)
8. ✓ Apply + refund split scenario
9. ✓ Void memo (reverses all transactions)
10. ✗ Try to update approved memo (should fail)
11. ✗ Try to refund unapproved memo (should fail)
12. ✗ Try to refund amount exceeding balance (should fail)

### Sample cURL Commands

**Create Debit Memo:**
```bash
curl -X POST "https://localhost:7000/accounting/debit-memos" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "memoNumber": "DM-TEST-001",
    "memoDate": "2025-10-15",
    "amount": 500.00,
    "referenceType": "Customer",
    "referenceId": "guid",
    "reason": "Test debit memo"
  }'
```

**Search Debit Memos:**
```bash
curl -X POST "https://localhost:7000/accounting/debit-memos/search" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "pageNumber": 1,
    "pageSize": 10,
    "status": "Draft"
  }'
```

---

## Summary

✅ **30 Files Created**
✅ **18 API Endpoints** (9 per memo type)
✅ **Complete CRUD Operations**
✅ **Specialized Workflow Actions**
✅ **Advanced Search with 9 Filters**
✅ **Proper Permissions & Authorization**
✅ **Comprehensive Error Handling**
✅ **Full Documentation**

**Status:** API Implementation Complete ✅
**Next Steps:** Test all endpoints, integrate with frontend pages
