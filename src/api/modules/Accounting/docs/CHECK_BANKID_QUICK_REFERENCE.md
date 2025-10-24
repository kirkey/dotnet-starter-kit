# Check Entity BankId/BankName - Quick Reference

## Files Modified

| File | Changes |
|------|---------|
| `Check.cs` | Added BankId and BankName properties, updated Create() and Update() methods |
| `CheckConfiguration.cs` | Added BankName config and BankId index |
| `CheckCreateCommand.cs` | Added BankId and BankName fields |
| `CheckCreateHandler.cs` | Passed new fields to Check.Create() |
| `CheckCreateCommandValidator.cs` | Added validation for BankName |
| `CheckSearchResponse.cs` | Added BankId and BankName fields |
| `CheckGetResponse.cs` | Added BankId and BankName fields |
| `CheckViewModel.cs` | Added BankId and BankName properties |
| `Checks.razor.cs` | Added BankName to table fields |
| `Checks.razor` | Added AutocompleteBank component and BankName display |

## How to Use BankId in the Checks Page

### Create a New Check
1. Open the Checks page (`/accounting/checks`)
2. Click "Create" to open the form
3. Fill in required fields:
   - Check Number
   - Bank Account (from chart of accounts)
4. **New:** Select a Bank from the autocomplete dropdown
   - Type to search by bank name or code
   - Select from the list
5. Fill optional fields (Account Name, Description, Notes)
6. Click "Save"

### Edit Existing Check
- Only available checks can be edited
- The BankId field updates along with other check details
- BankName displays as read-only in the detail view

### View Bank Information
- The table displays the Bank Name for each check
- Click on a check to see full details including BankId and BankName

## AutocompleteBank Component Configuration

```razor
<AutocompleteBank @bind-Value="context.BankId"
                  For="@(() => context.BankId)"
                  Label="Bank"
                  TextFormat="NameCode"
                  Variant="Variant.Filled" />
```

### TextFormat Options
- `"Name"` - Shows only bank name (default)
- `"Code"` - Shows only bank code
- `"CodeName"` - Shows "CODE - Name"
- `"NameCode"` - Shows "Name (CODE)" ← **Used in Checks page**

## API Examples

### Create Check with Bank
```json
POST /api/v1/accounting/checks

{
  "checkNumber": "1001",
  "bankAccountCode": "102",
  "bankAccountName": "Operating Checking",
  "bankId": "550e8400-e29b-41d4-a716-446655440001",
  "bankName": "Chase Bank",
  "description": "Vendor payment check",
  "notes": "For vendor ABC Corp"
}
```

### Response includes BankId and BankName
```json
GET /api/v1/accounting/checks/search

{
  "id": "550e8400-e29b-41d4-a716-446655440002",
  "checkNumber": "1001",
  "bankAccountCode": "102",
  "bankAccountName": "Operating Checking",
  "bankId": "550e8400-e29b-41d4-a716-446655440001",
  "bankName": "Chase Bank",
  "status": "Available",
  ...
}
```

## Database Schema Changes

### New Columns on Checks Table
- `BankId` (uuid, nullable) - Foreign key to Bank entity
- `BankName` (varchar(256), nullable) - Denormalized bank name

### New Index
- `IX_Checks_BankId` - For performance on BankId filtering

## Business Rules

✅ **BankId is Optional**
- Checks can be created without selecting a bank
- Maintains backward compatibility

✅ **Update Constraints**
- BankId and BankName can only be updated for "Available" checks
- Once a check is issued, the bank cannot be changed

✅ **Display Logic**
- BankName shows in table view for quick identification
- AutocompleteBank provides autocomplete search
- Supports searching by bank name or code

## Migration Command

```bash
# Create migration
dotnet ef migrations add AddCheckBankIdAndBankName \
  --project src/api/modules/Accounting/Accounting.Infrastructure \
  --startup-project src/api

# Apply migration
dotnet ef database update \
  --project src/api/modules/Accounting/Accounting.Infrastructure \
  --startup-project src/api
```

## Next Steps

1. ✅ Code changes completed
2. ⏳ Create database migration
3. ⏳ Test in development environment
4. ⏳ Update API documentation if needed
5. ⏳ Deploy migrations to higher environments

---

**Last Updated:** October 16, 2025
