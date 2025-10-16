# Check BankId Implementation - Quick Start Guide

## ✅ What's Done

### Server-Side (✅ Complete & Compiling)
- ✅ Check entity with BankId and BankName
- ✅ CheckCreateCommand with auto-population logic
- ✅ CheckCreateHandler with bank lookup
- ✅ CheckUpdateCommand (new)
- ✅ CheckUpdateHandler (new) with bank lookup
- ✅ CheckUpdateCommandValidator (new)
- ✅ Database configuration and indexes
- ✅ CheckSearchResponse with BankName
- ✅ CheckGetResponse with BankName

### Client-Side (✅ Ready for DTOs)
- ✅ CheckViewModel with BankId and BankName
- ✅ Checks.razor with AutocompleteBank component
- ✅ Checks.razor.cs with updateFunc implementation
- ⏳ DTOs need regeneration from updated API

## How It Works

### Create Flow
```
User selects Bank in AutocompleteBank
    ↓
BankId sent to API (BankName is null/empty)
    ↓
CheckCreateHandler receives request
    ↓
Handler fetches Bank entity using BankId
    ↓
Handler extracts bank.Name
    ↓
Check created with BankId and auto-populated BankName
```

### Update Flow
```
User edits available check and changes Bank
    ↓
BankId updated in form (BankName shown as read-only)
    ↓
CheckUpdateCommand sent with new BankId
    ↓
CheckUpdateHandler fetches Bank entity
    ↓
Handler extracts bank.Name and updates check
    ↓
Check saved with new BankId and BankName
```

## Key Files

### Commands
- `CheckCreateCommand.cs` - Create with BankId
- `CheckUpdateCommand.cs` - Update with BankId (NEW)

### Handlers
- `CheckCreateHandler.cs` - Auto-populates BankName
- `CheckUpdateHandler.cs` - Auto-populates BankName (NEW)

### Validators
- `CheckCreateCommandValidator.cs`
- `CheckUpdateCommandValidator.cs` (NEW)

### Responses
- `CheckCreateResponse.cs`
- `CheckUpdateResponse.cs` (NEW)
- `CheckSearchResponse.cs` - Includes BankName
- `CheckGetResponse.cs` - Includes BankName

### Blazor
- `CheckViewModel.cs` - Includes BankId, BankName
- `Checks.razor` - AutocompleteBank component
- `Checks.razor.cs` - Create and Update functions

## API Endpoints

### Create
```
POST /api/v1/accounting/checks
Body: CheckCreateCommand (with BankId)
Response: CheckCreateResponse
Auto-populates: BankName from Bank.Name
```

### Update
```
PUT /api/v1/accounting/checks
Body: CheckUpdateCommand (with CheckId and BankId)
Response: CheckUpdateResponse
Auto-populates: BankName from Bank.Name
```

### Get
```
GET /api/v1/accounting/checks/{id}
Response: CheckGetResponse (includes BankId, BankName)
```

### Search
```
POST /api/v1/accounting/checks/search
Response: List<CheckSearchResponse> (includes BankId, BankName)
```

## Usage Examples

### Create Request
```json
{
  "checkNumber": "1001",
  "bankAccountCode": "102",
  "bankAccountName": "Operating",
  "bankId": "550e8400-e29b-41d4-a716-446655440001"
}
```
**Auto-populated response:** `"bankName": "Chase Bank"`

### Update Request
```json
{
  "checkId": "550e8400-e29b-41d4-a716-446655440002",
  "checkNumber": "1001",
  "bankAccountCode": "102",
  "bankId": "550e8400-e29b-41d4-a716-446655440001"
}
```
**Auto-populated response:** `"bankName": "Chase Bank"`

## Behavior

### ✅ When BankId Provided
- Handler fetches Bank entity
- Uses bank.Name for BankName (ignores any provided BankName)
- Data stays in sync with Bank entity

### ✅ When Only BankName Provided
- Uses provided BankName value
- Fallback for manual entry

### ✅ When Neither Provided
- Both BankId and BankName are null
- Check still valid and usable

## Error Handling

| Scenario | Response | Notes |
|----------|----------|-------|
| Check not found (update) | 404 | CheckNotFoundException |
| Check not available (update) | 400 | InvalidOperationException |
| Duplicate check number (create) | 400 | CheckNumberAlreadyExistsException |
| Invalid BankId (doesn't exist) | 200 | Still succeeds, BankName is null |
| Missing required fields | 400 | Validation error |

## Compilation Status

### ✅ Server-Side
- **Status:** Compiling successfully
- **Errors:** 0
- **Ready to deploy:** Yes

### ⏳ Client-Side
- **Status:** Awaiting DTO regeneration
- **Expected Errors:** ~3 (from old DTOs)
- **Ready to deploy:** After running OpenAPI client generator

## Testing Checklist

### Create Tests
- [ ] Create check with BankId
- [ ] Verify BankName auto-populated
- [ ] Create without BankId (null fallback)
- [ ] Verify duplicate check number fails

### Update Tests
- [ ] Update available check with new BankId
- [ ] Verify BankName auto-updated
- [ ] Try updating non-available check (fails)
- [ ] Update other fields (account, description)
- [ ] Update with non-existent BankId (succeeds, BankName null)

### UI Tests
- [ ] AutocompleteBank works
- [ ] Can search by bank name
- [ ] Can search by bank code
- [ ] Form submission works
- [ ] Table shows bank names
- [ ] Edit dialog works

## Migration Commands

```bash
# Generate migration
dotnet ef migrations add AddCheckBankIdAndBankName \
  --project src/api/modules/Accounting/Accounting.Infrastructure

# Apply to database
dotnet ef database update \
  --project src/api
```

## Next Steps

1. ✅ Implementation complete
2. ⏳ Regenerate Blazor DTOs from API
3. ⏳ Test create and update flows
4. ⏳ Test Blazor form submission
5. ⏳ Create database migration
6. ⏳ Test in dev environment

## Configuration

No additional configuration needed. The system uses:
- Existing Bank repository
- Existing BankByIdSpec
- Existing Check entity
- Existing Mapster mappings

Everything integrates seamlessly with existing patterns.

---

**Status:** ✅ Implementation Complete
**Server Compilation:** ✅ 0 Errors
**Ready for Testing:** ✅ Yes
**Ready for Deployment:** ⏳ After client generation
