# Check Bundle Creation Implementation

## Overview

The Check Management system has been updated to support **Check Bundle Registration**, which is the standard way checks are managed in real-world accounting systems. Instead of creating one check at a time, checks come in pre-printed pads/books with sequential numbers, and all checks in a pad are registered at once.

## Concept: Why Bundles?

In accounting systems, checks are typically:
- **Ordered from printers** in pre-printed pads/books
- **Numbered sequentially** (e.g., 3453000-3453500 = 501 checks)
- **Registered as a group** in the system when received
- **Tracked by status** (Available → Issued → Cleared, etc.)

### Real-World Example
**Scenario:** Company receives a new checkbook from the bank
- **Checkbook contains:** Checks numbered 3453000 to 3453500 (501 checks)
- **Action:** Register all 501 checks at once in the system
- **Benefit:** Proper inventory management and easier to track used/unused checks

---

## Implementation Changes

### 1. **CheckCreateCommand** - Updated for Bundle Registration

**Before:**
```csharp
public record CheckCreateCommand(
    string CheckNumber,
    string BankAccountCode,
    DefaultIdType? BankId,
    string? Description,
    string? Notes
) : IRequest<CheckCreateResponse>;
```

**After:**
```csharp
public record CheckCreateCommand(
    string StartCheckNumber,      // First check in the range (e.g., "3453000")
    string EndCheckNumber,        // Last check in the range (e.g., "3453500")
    string BankAccountCode,       // Bank account code
    DefaultIdType? BankId,        // Bank ID (optional)
    string? Description,          // Description (applied to all checks)
    string? Notes                 // Notes (applied to all checks)
) : IRequest<CheckCreateResponse>;
```

**Usage Example:**
```csharp
var command = new CheckCreateCommand(
    StartCheckNumber: "3453000",
    EndCheckNumber: "3453500",
    BankAccountCode: "102",
    BankId: bankId,
    Description: "Check pad received from Chase Bank",
    Notes: "Blue ink checks"
);
```

### 2. **CheckCreateResponse** - Includes Bundle Information

**Before:**
```csharp
public record CheckCreateResponse(DefaultIdType Id);
```

**After:**
```csharp
public record CheckCreateResponse(
    DefaultIdType Id,                  // First check ID (reference)
    string StartCheckNumber,           // "3453000"
    string EndCheckNumber,             // "3453500"
    int ChecksCreated                  // 501 (actual count created)
);
```

**Response Example:**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "startCheckNumber": "3453000",
  "endCheckNumber": "3453500",
  "checksCreated": 501
}
```

### 3. **CheckCreateHandler** - Bulk Creation Logic

**Key Features:**
- ✅ Parses numeric check numbers
- ✅ Validates range (max 10,000 checks per bundle)
- ✅ Validates end >= start
- ✅ Fetches BankAccountName and BankName once (efficiency)
- ✅ Creates all checks in a loop
- ✅ Handles duplicates gracefully (skips and logs)
- ✅ Uses `AddRangeAsync` for bulk insert
- ✅ Single database transaction for all checks

**Logic Flow:**
```
1. Parse StartCheckNumber and EndCheckNumber to integers
2. Calculate range size (endNum - startNum + 1)
3. Validate range (1-10,000 checks)
4. Fetch BankAccountName from ChartOfAccount (once)
5. Fetch BankName from Bank (once)
6. FOR each check number in range:
   - Check if already exists (if yes, skip and log)
   - Create Check entity
   - Add to list
7. Bulk insert all valid checks
8. Return response with bundle info and count
```

### 4. **CheckCreateCommandValidator** - Range Validation

**Validation Rules:**
- ✅ StartCheckNumber: Required, max 64 chars, alphanumeric + hyphens/underscores
- ✅ EndCheckNumber: Required, max 64 chars, alphanumeric + hyphens/underscores
- ✅ Both must be numeric for range comparison
- ✅ EndCheckNumber >= StartCheckNumber
- ✅ Range <= 10,000 checks per bundle
- ✅ Range >= 1 check minimum

**Validation Examples:**
```
✅ Valid:   "3453000" to "3453500"  (501 checks)
✅ Valid:   "1000" to "1001"        (2 checks)
❌ Invalid: "3453500" to "3453000"  (end < start)
❌ Invalid: "3453000" to "3463000"  (11,001 checks - exceeds limit)
❌ Invalid: "ABC" to "DEF"          (non-numeric)
```

### 5. **Blazor UI - CheckViewModel Updates**

**New Properties:**
```csharp
public string? StartCheckNumber { get; set; }  // For create mode
public string? EndCheckNumber { get; set; }    // For create mode
public string CheckNumber { get; set; }        // For display/edit mode
```

**UI Behavior:**
- **Create Mode:** Shows StartCheckNumber and EndCheckNumber inputs
- **Edit Mode:** Shows read-only CheckNumber (displays individual check)

### 6. **Blazor Page - Updated Form**

**Create Form (New):**
```html
<MudItem xs="12" sm="6" md="4">
    <MudTextField @bind-Value="context.StartCheckNumber"
                  Label="Start Check Number"
                  Placeholder="e.g., 3453000"
                  Required="true"
                  HelperText="First check number in the bundle" />
</MudItem>

<MudItem xs="12" sm="6" md="4">
    <MudTextField @bind-Value="context.EndCheckNumber"
                  Label="End Check Number"
                  Placeholder="e.g., 3453500"
                  Required="true"
                  HelperText="Last check number in the bundle" />
</MudItem>
```

**Edit Form (Unchanged):**
- Shows individual CheckNumber (read-only)
- Shows all other fields (BankAccountCode, BankId, Description, Notes)

---

## API Usage

### Endpoint
```
POST /api/v1/checks
```

### Request Body
```json
{
  "startCheckNumber": "3453000",
  "endCheckNumber": "3453500",
  "bankAccountCode": "102",
  "bankId": "550e8400-e29b-41d4-a716-446655440000",
  "description": "Check pad from Chase Bank",
  "notes": "Blue ink, sequential numbering"
}
```

### Success Response (201 Created)
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440001",
  "startCheckNumber": "3453000",
  "endCheckNumber": "3453500",
  "checksCreated": 501
}
```

### Error Responses

**400 Bad Request - Invalid Range:**
```json
{
  "errors": {
    "EndCheckNumber": ["End check number must be greater than or equal to start check number."]
  }
}
```

**400 Bad Request - Range Too Large:**
```json
{
  "errors": {
    "CheckRange": ["Check range cannot exceed 10,000 checks per bundle."]
  }
}
```

**409 Conflict - Duplicate Range:**
```json
{
  "detail": "Check number range '3453000-3453500' already exists for bank account '102'."
}
```

---

## Database Behavior

### Single Transaction
All checks in the bundle are created within a single database transaction:
- **All succeed together** or **all fail together**
- **Atomic operation** - no partial bundles
- **Efficient bulk insert** using `AddRangeAsync`

### Duplicate Handling
If some checks already exist:
- **Skipped checks** are logged but don't fail the bundle
- Example: Range 3453000-3453500 where 3453200-3453300 exist
  - Result: 401 checks created, 100 skipped
  - Response shows `"checksCreated": 401`

### Check Number Format
- Preserves leading zeros using original length
- Example: "001000" range maintains 6-digit format
- Enables proper sorting and sequential tracking

---

## Workflow: Registering a New Check Pad

**Step 1: Receive Checkbook from Printer/Bank**
- Physical checkbook contains checks numbered 3453000-3453500 (501 checks)

**Step 2: Open Check Management**
- Navigate to Accounting → Check Management

**Step 3: Click "Create New" Button**
- Form shows: Start Check Number, End Check Number fields (instead of single number)

**Step 4: Fill Bundle Information**
```
Start Check Number: 3453000
End Check Number:   3453500
Bank Account:       102 (Operating Checking)
Bank:               Chase Bank
Description:        Check pad from Chase Bank order #CH-2024-1234
Notes:              Blue ink, laser printed
```

**Step 5: Click "Create Bundle"**
- System creates 501 Check entities
- All in "Available" status
- Ready to be issued for payments

**Step 6: Track Check Usage**
- As checks are issued, status changes to "Issued"
- When cleared by bank: "Cleared"
- If voided: "Void"
- If cancelled: "StopPayment"

---

## Technical Benefits

| Benefit | Description |
|---------|-------------|
| **Efficiency** | Create 500 checks with 1 API call instead of 500 calls |
| **Atomicity** | All or nothing - no partial bundles |
| **Auditability** | Know which checks came from which pad |
| **Inventory** | Track unused/available checks by pad |
| **Error Handling** | Graceful skip of duplicates with logging |
| **Performance** | Bulk insert is much faster than individual inserts |
| **Data Integrity** | Single transaction prevents inconsistencies |

---

## Migration Notes

### Breaking Changes
- `CheckCreateCommand.CheckNumber` → `CheckCreateCommand.StartCheckNumber` and `EndCheckNumber`
- API clients must be updated to send ranges instead of single numbers
- Blazor client DTOs must be regenerated

### Backward Compatibility
- ✅ Update endpoints unchanged
- ✅ Get endpoints unchanged
- ✅ Search unchanged
- ✅ Issue/Void/Clear/StopPayment/Print unchanged
- ✅ Only Create operation affected

### Database Changes
- No schema changes required
- Existing checks still work
- New checks created with bundle follow same structure

---

## Best Practices

1. **Match Physical Pads:** Always register check ranges that match physical checkbook pads
2. **Document Pad Info:** Include pad order number or date in Description
3. **Handle Duplicates:** If some checks already exist, API will skip them - verify count matches expectations
4. **Validate Range:** Ensure EndCheckNumber > StartCheckNumber before submission
5. **Bulk Operations:** Use this for new pad registration (not for individual check entry)

---

## Example Scenarios

### Scenario 1: Standard Check Pad
```
Received: 500-check pad
Start: 10001
End: 10500
Description: "Standard check pad from Chase"
Result: 500 checks created
```

### Scenario 2: Smaller Pad (Travel Checks)
```
Received: 25-check pad for business travel
Start: 20001
End: 20025
Description: "Travel reimbursement checks"
Result: 25 checks created
```

### Scenario 3: Large Pad (Annual Order)
```
Received: 1000-check pad annual order
Start: 30001
End: 31000
Description: "Annual check order from Chase, Invoice #CH-2024-5678"
Result: 1000 checks created
```

### Scenario 4: Duplicate Skip
```
Previous registration: 10001-10500 already exists
New attempt: 10401-10900 (overlap)
System behavior:
  - Skips 10401-10500 (already exist)
  - Creates 10501-10900 (new)
  - Result: checksCreated = 400, Warning: 100 duplicates skipped
```

---

## Summary

The Bundle Registration implementation allows efficient, batch registration of check pads matching real-world accounting practices. It provides:
- ✅ Single API call for 500-1000 checks
- ✅ Range validation and duplicate handling
- ✅ Efficient bulk database operations
- ✅ Proper inventory management
- ✅ Full audit trail support
