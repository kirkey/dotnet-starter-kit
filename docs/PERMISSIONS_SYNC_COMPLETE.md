# API and UI Permissions Synchronization - Complete

**Date:** December 13, 2025  
**Status:** ✅ Complete

## Overview

Synchronized the API permissions (backend) with the UI permissions (Angular frontend) to ensure consistency across the entire application. The frontend previously only supported ~25 resources and ~26 actions, while the backend had 138 resources and 83 actions.

## Changes Made

### 1. Updated Angular Permission Model (`permission.model.ts`)

#### Resources Expanded
- **Before:** 25 resources (basic modules only)
- **After:** 138 resources (complete coverage)

**Added Resource Categories:**
- **MicroFinance Module** (70+ resources)
  - Organization & Setup: Branches, BranchTargets, MfiConfigurations, Staff, StaffTrainings
  - Member Management: Members, MemberGroups, GroupMemberships, KycDocuments, CustomerSegments, CustomerSurveys
  - Product Catalog: LoanProducts, SavingsProducts, ShareProducts, FeeDefinitions, InsuranceProducts, InvestmentProducts
  - Accounts: SavingsAccounts, ShareAccounts, FixedDeposits, InvestmentAccounts, InsurancePolicies
  - Loan Operations: Loans, LoanApplications, LoanSchedules, LoanRepayments, LoanCollaterals, LoanGuarantors, LoanDisbursementTranches, LoanOfficerAssignments, LoanOfficerTargets, LoanRestructures, LoanWriteOffs
  - Collateral Management: CollateralTypes, CollateralValuations, CollateralInsurances, CollateralReleases
  - Collections & Recovery: CollectionCases, CollectionActions, CollectionStrategies, PromiseToPays, DebtSettlements, LegalActions
  - Transactions: SavingsTransactions, ShareTransactions, FeeCharges, MobileTransactions, InvestmentTransactions
  - Insurance: InsuranceClaims
  - Risk & Compliance: AmlAlerts, CreditScores, CreditBureauInquiries, CreditBureauReports, RiskAlerts, RiskCategories, RiskIndicators, Documents
  - Workflows & Approvals: ApprovalWorkflows, ApprovalRequests, CustomerCases
  - Communications: CommunicationTemplates, CommunicationLogs, MarketingCampaigns
  - Digital Channels: AgentBankings, MobileWallets, PaymentGateways, QrPayments, UssdSessions
  - Cash Management: CashVaults, TellerSessions
  - Reporting: ReportDefinitions, ReportGenerations

#### Actions Expanded
- **Before:** 26 actions
- **After:** 83 actions (complete coverage)

**Added Action Categories:**
- **Financial Actions:**
  - Disburse, Deposit, Withdraw, Transfer, WriteOff, Mature

- **Status Management:**
  - Activate, Deactivate, Suspend, Close, Freeze, Unfreeze, Renew, Return

- **MicroFinance-Specific:**
  - Collections: Escalate, MarkBroken, RecordPayment
  - AML/Compliance: FileSar, Confirm, Clear
  - Insurance: RecordPremium
  - Investment: Invest, Redeem, SetupSip
  - Agent Banking: RecordAudit, UpgradeTier, CreditFloat, DebitFloat
  - Loan Operations: ApplyPayment, Restructure

### 2. Enhanced Permission Dialog Component

#### Icon Mappings
Updated `getActionIcon()` method to include Material icons for all 83 actions:

```typescript
// Financial icons: payments, account_balance, money_off, swap_horiz, etc.
// Workflow icons: check_circle, cancel, send, done_all, etc.
// Status icons: check_circle_outline, highlight_off, pause_circle_outline, etc.
// MicroFinance icons: priority_high, report_problem, verified, trending_up, etc.
```

**File:** `src/apps/angular/src/app/features/identity/roles/permission-dialog/permission-dialog.component.ts`

### 3. Enhanced Permission Service

Added 26+ helper methods for common permission checks:

#### Standard CRUD Helpers
- `canView(resource)`, `canCreate(resource)`, `canUpdate(resource)`, `canDelete(resource)`
- `canSearch(resource)`, `canImport(resource)`, `canExport(resource)`

#### Workflow & Approval Helpers
- `canApprove(resource)`, `canReject(resource)`, `canSubmit(resource)`
- `canProcess(resource)`, `canComplete(resource)`

#### Financial Operation Helpers
- `canDisburse(resource)` - For loan disbursements
- `canDeposit(resource)` - For account deposits
- `canWithdraw(resource)` - For account withdrawals
- `canWriteOff(resource)` - For loan write-offs
- `canPost(resource)` - For accounting posts

#### Status Management Helpers
- `canActivate(resource)`, `canDeactivate(resource)`
- `canSuspend(resource)`, `canClose(resource)`
- `canAssign(resource)`

**File:** `src/apps/angular/src/app/core/services/permission.service.ts`

### 4. Updated getAllPermissions Function

Enhanced the function with:
- Support for all 138 resources
- Common actions array (View, Search, Create, Update, Delete, Export, Import)
- Documentation noting that actual permissions come from backend API
- Proper isBasic and isRoot flag handling

## Verification

### Backend Files (Already Complete)
✅ `src/Shared/Authorization/FshResources.cs` - 138 resources defined  
✅ `src/Shared/Authorization/FshActions.cs` - 83 actions defined  
✅ `src/Shared/Authorization/FshPermissions.cs` - All permission combinations defined  

### Frontend Files (Now Synchronized)
✅ `src/apps/angular/src/app/core/models/permission.model.ts` - Updated to match backend  
✅ `src/apps/angular/src/app/features/identity/roles/permission-dialog/permission-dialog.component.ts` - Enhanced with all action icons  
✅ `src/apps/angular/src/app/core/services/permission.service.ts` - Enhanced with helper methods  

### Blazor App
✅ Already uses shared backend authorization directly via `@using Shared.Authorization`  
✅ No changes needed - automatically synchronized

## Permission Naming Convention

Both API and UI now use consistent naming:
```
Permissions.{Resource}.{Action}

Examples:
- Permissions.Loans.View
- Permissions.SavingsAccounts.Deposit
- Permissions.LoanApplications.Approve
- Permissions.Members.Activate
```

## Component Usage Examples

### Using Permission Service in Components

```typescript
// Standard CRUD
if (this.permissionService.canView(FshResources.Loans)) { ... }
if (this.permissionService.canCreate(FshResources.Members)) { ... }

// Financial Operations
if (this.permissionService.canDisburse(FshResources.Loans)) { ... }
if (this.permissionService.canDeposit(FshResources.SavingsAccounts)) { ... }

// Workflow Actions
if (this.permissionService.canApprove(FshResources.LoanApplications)) { ... }
if (this.permissionService.canReject(FshResources.LoanApplications)) { ... }

// Status Management
if (this.permissionService.canActivate(FshResources.Members)) { ... }
if (this.permissionService.canClose(FshResources.SavingsAccounts)) { ... }

// Direct permission check
if (this.permissionService.hasPermission(FshActions.WriteOff, FshResources.Loans)) { ... }
```

### Using in Templates

```typescript
// Component
canDisburseLoan = computed(() => 
  this.permissionService.hasPermission(FshActions.Disburse, FshResources.Loans)
);

// Template
@if (canDisburseLoan()) {
  <button (click)="disburseLoan()">Disburse Loan</button>
}
```

## Benefits

1. **Complete Coverage**: All 138 backend resources and 83 actions now available in UI
2. **Type Safety**: TypeScript constants for all resources and actions
3. **Developer Experience**: Helper methods for common operations
4. **Consistency**: Identical permission structure across API and UI
5. **Maintainability**: Centralized permission definitions
6. **Scalability**: Easy to add new permissions following established pattern

## Future Recommendations

### 1. API-Driven Permissions (Optional Enhancement)
Consider fetching available permissions from a new API endpoint instead of maintaining static definitions:

```typescript
GET /api/permissions/available
Response: [
  { name: "Permissions.Loans.View", description: "View Loans", ... },
  { name: "Permissions.Loans.Create", description: "Create Loans", ... },
  ...
]
```

**Benefits:**
- Automatic synchronization when backend changes
- No manual updates needed for new permissions
- Single source of truth

### 2. Resource Categorization in UI
With 138 resources, consider organizing the permission dialog:

```typescript
export const ResourceCategories = {
  Identity: [FshResources.Users, FshResources.Roles, ...],
  Catalog: [FshResources.Products, FshResources.Brands, ...],
  HumanResources: [FshResources.Employees, FshResources.Payroll, ...],
  MicroFinance: {
    Organization: [FshResources.Branches, FshResources.Staff, ...],
    Members: [FshResources.Members, FshResources.MemberGroups, ...],
    Loans: [FshResources.Loans, FshResources.LoanApplications, ...],
    ...
  },
  ...
};
```

**UI Improvements:**
- Tabbed interface or accordion
- Tree-view structure
- Search/filter functionality
- Bulk selection by category

### 3. Action-Resource Mapping
Not all actions apply to all resources. Create a mapping based on actual backend permissions:

```typescript
export const ResourceActions: Record<string, string[]> = {
  [FshResources.Loans]: [
    FshActions.View, FshActions.Create, FshActions.Update,
    FshActions.Approve, FshActions.Disburse, FshActions.Close,
    FshActions.WriteOff, FshActions.Restructure, FshActions.ApplyPayment
  ],
  [FshResources.SavingsAccounts]: [
    FshActions.View, FshActions.Create, FshActions.Activate,
    FshActions.Deposit, FshActions.Withdraw, FshActions.Close,
    FshActions.Freeze, FshActions.Unfreeze
  ],
  ...
};
```

**Benefits:**
- Shows only relevant actions per resource
- Prevents invalid permission assignments
- Cleaner UI
- Better user experience

## Testing Checklist

- [x] Verify TypeScript compilation succeeds
- [ ] Test permission dialog displays all resources
- [ ] Test permission dialog displays all action icons correctly
- [ ] Verify role permissions can be updated with new permissions
- [ ] Test permission checks work for MicroFinance features
- [ ] Verify Blazor app still works with shared authorization
- [ ] Test permission service helper methods
- [ ] Verify permission caching works correctly

## Related Files

### Frontend
- `src/apps/angular/src/app/core/models/permission.model.ts`
- `src/apps/angular/src/app/core/services/permission.service.ts`
- `src/apps/angular/src/app/features/identity/roles/permission-dialog/permission-dialog.component.ts`
- `src/apps/angular/src/app/features/identity/roles/role-permissions/role-permissions.component.ts`

### Backend
- `src/Shared/Authorization/FshPermissions.cs`
- `src/Shared/Authorization/FshResources.cs`
- `src/Shared/Authorization/FshActions.cs`
- `src/api/framework/Infrastructure/Auth/Policy/RequiredPermissionAttribute.cs`

### Blazor
- `src/apps/blazor/client/Pages/Identity/Roles/RolePermissions.razor`
- Uses shared backend authorization directly

## Notes

- All warnings about unused methods in `permission.service.ts` are expected - these are public API methods for component use
- The `getAllPermissions()` function generates a simplified set; actual permissions from API may differ
- Backend FshPermissions.cs is the single source of truth for actual permission definitions
- UI model serves as type definitions and intellisense support

## Completion Status

✅ **API Permissions**: 138 resources, 83 actions  
✅ **UI Permissions Model**: 138 resources, 83 actions  
✅ **Permission Dialog Icons**: 83 action icons mapped  
✅ **Permission Service Helpers**: 26+ helper methods  
✅ **Documentation**: Complete  
✅ **Type Safety**: Full TypeScript support  

---

**Synchronized By:** GitHub Copilot  
**Date:** December 13, 2025  
**Version:** 1.0

