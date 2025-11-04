## Bank Reconciliation - Quick Reference Guide

### API Endpoints

```
POST   /accounting/bank-reconciliations           - Create new reconciliation
PUT    /accounting/bank-reconciliations/{id}      - Update reconciliation items
GET    /accounting/bank-reconciliations/{id}      - Get reconciliation details
DELETE /accounting/bank-reconciliations/{id}      - Delete reconciliation
POST   /accounting/bank-reconciliations/search    - Search reconciliations
POST   /accounting/bank-reconciliations/{id}/start      - Start reconciliation
POST   /accounting/bank-reconciliations/{id}/complete   - Complete reconciliation
POST   /accounting/bank-reconciliations/{id}/approve    - Approve reconciliation
POST   /accounting/bank-reconciliations/{id}/reject     - Reject reconciliation
```

### Status Flow

```
Pending --> InProgress --> Completed --> Approved
              |                |
              +------ Reject ---+
```

### Create Reconciliation

**Minimal Request:**
```json
{
  "bankAccountId": "guid",
  "reconciliationDate": "2025-10-31",
  "statementBalance": 50000.00,
  "bookBalance": 49980.00
}
```

**Full Request:**
```json
{
  "bankAccountId": "guid",
  "reconciliationDate": "2025-10-31",
  "statementBalance": 50000.00,
  "bookBalance": 49980.00,
  "statementNumber": "STM-202510-001",
  "description": "October 2025 bank reconciliation",
  "notes": "Initial reconciliation setup"
}
```

### Update Reconciliation Items

**Request:**
```json
{
  "id": "guid",
  "outstandingChecksTotal": 500.00,
  "depositsInTransitTotal": 200.00,
  "bankErrors": 0.00,
  "bookErrors": 20.00
}
```

**Calculation:**
- AdjustedBalance = BookBalance + BookErrors = 49980.00 + 20.00 = 50000.00
- Should match: StatementBalance (50000.00) ✓

### Complete Reconciliation

**Request:**
```json
{
  "id": "guid",
  "reconciledBy": "john.smith@company.com"
}
```

**Validation:**
- Status must be InProgress
- AdjustedBalance must equal StatementBalance (±0.01 tolerance)
- ReconciledBy is required

### Approve Reconciliation

**Request:**
```json
{
  "id": "guid",
  "approvedBy": "manager@company.com"
}
```

**Result:**
- Status changes to Approved
- IsReconciled flag set to true
- ApprovedDate recorded

### Reject Reconciliation

**Request:**
```json
{
  "id": "guid",
  "rejectedBy": "manager@company.com",
  "reason": "Discrepancy found in check #1234. Please verify and resubmit."
}
```

**Result:**
- Status changes back to Pending
- Reason appended to Notes field

### Search Reconciliations

**Request:**
```json
{
  "bankAccountId": "guid",
  "fromDate": "2025-10-01",
  "toDate": "2025-10-31",
  "status": "Completed",
  "isReconciled": false,
  "pageNumber": 1,
  "pageSize": 10
}
```

**Valid Status Values:**
- `Pending`
- `InProgress`
- `Completed`
- `Approved`

### Response Example

```json
{
  "id": "guid",
  "bankAccountId": "guid",
  "reconciliationDate": "2025-10-31",
  "statementBalance": 50000.00,
  "bookBalance": 49980.00,
  "adjustedBalance": 50000.00,
  "outstandingChecksTotal": 500.00,
  "depositsInTransitTotal": 200.00,
  "bankErrors": 0.00,
  "bookErrors": 20.00,
  "status": "Completed",
  "isReconciled": false,
  "reconciledDate": "2025-11-01T14:30:00Z",
  "reconciledBy": "john.smith@company.com",
  "approvedBy": null,
  "approvedDate": null,
  "statementNumber": "STM-202510-001",
  "description": "October 2025 bank reconciliation",
  "notes": "Initial reconciliation setup",
  "createdOn": "2025-10-31T09:00:00Z",
  "createdBy": "guid",
  "lastModifiedOn": "2025-11-01T14:30:00Z",
  "lastModifiedBy": "guid"
}
```

### Error Responses

**400 - Bad Request (Validation Error)**
```json
{
  "errors": {
    "StatementBalance": ["Statement balance cannot be negative."]
  }
}
```

**404 - Not Found**
```json
{
  "detail": "Bank reconciliation with id {id} not found"
}
```

**409 - Conflict (Business Rule Violation)**
```json
{
  "detail": "Bank reconciliation {id} has already been reconciled"
}
```

### Permissions Required

- **Create:** `Permissions.Accounting.Create`
- **Update:** `Permissions.Accounting.Update`
- **Get:** `Permissions.Accounting.View`
- **Delete:** `Permissions.Accounting.Delete`
- **Start/Complete/Reject:** `Permissions.Accounting.Update`
- **Approve:** `Permissions.Accounting.Approve`
- **Search:** `Permissions.Accounting.View`

### Validation Rules by Field

| Field | Required | Type | Rules |
|-------|----------|------|-------|
| BankAccountId | Yes | GUID | Must be non-empty valid ID |
| ReconciliationDate | Yes | DateTime | Cannot be in future; <= today |
| StatementBalance | Yes | Decimal | >= 0, <= 999,999,999.99 |
| BookBalance | Yes | Decimal | >= 0, <= 999,999,999.99 |
| OutstandingChecks | No | Decimal | >= 0, <= 999,999,999.99 |
| DepositsInTransit | No | Decimal | >= 0, <= 999,999,999.99 |
| BankErrors | No | Decimal | -999,999,999.99 to 999,999,999.99 |
| BookErrors | No | Decimal | -999,999,999.99 to 999,999,999.99 |
| Status | N/A | String | Pending, InProgress, Completed, Approved |
| StatementNumber | No | String | Max 100 chars; alphanumeric/hyphens/dots/slashes |
| Description | No | String | Max 2048 chars |
| Notes | No | String | Max 2048 chars |
| ReconciledBy | In Complete | String | Max 256 chars; required for completion |
| ApprovedBy | In Approve | String | Max 256 chars; required for approval |
| RejectedBy | In Reject | String | Max 256 chars; required for rejection |
| Reason | In Reject | String | Optional; max 2048 chars; min 5 if provided |

### Common Workflows

#### Simple Reconciliation
1. Create with opening balances
2. Start reconciliation
3. Update with outstanding items
4. Complete reconciliation (validates balance)
5. Approve reconciliation

#### Rejected Reconciliation
1. Create with opening balances
2. Start reconciliation
3. Update with items
4. Complete reconciliation
5. Approve reconciliation
6. ⚠️ Reject with reason
7. Go back to step 2 (Start)

#### Delete Unfinished
1. Create reconciliation
2. Delete immediately (or anytime before completion)

### Troubleshooting

**Q: Balance mismatch when completing**
A: Verify calculation:
- Expected = StatementBalance + OutstandingChecks - DepositsInTransit + BankErrors
- AdjustedBalance = BookBalance + BookErrors
- These must match within 0.01

**Q: Cannot approve unfineted reconciliation**
A: Reconciliation must be in Completed status first. Call the Complete endpoint.

**Q: Cannot update completed reconciliation**
A: Once marked as Completed, call Reject first to return to Pending, then Update.

**Q: Permission denied errors**
A: Verify user has appropriate accounting permissions:
- View: `Permissions.Accounting.View`
- Create: `Permissions.Accounting.Create`
- Update: `Permissions.Accounting.Update`
- Delete: `Permissions.Accounting.Delete`
- Approve: `Permissions.Accounting.Approve`

### Status Code Reference

| Code | Meaning | Common Cause |
|------|---------|--------------|
| 201 | Created | Reconciliation created successfully |
| 204 | No Content | Update/Complete/Approve/Reject successful |
| 200 | OK | GET or Search successful |
| 400 | Bad Request | Validation error in request |
| 401 | Unauthorized | Not authenticated |
| 403 | Forbidden | User lacks required permission |
| 404 | Not Found | Reconciliation not found |
| 409 | Conflict | Business rule violation (status conflict, already reconciled, etc.) |
| 500 | Server Error | Unexpected server error |

---

For complete documentation, see: `BANK_RECONCILIATION_IMPLEMENTATION.md`

