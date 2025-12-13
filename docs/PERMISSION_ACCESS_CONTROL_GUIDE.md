# Permission-Based Access Control - Complete Implementation Guide

**Date:** December 13, 2025  
**Status:** ✅ Complete  
**Applies To:** Angular & Blazor Applications

## Overview

This document describes the complete permission-based access control system that restricts user access to modules, menus, pages, and actions based on their assigned permissions.

## Key Principle

**If a user does not have permission to access a module (e.g., MicroFinance, Accounting, Store, Warehouse), the user will NOT see:**
- The module in the navigation menu
- Any routes/pages within that module
- Any actions/buttons related to that module

## Architecture

### Backend (API)
- **Location:** `src/Shared/Authorization/`
- **Files:**
  - `FshPermissions.cs` - Defines all 596 permissions
  - `FshResources.cs` - Defines all 138 resources
  - `FshActions.cs` - Defines all 83 actions

### Angular Frontend
- **Permission Model:** `src/apps/angular/src/app/core/models/permission.model.ts`
- **Permission Service:** `src/apps/angular/src/app/core/services/permission.service.ts`
- **Menu Service:** `src/apps/angular/src/app/core/services/menu.service.ts`
- **Route Guards:** `src/apps/angular/src/app/core/guards/permission.guard.ts`
- **Directives:** `src/apps/angular/src/app/core/directives/permission.directive.ts`

### Blazor Frontend
- **Menu Service:** `src/apps/blazor/client/Services/Navigation/MenuService.cs`
- **Menu Component:** `src/apps/blazor/client/Layout/NavMenu.razor.cs`
- **Authorization Extensions:** `src/apps/blazor/infrastructure/Auth/AuthorizationServiceExtensions.cs`

## Permission Structure

### Format
```
Permissions.{Resource}.{Action}
```

### Examples
```csharp
Permissions.Accounting.View          // View Accounting module
Permissions.MicroFinance.View        // View MicroFinance module
Permissions.Store.View               // View Store module
Permissions.Warehouse.View           // View Warehouse module
Permissions.Loans.Create             // Create loans
Permissions.SavingsAccounts.Deposit  // Deposit to savings accounts
Permissions.Members.Activate         // Activate members
```

## Module-Level Access Control

### Key Resources for Module Access

| Module | Permission Required | Resource |
|--------|---------------------|----------|
| **Accounting** | `Permissions.Accounting.View` | `FshResources.Accounting` |
| **MicroFinance** | `Permissions.MicroFinance.View` | `FshResources.MicroFinance` |
| **Store** | `Permissions.Store.View` | `FshResources.Store` |
| **Warehouse** | `Permissions.Warehouse.View` | `FshResources.Warehouse` |
| **Human Resources** | `Permissions.Organization.View` | `FshResources.Organization` |
| **Products (Catalog)** | `Permissions.Products.View` | `FshResources.Products` |
| **Messaging** | `Permissions.Messaging.View` | `FshResources.Messaging` |

## Angular Implementation

### 1. Menu Filtering (Automatic)

The `MenuService` automatically filters menu items based on user permissions:

```typescript
// src/apps/angular/src/app/core/services/menu.service.ts

export class MenuService {
  // Menu is automatically filtered via computed signal
  readonly menuSections = computed(() => 
    this.filterMenuByPermissions(this._menuSections())
  );
  
  private filterMenuByPermissions(sections: MenuSection[]): MenuSection[] {
    // Filters out items user doesn't have permission to see
    // If user has no Accounting.View permission, entire Accounting menu is hidden
  }
}
```

**Result:** Users without `Permissions.MicroFinance.View` will NOT see the MicroFinance menu.

### 2. Route Guards

Protect routes from unauthorized access:

```typescript
// In route configuration
const routes: Routes = [
  {
    path: 'accounting',
    loadChildren: () => import('./accounting/accounting.routes'),
    canActivate: [authGuard, permissionGuard],
    data: { 
      action: FshActions.View, 
      resource: FshResources.Accounting 
    }
  },
  {
    path: 'microfinance',
    loadChildren: () => import('./microfinance/microfinance.routes'),
    canActivate: [authGuard, permissionGuard],
    data: { 
      action: FshActions.View, 
      resource: FshResources.MicroFinance 
    }
  }
];
```

**Result:** Direct URL access to `/accounting` or `/microfinance` is blocked if user lacks permission.

### 3. Template Directives

Hide/show UI elements based on permissions:

```html
<!-- Single permission check -->
<button *hasPermission="{ action: 'Create', resource: 'Products' }">
  Create Product
</button>

<!-- Check if user can access MicroFinance -->
<div *hasPermission="{ action: 'View', resource: 'MicroFinance' }">
  <app-microfinance-dashboard />
</div>

<!-- Multiple permissions (ANY) -->
<div *hasAnyPermission="[
  { action: 'View', resource: 'Dashboard' },
  { action: 'View', resource: 'Analytics' }
]">
  Dashboard content
</div>

<!-- Multiple permissions (ALL) -->
<button *hasAllPermissions="[
  { action: 'View', resource: 'Users' },
  { action: 'Update', resource: 'Users' }
]">
  Edit User
</button>

<!-- Inverse logic (show if NO permission) -->
<div *hasPermission="{ action: 'View', resource: 'MicroFinance', inverse: true }">
  You don't have access to MicroFinance module
</div>
```

### 4. Component-Level Checks

```typescript
import { Component, inject, computed } from '@angular/core';
import { PermissionService } from '@core/services/permission.service';
import { FshActions, FshResources } from '@core/models/permission.model';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html'
})
export class ProductsComponent {
  private permissionService = inject(PermissionService);
  
  // Using helper methods
  canCreate = computed(() => 
    this.permissionService.canCreate(FshResources.Products)
  );
  
  canUpdate = computed(() => 
    this.permissionService.canUpdate(FshResources.Products)
  );
  
  canDelete = computed(() => 
    this.permissionService.canDelete(FshResources.Products)
  );
  
  // Direct permission check
  canDisburseLoans = computed(() =>
    this.permissionService.hasPermission(FshActions.Disburse, FshResources.Loans)
  );
  
  // Multiple permissions
  canManageAccounting = computed(() =>
    this.permissionService.hasPermission(FshActions.View, FshResources.Accounting) &&
    this.permissionService.hasPermission(FshActions.Update, FshResources.Accounting)
  );
}
```

## Blazor Implementation

### 1. Menu Filtering (Automatic)

The `NavMenu` component automatically filters menu items:

```csharp
// src/apps/blazor/client/Layout/NavMenu.razor.cs

protected override async Task OnParametersSetAsync()
{
    var user = (await AuthState).User;
    
    // Filter sections and items based on permissions
    foreach (var item in section.SectionItems)
    {
        if (!await HasPermissionAsync(item.Action, item.Resource))
            continue; // Skip if no permission
            
        // Filter child items
        var filteredSubs = new List<MenuSectionSubItemModel>();
        foreach (var sub in item.MenuItems)
        {
            if (!await HasPermissionAsync(sub.Action, sub.Resource))
                continue;
            filteredSubs.Add(sub);
        }
        
        // Only add parent if it has visible children
        if (filteredSubs.Count > 0)
            filteredSection.SectionItems.Add(item);
    }
}
```

**Result:** Menu automatically adapts to user's permissions.

### 2. Page-Level Checks

```csharp
// In Blazor page component

protected override async Task OnInitializedAsync()
{
    var state = await AuthState;
    
    // Check module access
    _canViewMicroFinance = await AuthService.HasPermissionAsync(
        state.User, 
        FshActions.View, 
        FshResources.MicroFinance
    );
    
    // Check specific actions
    _canCreateLoans = await AuthService.HasPermissionAsync(
        state.User, 
        FshActions.Create, 
        FshResources.Loans
    );
    
    _canDisburseLoans = await AuthService.HasPermissionAsync(
        state.User, 
        FshActions.Disburse, 
        FshResources.Loans
    );
}
```

### 3. Template-Level Checks

```razor
@* Blazor Razor syntax *@

@if (_canViewMicroFinance)
{
    <MudCard>
        <MudCardContent>
            @* MicroFinance content *@
        </MudCardContent>
    </MudCard>
}

@if (_canCreateLoans)
{
    <MudButton OnClick="CreateLoan">Create Loan</MudButton>
}

@if (_canDisburseLoans)
{
    <MudButton OnClick="DisburseLoan">Disburse Loan</MudButton>
}
```

## Common Permission Scenarios

### Scenario 1: User Has No MicroFinance Access

**Configuration:**
```csharp
User Role: "Basic User"
Permissions: 
  - Permissions.Products.View
  - Permissions.Products.Create
  - Permissions.Brands.View
```

**Result:**
- ❌ MicroFinance menu is **hidden**
- ❌ Cannot access `/microfinance/*` routes
- ❌ All MicroFinance UI elements are **hidden**
- ✅ Can see and access Products and Brands

### Scenario 2: User Has MicroFinance View Only

**Configuration:**
```csharp
User Role: "MicroFinance Viewer"
Permissions:
  - Permissions.MicroFinance.View
  - Permissions.Loans.View
  - Permissions.Members.View
  - Permissions.SavingsAccounts.View
```

**Result:**
- ✅ MicroFinance menu is **visible**
- ✅ Can access `/microfinance/*` routes
- ✅ Can **view** loans, members, accounts
- ❌ Cannot **create** or **edit** anything
- ❌ No "Create", "Edit", "Delete" buttons visible

### Scenario 3: User Has Full Accounting Access

**Configuration:**
```csharp
User Role: "Accountant"
Permissions:
  - Permissions.Accounting.View
  - Permissions.Accounting.Create
  - Permissions.Accounting.Update
  - Permissions.Accounting.Delete
  - Permissions.Accounting.Post
  - Permissions.Accounting.Approve
```

**Result:**
- ✅ Accounting menu is **visible**
- ✅ Can access all accounting routes
- ✅ Can create journal entries, invoices, bills
- ✅ Can post transactions
- ✅ Can approve entries
- ✅ All CRUD buttons are **visible**

### Scenario 4: User Has No Store Access

**Configuration:**
```csharp
User Role: "Office Worker"
Permissions:
  - Permissions.Dashboard.View
  - Permissions.Todos.View
  - Permissions.Messaging.View
```

**Result:**
- ❌ Store menu is **hidden**
- ❌ Warehouse menu is **hidden**
- ❌ Accounting menu is **hidden**
- ❌ MicroFinance menu is **hidden**
- ✅ Can see Dashboard, Todos, Messaging

## Permission Helper Methods

### Angular Permission Service

```typescript
// Standard CRUD
canView(resource: string): boolean
canCreate(resource: string): boolean
canUpdate(resource: string): boolean
canDelete(resource: string): boolean
canSearch(resource: string): boolean
canImport(resource: string): boolean
canExport(resource: string): boolean

// Workflow & Approval
canApprove(resource: string): boolean
canReject(resource: string): boolean
canSubmit(resource: string): boolean
canProcess(resource: string): boolean
canComplete(resource: string): boolean

// Financial Operations
canDisburse(resource: string): boolean
canDeposit(resource: string): boolean
canWithdraw(resource: string): boolean
canWriteOff(resource: string): boolean
canPost(resource: string): boolean

// Status Management
canActivate(resource: string): boolean
canDeactivate(resource: string): boolean
canSuspend(resource: string): boolean
canClose(resource: string): boolean
canAssign(resource: string): boolean

// Direct permission check
hasPermission(action: string, resource: string): boolean
```

### Blazor Authorization Extensions

```csharp
// Extension method
await AuthService.HasPermissionAsync(user, action, resource);

// Examples
await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.MicroFinance);
await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.Loans);
await AuthService.HasPermissionAsync(user, FshActions.Disburse, FshResources.Loans);
```

## Testing Permission-Based Access

### Test Case 1: Module Access
1. Create role "BasicUser" with NO permissions
2. Assign user to "BasicUser" role
3. Login as that user
4. **Expected:** Only see Home/Dashboard, no module menus

### Test Case 2: Accounting Access
1. Create role "Accountant" with `Permissions.Accounting.View`
2. Assign user to "Accountant" role
3. Login as that user
4. **Expected:**
   - ✅ See Accounting menu
   - ❌ Don't see MicroFinance, Store, Warehouse menus
   - ✅ Can view accounting pages
   - ❌ Cannot create/edit (no Create/Update permissions)

### Test Case 3: MicroFinance Full Access
1. Create role "MFI Manager" with all MicroFinance permissions
2. Assign user to role
3. Login
4. **Expected:**
   - ✅ See MicroFinance menu with all sub-items
   - ✅ See all CRUD buttons
   - ✅ Can create loans, members, accounts
   - ✅ Can approve, disburse, activate

### Test Case 4: Direct URL Access
1. Login as user WITHOUT MicroFinance access
2. Try to navigate to `/microfinance/loans` directly
3. **Expected:**
   - Angular: Redirected to `/unauthorized` 
   - Blazor: Permission check fails, shows unauthorized

## Best Practices

### 1. Always Check Permissions at Multiple Levels

```
✅ Menu Level    - Hide menu items
✅ Route Level   - Block navigation
✅ Page Level    - Verify on load
✅ Action Level  - Hide buttons/controls
✅ API Level     - Server validates (most important!)
```

### 2. Use Appropriate Permission Granularity

```typescript
// ❌ Too broad
hasPermission('View', 'MicroFinance')

// ✅ Specific to feature
hasPermission('View', 'Loans')
hasPermission('Disburse', 'Loans')
hasPermission('Create', 'Members')
```

### 3. Combine Module and Feature Permissions

```typescript
// Check both module access AND specific feature
const canDisburseLoan = computed(() => 
  this.permissionService.hasPermission(FshActions.View, FshResources.MicroFinance) &&
  this.permissionService.hasPermission(FshActions.Disburse, FshResources.Loans)
);
```

### 4. Provide Feedback for Missing Permissions

```html
<!-- Angular -->
<div *hasPermission="{ action: 'View', resource: 'MicroFinance' }">
  <app-microfinance-content />
</div>

<div *hasPermission="{ action: 'View', resource: 'MicroFinance', inverse: true }">
  <mat-card>
    <mat-card-content>
      <p>You don't have permission to access MicroFinance.</p>
      <p>Please contact your administrator.</p>
    </mat-card-content>
  </mat-card>
</div>
```

## Troubleshooting

### Issue: Menu items not filtering

**Solution:**
- Ensure `MenuService.refreshMenu()` is called after login
- Verify `PermissionService.loadUserPermissions()` is called
- Check that menu items have correct `action` and `resource` properties

### Issue: User can access route directly

**Solution:**
- Add `permissionGuard` to route configuration
- Ensure route data includes `action` and `resource`
- Create `/unauthorized` route for redirects

### Issue: Permissions not updating after role change

**Solution:**
- Call `PermissionService.loadUserPermissions()` after role update
- Call `MenuService.refreshMenu()` to re-filter menu
- Consider requiring re-login for role changes

## Summary

The permission-based access control system ensures that:

1. **Menu Visibility:** Users only see menu items they have permission to access
2. **Route Protection:** Direct URL access is blocked without proper permissions
3. **UI Elements:** Buttons and actions are hidden based on permissions
4. **Module Access:** Entire modules (Accounting, MicroFinance, Store, Warehouse) are hidden if user lacks `View` permission
5. **Granular Control:** Individual features within modules can be controlled separately

**Key Files:**
- Angular: `menu.service.ts`, `permission.service.ts`, `permission.guard.ts`, `permission.directive.ts`
- Blazor: `MenuService.cs`, `NavMenu.razor.cs`, `AuthorizationServiceExtensions.cs`
- Shared: `FshPermissions.cs`, `FshResources.cs`, `FshActions.cs`

---

**Last Updated:** December 13, 2025  
**Version:** 1.0  
**Status:** Production Ready ✅

