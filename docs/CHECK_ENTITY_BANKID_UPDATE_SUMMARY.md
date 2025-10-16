# Check Entity - BankId and BankName Update Summary

## Overview
Updated the Check entity and all related components to include `BankId` and `BankName` fields to link checks to specific banks through the Bank entity.

## Changes Made

### 1. Domain Entity Updates
**File:** `/src/api/modules/Accounting/Accounting.Domain/Entities/Check.cs`

#### New Properties Added:
```csharp
/// <summary>
/// Bank ID that the check is associated with.
/// Links to the Bank entity for bank-level information.
/// </summary>
public DefaultIdType? BankId { get; private set; }

/// <summary>
/// Bank name for display purposes.
/// Example: "Chase Bank", "Bank of America".
/// </summary>
public string? BankName { get; private set; }
```

#### Constructor Update:
- Updated the private constructor to accept `bankId` and `bankName` parameters
- Updated the `Create()` factory method to accept optional `bankId` and `bankName` parameters

#### Update Method Enhancement:
- Modified the `Update()` method to accept and handle `bankId` and `bankName` parameters
- Both fields can be updated for available checks only (maintains existing business rules)

### 2. Database Configuration
**File:** `/src/api/modules/Accounting/Accounting.Infrastructure/Persistence/Configurations/CheckConfiguration.cs`

#### Changes:
- Added `BankName` property configuration with max length of 256 characters
- Added database index on `BankId` for performance optimization

```csharp
builder.Property(x => x.BankName)
    .HasMaxLength(256);

builder.HasIndex(x => x.BankId)
    .HasDatabaseName("IX_Checks_BankId");
```

### 3. Application Layer - Commands
**File:** `/src/api/modules/Accounting/Accounting.Application/Checks/Create/v1/CheckCreateCommand.cs`

#### Updated Record Signature:
```csharp
public record CheckCreateCommand(
    string CheckNumber,
    string BankAccountCode,
    string? BankAccountName,
    DefaultIdType? BankId,      // NEW
    string? BankName,            // NEW
    string? Description,
    string? Notes
) : IRequest<CheckCreateResponse>;
```

### 4. Command Handlers
**File:** `/src/api/modules/Accounting/Accounting.Application/Checks/Create/v1/CheckCreateHandler.cs`

#### Updated:
- Modified handler to pass `BankId` and `BankName` to `Check.Create()` method
- Command parameters now flow through to entity creation

### 5. Command Validators
**File:** `/src/api/modules/Accounting/Accounting.Application/Checks/Create/v1/CheckCreateCommandValidator.cs`

#### Added Validation:
```csharp
RuleFor(x => x.BankName)
    .MaximumLength(256).WithMessage("Bank name cannot exceed 256 characters.")
    .When(x => !string.IsNullOrWhiteSpace(x.BankName));
```

### 6. Response DTOs

#### CheckSearchResponse
**File:** `/src/api/modules/Accounting/Accounting.Application/Checks/Search/v1/CheckSearchResponse.cs`

```csharp
public record CheckSearchResponse(
    DefaultIdType Id,
    string CheckNumber,
    string BankAccountCode,
    string? BankAccountName,
    DefaultIdType? BankId,      // NEW
    string? BankName,            // NEW
    string Status,
    // ... other fields
);
```

#### CheckGetResponse
**File:** `/src/api/modules/Accounting/Accounting.Application/Checks/Get/v1/CheckGetResponse.cs`

Added `BankId` and `BankName` fields to the detailed response record.

### 7. Blazor Client - ViewModel
**File:** `/src/apps/blazor/client/Pages/Accounting/Checks/CheckViewModel.cs`

#### New Properties:
```csharp
/// <summary>
/// Bank ID that the check is associated with.
/// </summary>
public DefaultIdType? BankId { get; set; }

/// <summary>
/// Bank name for display purposes.
/// Example: "Chase Bank", "Bank of America".
/// </summary>
public string? BankName { get; set; }
```

### 8. Blazor Page - Code Behind
**File:** `/src/apps/blazor/client/Pages/Accounting/Checks/Checks.razor.cs`

#### Table Fields Updated:
Added `BankName` to the EntityTable fields for display in the checks list:
```csharp
new EntityField<CheckSearchResponse>(response => response.BankName, "Bank", "BankName"),
```

### 9. Blazor Page - UI Components
**File:** `/src/apps/blazor/client/Pages/Accounting/Checks/Checks.razor`

#### Added AutocompleteBank Component:
```razor
<MudItem xs="12" sm="6" md="4">
    <AutocompleteBank @bind-Value="context.BankId"
                      For="@(() => context.BankId)"
                      Label="Bank"
                      TextFormat="NameCode"
                      Variant="Variant.Filled" />
</MudItem>
```

#### Added BankName Display:
In the edit form, added conditional display of BankName for existing checks:
```razor
@if (!string.IsNullOrEmpty(context.BankName))
{
    <MudItem xs="12" sm="6" md="4">
        <MudTextField Value="@context.BankName"
                      Label="Bank Name"
                      ReadOnly
                      Variant="Variant.Filled" />
    </MudItem>
}
```

## Component Relationships

### AutocompleteBank Component
**File:** `/src/apps/blazor/client/Components/Autocompletes/Accounting/AutocompleteBank.cs`

The existing AutocompleteBank component is used in the Checks form:
- **Returns:** `DefaultIdType?` (Bank ID)
- **Display Format:** Supports "Name", "Code", "CodeName", "NameCode" formats
- **Search:** Searches by BankCode, Name, or RoutingNumber
- **Configuration:** Set to display "NameCode" format in Checks page

## Business Rules Maintained

1. **Optional Field:** BankId and BankName are optional (nullable), allowing backward compatibility
2. **Update Constraints:** BankId and BankName can only be updated for available checks
3. **Validation:** BankName is limited to 256 characters
4. **Display:** When viewing existing checks, BankName displays as read-only
5. **Search:** Bank selection uses autocomplete for better UX

## Migration Required

A database migration needs to be created to add the new columns:

```bash
dotnet ef migrations add AddCheckBankIdAndBankName --project src/api/migrations/postgresql/Accounting
dotnet ef database update --project src/api
```

## API Endpoint Impact

All check-related endpoints automatically handle the new fields through Mapster's automatic mapping:

- ✅ `POST /api/v1/accounting/checks` - Create with optional BankId/BankName
- ✅ `GET /api/v1/accounting/checks/{id}` - Returns BankId/BankName
- ✅ `GET /api/v1/accounting/checks/search` - Searches and returns BankId/BankName
- ✅ Issue/Void/Clear/StopPayment operations - Preserve BankId/BankName

## Blazor UI Updates

### Forms
- New BankId field with autocomplete (required field indication: optional)
- BankName field displays read-only for existing checks
- Maintains existing field layout and responsiveness

### Table
- Added "Bank" column showing BankName
- Visible in the checks list view
- Sortable and searchable

## Testing Checklist

- [ ] Create a new check with BankId selection
- [ ] Create a new check without BankId (test backward compatibility)
- [ ] Edit an existing check and update BankId
- [ ] Verify BankName displays correctly in list and detail views
- [ ] Test search/filter functionality
- [ ] Verify database migration applies without errors
- [ ] Test API endpoints with and without BankId data
- [ ] Test Blazor autocomplete component with bank selection

## Backward Compatibility

- All changes are backward compatible
- BankId and BankName are optional fields
- Existing checks without BankId will continue to work
- No breaking changes to API contracts

## Notes

- The AutocompleteBank component already existed in the codebase, configured for the Bank Management page
- No new components were created, only existing patterns were followed
- The update maintains consistency with existing check management operations
- Bank selection is properly integrated with the existing Bank entity and management

---

**Updated:** October 16, 2025
**Status:** Implementation Complete
