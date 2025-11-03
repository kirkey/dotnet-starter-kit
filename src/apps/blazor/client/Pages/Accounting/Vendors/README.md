# Vendors Implementation - Blazor Client

## Overview
The Vendors module provides a complete CRUD interface for managing vendor/supplier information in the Accounting module. This implementation follows the established patterns used in other Accounting entities like Payees, Banks, and Checks.

## Files Created

### 1. `/Pages/Accounting/Vendors/Vendors.razor`
The main UI component that renders the vendor management interface using the `EntityTable` component.

**Features:**
- Create, Read, Update, Delete (CRUD) operations
- Search and filter functionality
- Responsive grid layout for form fields
- Chart of Account autocomplete for expense account selection
- Comprehensive field validation

**Form Fields:**
- Vendor Code (required)
- Name (required)
- TIN (Tax Identification Number)
- Address
- Billing Address
- Contact Person
- Email
- Phone Number
- Payment Terms (e.g., Net 30, Net 60, COD)
- Default Expense Account (with autocomplete)
- Description
- Notes

### 2. `/Pages/Accounting/Vendors/Vendors.razor.cs`
The code-behind file containing the business logic for the Vendors page.

**Key Components:**
- `EntityServerTableContext` configuration
- Field definitions for data grid display
- CRUD operation mappings to API endpoints
- Search functionality using `VendorSearchQuery`
- Mapster adapters for DTO transformations

### 3. `/Pages/Accounting/Vendors/VendorViewModel.cs`
The view model class that represents vendor data in the UI layer.

**Properties:**
- All vendor fields with comprehensive documentation
- Maps to API commands (Create/Update) via Mapster
- Follows DRY principles by reusing the same model for both create and edit operations

### 4. `/Services/Navigation/MenuService.cs` (Modified)
Added the Vendors menu item to the Accounting section.

**Menu Configuration:**
- Title: "Vendors"
- Icon: `Icons.Material.Filled.Business`
- Route: `/accounting/vendors`
- Status: `PageStatus.InProgress`
- Permissions: Requires `FshActions.View` on `FshResources.Accounting`

## API Integration

The implementation uses the auto-generated API client located in `/apps/blazor/infrastructure/Api/Client.cs`.

**API Endpoints Used:**
- `VendorSearchEndpointAsync` - Search and paginate vendors
- `VendorCreateEndpointAsync` - Create new vendor
- `VendorUpdateEndpointAsync` - Update existing vendor
- `VendorDeleteEndpointAsync` - Delete vendor

**API Commands & Queries:**
- `VendorSearchQuery` - Search with pagination
- `VendorSearchResponse` - Search result DTO
- `VendorCreateCommand` - Create vendor command
- `VendorUpdateCommand` - Update vendor command

## Usage

### Accessing the Page
Navigate to `/accounting/vendors` or use the main navigation menu:
**Modules → Accounting → Vendors**

### Creating a Vendor
1. Click the "Add Vendor" button
2. Fill in the required fields (Vendor Code, Name)
3. Optionally fill in additional fields (address, contact, terms, etc.)
4. Select a default expense account using the autocomplete
5. Click "Save"

### Editing a Vendor
1. Click the edit icon on any vendor row
2. Modify the desired fields
3. Click "Save"

### Deleting a Vendor
1. Click the delete icon on any vendor row
2. Confirm the deletion

### Searching Vendors
- Use the search box to filter vendors by any field
- Enable advanced search for more granular filtering
- Results are paginated for better performance

## Design Patterns

### CQRS (Command Query Responsibility Segregation)
- Commands: `VendorCreateCommand`, `VendorUpdateCommand`
- Queries: `VendorSearchQuery`
- Clear separation between read and write operations

### DRY (Don't Repeat Yourself)
- Reuses `VendorViewModel` for both create and edit operations
- Leverages Mapster for automatic DTO mapping
- Uses generic `EntityTable` component for consistent UI

### Repository Pattern
- API endpoints abstract data access
- Consistent interface across all entity types

## Validation

Validation is handled at multiple levels:

1. **Client-Side (Blazor):**
   - Form field validation using FluentValidation
   - Required field enforcement
   - Data format validation

2. **API-Side:**
   - `VendorCreateCommandValidator`
   - `VendorUpdateCommandValidator`
   - Business rule enforcement

## Related Entities

Vendors integrate with:
- **Chart of Accounts** - Default expense account mapping
- **Bills** - Purchase invoice tracking
- **Payments** - Vendor payment processing
- **Accounts Payable** - Outstanding balances
- **Purchase Orders** - Procurement workflow

## Future Enhancements

Potential improvements for future releases:
- Vendor performance metrics
- Purchase history tracking
- Vendor rating system
- Document attachment support
- Multi-currency support
- Vendor portal integration
- Automated 1099 report generation
- Vendor approval workflow

## Testing Checklist

- [x] Page renders correctly
- [x] Create vendor operation
- [x] Update vendor operation
- [x] Delete vendor operation
- [x] Search and filter functionality
- [x] Navigation menu integration
- [x] Responsive layout
- [x] Chart of Account autocomplete
- [x] Form validation
- [x] API endpoint integration

## Notes

- The implementation uses `PageStatus.InProgress` to indicate the feature is functional but may receive additional refinements
- All API endpoints are versioned (currently v1)
- The module respects user permissions via `FshActions` and `FshResources`
- Compatible with the existing tenant-based architecture

