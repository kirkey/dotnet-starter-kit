# Image Upload Implementation for Items and Suppliers

## Summary
Added image upload functionality to both Items and Suppliers pages, mirroring the implementation from Categories.

## Changes Made

### Backend Changes

#### 1. Item Entity (Store Module)
**Files Modified:**
- `/api/modules/Store/Store.Application/Items/Create/v1/CreateItemCommand.cs`
  - Added `ImageUrl` property (string?)
  - Added `Image` property (FileUploadCommand?)
  - Added using statement for `FSH.Framework.Core.Storage.File.Features`

- `/api/modules/Store/Store.Application/Items/Update/v1/UpdateItemCommand.cs`
  - Added `ImageUrl` property (string?)
  - Added `Image` property (FileUploadCommand?)
  - Added using statement for `FSH.Framework.Core.Storage.File.Features`

- `/api/modules/Store/Store.Application/Items/Create/v1/CreateItemHandler.cs`
  - Added `IStorageService` dependency injection
  - Added image upload logic before creating the entity
  - Sets `item.ImageUrl` after creation if image was uploaded
  - Added proper logging with ImageUrl

- `/api/modules/Store/Store.Application/Items/Update/v1/UpdateItemHandler.cs`
  - Added `IStorageService` dependency injection
  - Added image upload logic before updating the entity
  - Updates `item.ImageUrl` if image was uploaded
  - Added proper logging with ImageUrl

**Note:** Item entity already has `ImageUrl` property inherited from `AuditableEntity` base class.

#### 2. Supplier Entity (Store Module)
**Files Modified:**
- `/api/modules/Store/Store.Application/Suppliers/Create/v1/CreateSupplierCommand.cs`
  - Added `ImageUrl` parameter to record constructor
  - Added `Image` property (FileUploadCommand?)
  - Added using statement for `FSH.Framework.Core.Storage.File.Features`
  - Updated XML documentation

- `/api/modules/Store/Store.Application/Suppliers/Update/v1/UpdateSupplierCommand.cs`
  - Added `ImageUrl` property (string?)
  - Added `Image` property (FileUploadCommand?)
  - Added using statement for `FSH.Framework.Core.Storage.File.Features`
  - Added XML documentation for new properties

- `/api/modules/Store/Store.Application/Suppliers/Create/v1/CreateSupplierHandler.cs`
  - Added `IStorageService` dependency injection
  - Added image upload logic before creating the entity
  - Sets `supplier.ImageUrl` after creation if image was uploaded
  - Added proper logging with ImageUrl

- `/api/modules/Store/Store.Application/Suppliers/Update/v1/UpdateSupplierHandler.cs`
  - Added `IStorageService` dependency injection
  - Added image upload logic before updating the entity
  - Updates `supplier.ImageUrl` if image was uploaded
  - Added proper logging with ImageUrl

**Note:** Supplier entity already has `ImageUrl` property inherited from `AuditableEntity` base class.

### Frontend Changes (Blazor Client)

#### 1. Items Page
**Files Modified:**
- `/apps/blazor/client/Pages/Store/Items/Items.razor`
  - Added `ImageUploader` component in the edit form
  - Added `@code` block with `TemplateImage` RenderFragment for displaying images in the table

- `/apps/blazor/client/Pages/Store/Items/Items.razor.cs`
  - Added `ImageUrlService` injection
  - Added Image column to the entity fields list with `TemplateImage` template
  - Updated `createFunc` to handle image upload (maps Image properties to FileUploadCommand)
  - Updated `updateFunc` to handle image upload (maps Image properties to FileUploadCommand)

**File Created:**
- `/apps/blazor/client/Pages/Store/Items/ItemViewModel.cs`
  - Created partial class extending `UpdateItemCommand`
  - Added `ImageUrl` property (string?)
  - Added `Image` property (FileUploadCommand?)
  - Added XML documentation

#### 2. Suppliers Page
**Files Modified:**
- `/apps/blazor/client/Pages/Store/Suppliers/Suppliers.razor`
  - Added `ImageUploader` component in the edit form
  - Added `@code` block with `TemplateImage` RenderFragment for displaying supplier logos in the table

- `/apps/blazor/client/Pages/Store/Suppliers/Suppliers.razor.cs`
  - Added `ImageUrlService` injection
  - Added Image column to the entity fields list with `TemplateImage` template
  - Updated `createFunc` to handle image upload (maps Image properties to FileUploadCommand)
  - Updated `updateFunc` to handle image upload (maps Image properties to FileUploadCommand)

**File Created:**
- `/apps/blazor/client/Pages/Store/Suppliers/SupplierViewModel.cs`
  - Created partial class extending `UpdateSupplierCommand`
  - Added `ImageUrl` property (string?)
  - Added `Image` property (FileUploadCommand?)
  - Added XML documentation

## Implementation Pattern
The implementation follows the exact same pattern as Categories:

1. **Backend:**
   - Commands include `ImageUrl` (string?) and `Image` (FileUploadCommand?) properties
   - Handlers inject `IStorageService`
   - Handlers upload images using `storageService.UploadAsync<T>()` before entity creation/update
   - ImageUrl is set on the entity after successful upload
   - Entities use the inherited `ImageUrl` property from `AuditableEntity`

2. **Frontend:**
   - Uses the `ImageUploader` component for file selection and preview
   - Displays images in the table using `TemplateImage` RenderFragment with `MudImage`
   - Uses `ImageUrlService.GetAbsoluteUrl()` to resolve image URLs
   - ViewModels extend the Update commands via partial classes to add Image/ImageUrl properties
   - Create/Update functions map the Image property to FileUploadCommand for API calls

## Next Steps (Required)

### 1. Regenerate API Client
The Blazor API client needs to be regenerated to include the new Image and ImageUrl properties in the DTOs:

```bash
# Navigate to the project root
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src

# Regenerate the API client (exact command depends on your setup)
# This will update the Client.cs file with the new properties
```

### 2. Database Migration (If Needed)
Check if the `ImageUrl` column exists in the Items and Suppliers tables. Since both entities inherit from `AuditableEntity`, they should already have the `ImageUrl` column. If not, create a migration:

```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api
dotnet ef migrations add AddImageUrlToItemsAndSuppliers --project modules/Store/Store.Infrastructure --startup-project server
```

### 3. Test the Implementation
After regenerating the API client:

1. **Test Items:**
   - Create a new item with an image
   - Update an existing item with an image
   - Verify image displays in the table
   - Verify image displays in the edit form

2. **Test Suppliers:**
   - Create a new supplier with a logo
   - Update an existing supplier with a logo
   - Verify logo displays in the table
   - Verify logo displays in the edit form

## Code Consistency
- Follows CQRS pattern
- DRY principle applied (reused ImageUploader component and pattern from Categories)
- Comprehensive XML documentation added to all new/modified classes and methods
- Uses the same storage service abstraction as Categories
- Consistent error handling and logging

## Notes
- The `ImageUrl` property is stored as an absolute URI returned by the storage service
- Images are uploaded to storage using the `FileType.Image` enum
- The `__builder` parameter warning in RenderFragments can be suppressed as it's used internally by Blazor
- Unused field warnings for `_table` are expected and can be ignored (used by EntityTable component via ref)

