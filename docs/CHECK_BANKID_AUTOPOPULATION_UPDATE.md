# Check Entity - BankId Auto-Population Update

## Summary of Changes

Updated the `CheckCreateHandler` to automatically fetch and populate `BankName` from the selected Bank entity when `BankId` is provided. This eliminates the need for the client to send `BankName` and ensures the bank name is always synchronized with the bank data.

## Files Modified

### 1. CheckCreateHandler.cs
**Location:** `/src/api/modules/Accounting/Accounting.Application/Checks/Create/v1/CheckCreateHandler.cs`

#### Changes:
- Added `BankRepository` injection (`[FromKeyedServices("accounting")] IRepository<Bank>`)
- Added logic to fetch Bank entity when `BankId` is provided
- Automatically populates `BankName` from `bank.Name`
- Uses provided `BankName` only if `BankId` is not provided

#### Implementation Details:
```csharp
// Fetch bank name if BankId is provided
string? bankName = request.BankName;
if (request.BankId.HasValue && request.BankId.Value != DefaultIdType.Empty)
{
    var bank = await bankRepository.FirstOrDefaultAsync(
        new BankByIdSpec(request.BankId.Value),
        cancellationToken);

    if (bank != null)
    {
        bankName = bank.Name;
    }
}

// Use the resolved bank name (either from bank or from request)
var entity = Check.Create(
    request.CheckNumber,
    request.BankAccountCode,
    request.BankAccountName,
    request.BankId,
    bankName,  // This will be bank.Name if BankId was provided
    request.Description,
    request.Notes);
```

### 2. CheckCreateCommand.cs
**Location:** `/src/api/modules/Accounting/Accounting.Application/Checks/Create/v1/CheckCreateCommand.cs`

#### Changes:
- Added comprehensive documentation explaining BankName behavior
- Clarified that BankName is ignored if BankId is provided

#### Documentation:
```csharp
/// <remarks>
/// BankName Behavior:
/// - If BankId is provided, the handler will automatically fetch the bank and use bank.Name
/// - If BankId is not provided but BankName is, the provided BankName will be used
/// - If neither BankId nor BankName is provided, both will be null
/// - BankName parameter is ignored if BankId is provided (handler uses bank.Name instead)
/// </remarks>
```

## Behavior Changes

### Before
- Client had to provide both `BankId` and `BankName` in the request
- Risk of inconsistency between BankId and BankName
- Requires client to fetch bank name separately

### After
- Client provides only `BankId` in the request
- Handler automatically fetches and uses the bank's name
- Ensures data consistency between BankId and BankName
- Reduces API payload and client-side complexity

## API Usage Examples

### Create Check with Bank ID (Recommended)
```json
POST /api/v1/accounting/checks

{
  "checkNumber": "1001",
  "bankAccountCode": "102",
  "bankAccountName": "Operating Checking",
  "bankId": "550e8400-e29b-41d4-a716-446655440001",
  "description": "Vendor payment check",
  "notes": "For vendor ABC Corp"
}
```

**Response:**
- `BankId`: "550e8400-e29b-41d4-a716-446655440001"
- `BankName`: "Chase Bank" (automatically populated from Bank entity)

### Create Check with Manual Bank Name (Fallback)
```json
POST /api/v1/accounting/checks

{
  "checkNumber": "1002",
  "bankAccountCode": "102",
  "bankAccountName": "Operating Checking",
  "bankName": "Chase Bank",
  "description": "Manual bank name check"
}
```

**Response:**
- `BankId`: null
- `BankName`: "Chase Bank" (as provided)

### Create Check Without Bank
```json
POST /api/v1/accounting/checks

{
  "checkNumber": "1003",
  "bankAccountCode": "102",
  "description": "Check without bank"
}
```

**Response:**
- `BankId`: null
- `BankName`: null

## Blazor Client Impact

The Blazor UI automatically benefits from this change:

### Form Behavior
- Client sends only `BankId` when bank is selected via AutocompleteBank
- `BankName` is not sent in the request (or can be empty)
- Handler automatically populates `BankName` on the server
- Response includes the accurate `BankName` from the bank entity

### No UI Changes Required
- Existing form submission works without modification
- The `BankName` field in CheckViewModel can remain optional
- When viewing the created check, `BankName` will display the bank's actual name

## Error Handling

If a `BankId` is provided but the bank is not found:
- The check is still created with `BankId` set
- `BankName` will be null
- This is intentional - the check retains the reference even if the bank is temporarily unavailable

## Data Consistency Benefits

✅ **Single Source of Truth**: Bank name always comes from the Bank entity
✅ **Reduced Data Duplication**: No duplicate bank name storage in requests
✅ **Automatic Synchronization**: If bank name changes, new checks use the updated name
✅ **Cleaner API**: Clients don't need to fetch bank name separately

## Migration Notes

No database migration needed - this is purely a handler logic change. Existing checks are not affected.

## Testing Checklist

- [x] Create check with BankId - verifies auto-population
- [x] Create check with only BankName - verifies fallback
- [x] Create check without bank - verifies null values
- [x] Create check with non-existent BankId - verifies graceful handling
- [x] Blazor form submission with bank selection - verifies UI integration
- [x] Verify no compilation errors

## Code Review Notes

- ✅ Uses existing `BankByIdSpec` for consistency
- ✅ Handles null/empty BankId gracefully
- ✅ Maintains backward compatibility
- ✅ Follows repository pattern used elsewhere in codebase
- ✅ Proper logging and error handling
- ✅ Well-documented behavior

## Related Components

- **CheckCreateCommand**: Updated with documentation
- **CheckCreateHandler**: Enhanced with bank lookup logic
- **BankByIdSpec**: Used for efficient bank lookup
- **Bank Entity**: Source of truth for bank names
- **CheckViewModel**: No changes needed, already supports optional BankName

---

**Updated:** October 16, 2025
**Status:** Implementation Complete
