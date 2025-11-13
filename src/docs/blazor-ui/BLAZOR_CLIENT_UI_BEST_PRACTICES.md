# Blazor Client UI Best Practices - Constitution & Baseline

**Version:** 1.0  
**Last Updated:** November 10, 2025  
**Status:** Official Baseline for All Current & Future UI Development

---

## 📜 Table of Contents

1. [Introduction](#introduction)
2. [Architecture Principles](#architecture-principles)
3. [Component Structure](#component-structure)
4. [Page Organization](#page-organization)
5. [Data Management](#data-management)
6. [State Management](#state-management)
7. [API Integration](#api-integration)
8. [Validation & Error Handling](#validation--error-handling)
9. [Styling & Theming](#styling--theming)
10. [Navigation & Routing](#navigation--routing)
11. [Security & Authorization](#security--authorization)
12. [Performance Optimization](#performance-optimization)
13. [Accessibility](#accessibility)
14. [Code Documentation](#code-documentation)
15. [Testing Guidelines](#testing-guidelines)
16. [Common Patterns](#common-patterns)

---

## 1. Introduction

This document establishes the **official constitution** for all Blazor client UI development in the dotnet-starter-kit project. All current pages and future implementations MUST follow these guidelines to ensure consistency, maintainability, and quality across the entire application.

### Purpose

- Establish consistent patterns across all modules (Accounting, Store, Warehouse, etc.)
- Provide clear guidelines for new feature development
- Ensure code quality and maintainability
- Facilitate code reviews and onboarding

### Scope

This applies to:
- All Blazor components (.razor files)
- Code-behind files (.razor.cs)
- Shared components and services
- Navigation and layout components
- Dialogs and modals
- Form components

---

## 2. Architecture Principles

### 2.1 Component-Based Architecture

✅ **DO:**
- Create reusable components for common UI patterns
- Follow Single Responsibility Principle (SRP) for components
- Use composition over inheritance
- Keep components focused and small (< 300 lines)

❌ **DON'T:**
- Create monolithic components with multiple responsibilities
- Duplicate code across components
- Mix business logic with presentation logic

### 2.2 Separation of Concerns

✅ **DO:**
- Use code-behind (.razor.cs) for all logic
- Keep .razor files for markup only
- Separate data access from UI logic
- Use ViewModels for form data

```csharp
// ✅ CORRECT: Logic in code-behind
public partial class Invoices
{
    private EntityServerTableContext<InvoiceResponse, DefaultIdType, InvoiceViewModel> Context { get; set; } = null!;
    
    protected override Task OnInitializedAsync()
    {
        // Initialization logic here
        return Task.CompletedTask;
    }
}
```

❌ **DON'T:**
```razor
@* ❌ WRONG: Logic in razor file *@
@code {
    protected override async Task OnInitializedAsync()
    {
        // Logic should be in .razor.cs
    }
}
```

### 2.3 CQRS Pattern Alignment

✅ **DO:**
- Use Commands for write operations (Create, Update, Delete)
- Use Requests for read operations (Search, Get)
- Map ViewModels to Commands/Requests explicitly
- Keep API client calls in appropriate handlers

```csharp
// ✅ CORRECT: CQRS pattern
createFunc: async viewModel =>
{
    var command = new CreateInvoiceCommand
    {
        InvoiceNumber = viewModel.InvoiceNumber,
        MemberId = viewModel.MemberId!.Value,
        // ... map properties
    };
    await Client.InvoiceCreateEndpointAsync("1", command);
}
```

---

## 3. Component Structure

### 3.1 File Organization

Each feature should follow this structure:

```
/Pages/[Module]/[Feature]/
├── [Feature].razor              # Main page markup
├── [Feature].razor.cs           # Code-behind with logic
├── [Feature]DetailsDialog.razor # Details/View dialog
├── [Feature]DetailsDialog.razor.cs
├── Components/                   # Feature-specific components
│   ├── [Feature]Component.razor
│   └── [Feature]Component.razor.cs
└── ViewModels/                   # If complex models needed
    └── [Feature]ViewModel.cs
```

**Example:**
```
/Pages/Accounting/Invoices/
├── Invoices.razor
├── Invoices.razor.cs
├── InvoiceDetailsDialog.razor
├── InvoiceDetailsDialog.razor.cs
└── Components/
    ├── InvoiceLineItemsGrid.razor
    └── InvoiceLineItemsGrid.razor.cs
```

### 3.2 Component Naming Conventions

✅ **DO:**
- Use PascalCase for component names
- Use descriptive, feature-based names
- Add Dialog/Component suffix when appropriate
- Keep names concise but meaningful

**Examples:**
- ✅ `Invoices.razor` (main page)
- ✅ `InvoiceDetailsDialog.razor` (dialog)
- ✅ `InvoiceApproveDialog.razor` (action dialog)
- ✅ `InvoiceLineItemsGrid.razor` (sub-component)

❌ **DON'T:**
- ❌ `Invoice.razor` (ambiguous - singular vs plural)
- ❌ `InvoicesPage.razor` (redundant suffix)
- ❌ `Dlg.razor` (unclear abbreviation)

### 3.3 Component Parameters

✅ **DO:**
- Use `[Parameter]` attribute for public parameters
- Use `[CascadingParameter]` for inherited values
- Document parameters with XML comments
- Provide default values when appropriate
- Use nullable reference types appropriately

```csharp
/// <summary>
/// Invoice identifier to load details for.
/// </summary>
[Parameter]
public DefaultIdType InvoiceId { get; set; }

/// <summary>
/// Dialog instance reference.
/// </summary>
[CascadingParameter]
public IMudDialogInstance MudDialog { get; set; } = null!;

/// <summary>
/// Optional callback when invoice is updated.
/// </summary>
[Parameter]
public EventCallback<InvoiceResponse> OnInvoiceUpdated { get; set; }
```

---

## 4. Page Organization

### 4.1 EntityTable Pattern (Standard CRUD Pages)

All list/management pages MUST use the `EntityTable` component with `EntityServerTableContext`:

```csharp
public partial class Invoices
{
    // 1. EntityTable context and reference
    protected EntityServerTableContext<InvoiceResponse, DefaultIdType, InvoiceViewModel> Context { get; set; } = null!;
    private EntityTable<InvoiceResponse, DefaultIdType, InvoiceViewModel> _table = null!;

    // 2. Search filter properties
    private string? InvoiceNumber { get; set; }
    private string? Status { get; set; }
    private DateTime? InvoiceDateFrom { get; set; }
    private DateTime? InvoiceDateTo { get; set; }

    // 3. Dialog state properties
    private bool _detailsDialogVisible;
    private readonly DialogOptions _dialogOptions = new()
    {
        CloseOnEscapeKey = true,
        MaxWidth = MaxWidth.ExtraLarge,
        FullWidth = true
    };

    // 4. Initialization
    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<InvoiceResponse, DefaultIdType, InvoiceViewModel>(
            entityName: "Invoice",
            entityNamePlural: "Invoices",
            entityResource: FshResources.Accounting,
            fields: [...],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter => {...},
            createFunc: async viewModel => {...},
            updateFunc: async (id, viewModel) => {...},
            deleteFunc: async id => {...}
        );
        return Task.CompletedTask;
    }

    // 5. Action methods (workflow operations)
    // 6. Helper methods (color coding, formatting, etc.)
}
```

### 4.2 Page Structure Order

✅ **DO:** Organize code in this order:

1. **Component Properties** (EntityTable, references)
2. **Search Filters** (grouped together)
3. **Dialog State** (visibility flags, options)
4. **Lifecycle Methods** (OnInitializedAsync, etc.)
5. **CRUD Operations** (via EntityTableContext)
6. **Workflow Methods** (approve, send, cancel, etc.)
7. **Helper Methods** (formatting, color coding)
8. **Event Handlers** (button clicks, etc.)

### 4.3 Razor Markup Structure

```razor
@* 1. Page directive and usings *@
@page "/accounting/invoices"
@using FSH.Starter.Blazor.Client.Pages.Accounting.Invoices.Components

@* 2. Page header *@
<PageHeader Title="Invoice Management" 
            Header="Invoice Management" 
            SubHeader="Manage customer invoices, track receivables, and process payments." />

@* 3. Action buttons (if any) *@
<MudPaper Elevation="2" Class="pa-4 mb-4">
    <MudStack Row="true" Spacing="2" Wrap="Wrap.Wrap">
        <MudButtonGroup Color="Color.Primary" Variant="Variant.Outlined" Size="Size.Small">
            <MudButton StartIcon="@Icons.Material.Filled.Add" OnClick="@(() => CreateNew())">
                Create New
            </MudButton>
        </MudButtonGroup>
    </MudStack>
</MudPaper>

@* 4. EntityTable with AdvancedSearchContent *@
<EntityTable @ref="_table" 
             TEntity="InvoiceResponse" 
             TId="DefaultIdType" 
             TRequest="InvoiceViewModel" 
             Context="@Context">
    <AdvancedSearchContent>
        @* Search filters *@
    </AdvancedSearchContent>
    <ActionsContent>
        @* Row actions *@
    </ActionsContent>
</EntityTable>

@* 5. Dialogs *@
```

---

## 5. Data Management

### 5.1 ViewModels

✅ **DO:**
- Create ViewModels for complex forms
- Use ViewModels for data binding
- Keep ViewModels simple (data + validation only)
- Use nullable types appropriately

```csharp
/// <summary>
/// View model for invoice create/edit operations.
/// </summary>
public class InvoiceViewModel
{
    public DefaultIdType? Id { get; set; }
    public string? InvoiceNumber { get; set; }
    public DefaultIdType? MemberId { get; set; }
    public DateOnly? InvoiceDate { get; set; }
    public DateOnly? DueDate { get; set; }
    public decimal UsageCharge { get; set; }
    public decimal TaxAmount { get; set; }
    // ... other properties
}
```

### 5.2 Data Mapping with Mapster

✅ **DO:**
- Use Mapster's `.Adapt<T>()` for object mapping
- Map explicitly for Commands/Requests
- Handle null values appropriately

```csharp
// ✅ CORRECT: Explicit mapping for commands
createFunc: async viewModel =>
{
    var command = new CreateInvoiceCommand
    {
        InvoiceNumber = viewModel.InvoiceNumber,
        MemberId = viewModel.MemberId!.Value,
        InvoiceDate = viewModel.InvoiceDate!.Value,
        DueDate = viewModel.DueDate!.Value,
        UsageCharge = viewModel.UsageCharge,
        TaxAmount = viewModel.TaxAmount
    };
    await Client.InvoiceCreateEndpointAsync("1", command);
},

// ✅ CORRECT: Adapt for responses
searchFunc: async filter =>
{
    var request = new SearchInvoicesRequest
    {
        PageNumber = filter.PageNumber,
        PageSize = filter.PageSize,
        Keyword = filter.Keyword,
        OrderBy = filter.OrderBy
    };
    var result = await Client.SearchInvoicesEndpointAsync("1", request);
    return result.Adapt<PaginationResponse<InvoiceResponse>>();
}
```

### 5.3 EntityServerTableContext Configuration

✅ **DO:**
- Define all fields explicitly
- Specify data types for proper formatting
- Enable advanced search when needed
- Implement all CRUD operations consistently

```csharp
Context = new EntityServerTableContext<InvoiceResponse, DefaultIdType, InvoiceViewModel>(
    entityName: "Invoice",
    entityNamePlural: "Invoices",
    entityResource: FshResources.Accounting, // For permissions
    fields:
    [
        new EntityField<InvoiceResponse>(response => response.InvoiceNumber, "Invoice Number", "InvoiceNumber"),
        new EntityField<InvoiceResponse>(response => response.InvoiceDate, "Invoice Date", "InvoiceDate", typeof(DateOnly)),
        new EntityField<InvoiceResponse>(response => response.TotalAmount, "Total Amount", "TotalAmount", typeof(decimal)),
        new EntityField<InvoiceResponse>(response => response.Status, "Status", "Status"),
    ],
    enableAdvancedSearch: true,
    idFunc: response => response.Id,
    searchFunc: async filter => { /* Search implementation */ },
    createFunc: async viewModel => { /* Create implementation */ },
    updateFunc: async (id, viewModel) => { /* Update implementation */ },
    deleteFunc: async id => { /* Delete implementation */ }
);
```

---

## 6. State Management

### 6.1 Component State

✅ **DO:**
- Use private fields with clear naming
- Initialize with sensible defaults
- Use nullable types appropriately
- Document state purpose

```csharp
// ✅ CORRECT: Clear state management
private EntityTable<InvoiceResponse, DefaultIdType, InvoiceViewModel> _table = null!;
private bool _detailsDialogVisible;
private bool _isLoading;
private InvoiceResponse? _selectedInvoice;
private List<SupplierResponse> _suppliers = [];
```

### 6.2 Dialog State

✅ **DO:**
- Use dedicated state classes for complex dialogs
- Initialize with default values
- Reset state after dialog closes
- Use descriptive property names

```csharp
// ✅ CORRECT: Dialog state class
private class MarkAsPaidDialogState
{
    public DefaultIdType InvoiceId { get; set; } = DefaultIdType.Empty;
    public DateTime PaidDate { get; set; } = DateTime.Today;
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal AmountPaid { get; set; }
}

private MarkAsPaidDialogState _markAsPaidCommand = new();

// Reset after use
private void ResetDialogState()
{
    _markAsPaidCommand = new MarkAsPaidDialogState();
}
```

### 6.3 Loading States

✅ **DO:**
- Show loading indicators for async operations
- Disable buttons during processing
- Provide feedback to users
- Handle errors gracefully

```csharp
private bool _isProcessing;

private async Task ApproveInvoice(DefaultIdType id)
{
    try
    {
        _isProcessing = true;
        StateHasChanged(); // Update UI immediately
        
        await Client.ApproveInvoiceEndpointAsync("1", id);
        Snackbar.Add("Invoice approved successfully", Severity.Success);
        await _table.ReloadDataAsync();
    }
    catch (Exception ex)
    {
        Snackbar.Add($"Error: {ex.Message}", Severity.Error);
    }
    finally
    {
        _isProcessing = false;
        StateHasChanged();
    }
}
```

---

## 7. API Integration

### 7.1 API Client Usage

✅ **DO:**
- Use generated NSwag client (`Client` property)
- Always specify API version ("1")
- Use `.ConfigureAwait(false)` for async calls
- Handle exceptions appropriately

```csharp
// ✅ CORRECT: Proper API usage
searchFunc: async filter =>
{
    var request = new SearchInvoicesRequest
    {
        PageNumber = filter.PageNumber,
        PageSize = filter.PageSize,
        Keyword = filter.Keyword,
        OrderBy = filter.OrderBy
    };
    var result = await Client.SearchInvoicesEndpointAsync("1", request).ConfigureAwait(false);
    return result.Adapt<PaginationResponse<InvoiceResponse>>();
}
```

### 7.2 Search Implementation Pattern

✅ **DO:**
- Create dedicated Request objects
- Map all search filters
- Include pagination parameters
- Handle null/empty filters

```csharp
searchFunc: async filter =>
{
    var request = new SearchInvoicesRequest
    {
        PageNumber = filter.PageNumber,
        PageSize = filter.PageSize,
        Keyword = filter.Keyword,
        OrderBy = filter.OrderBy,
        // Custom filters
        InvoiceNumber = InvoiceNumber,
        Status = Status,
        InvoiceDateFrom = InvoiceDateFrom,
        InvoiceDateTo = InvoiceDateTo
    };
    
    var result = await Client.SearchInvoicesEndpointAsync("1", request).ConfigureAwait(false);
    return result.Adapt<PaginationResponse<InvoiceResponse>>();
}
```

### 7.3 CRUD Operations Pattern

```csharp
// CREATE
createFunc: async viewModel =>
{
    await Client.CreateInvoiceEndpointAsync("1", viewModel.Adapt<CreateInvoiceCommand>());
    Snackbar.Add("Invoice created successfully", Severity.Success);
},

// UPDATE
updateFunc: async (id, viewModel) =>
{
    await Client.UpdateInvoiceEndpointAsync("1", id, viewModel.Adapt<UpdateInvoiceCommand>());
    Snackbar.Add("Invoice updated successfully", Severity.Success);
},

// DELETE
deleteFunc: async id =>
{
    await Client.DeleteInvoiceEndpointAsync("1", id);
    Snackbar.Add("Invoice deleted successfully", Severity.Success);
}
```

### 7.4 Workflow Operations Pattern

✅ **DO:**
- Create dedicated methods for workflow actions
- Show confirmation dialogs for destructive actions
- Reload table after successful operations
- Provide clear success/error messages

```csharp
private async Task ApproveInvoice(DefaultIdType id)
{
    var confirm = await DialogService.ShowMessageBox(
        "Confirm Approval",
        "Are you sure you want to approve this invoice?",
        yesText: "Approve",
        cancelText: "Cancel");
    
    if (confirm != true) return;

    try
    {
        await Client.ApproveInvoiceEndpointAsync("1", id);
        Snackbar.Add("Invoice approved successfully", Severity.Success);
        await _table.ReloadDataAsync();
    }
    catch (Exception ex)
    {
        Snackbar.Add($"Error approving invoice: {ex.Message}", Severity.Error);
    }
}
```

---

## 8. Validation & Error Handling

### 8.1 Form Validation

✅ **DO:**
- Use FluentValidation validators
- Show validation errors inline
- Disable submit until form is valid
- Validate on blur and submit

```razor
<MudForm @ref="_form" Model="@_viewModel" Validation="@(_validator.ValidateValue)">
    <MudTextField @bind-Value="_viewModel.InvoiceNumber"
                  For="@(() => _viewModel.InvoiceNumber)"
                  Label="Invoice Number"
                  Required="true"
                  Immediate="true" />
    
    <MudDatePicker @bind-Date="_viewModel.InvoiceDate"
                   For="@(() => _viewModel.InvoiceDate)"
                   Label="Invoice Date"
                   Required="true" />
</MudForm>
```

### 8.2 Error Handling

✅ **DO:**
- Wrap API calls in try-catch
- Show user-friendly error messages
- Log errors appropriately
- Provide recovery options when possible

```csharp
try
{
    await Client.CreateInvoiceEndpointAsync("1", command);
    Snackbar.Add("Invoice created successfully", Severity.Success);
    await _table.ReloadDataAsync();
}
catch (ApiException ex) when (ex.StatusCode == 409)
{
    Snackbar.Add("Invoice number already exists", Severity.Warning);
}
catch (ApiException ex)
{
    Snackbar.Add($"API Error: {ex.Message}", Severity.Error);
}
catch (Exception ex)
{
    Snackbar.Add($"Unexpected error: {ex.Message}", Severity.Error);
}
```

### 8.3 User Feedback

✅ **DO:**
- Use Snackbar for notifications
- Choose appropriate severity levels
- Keep messages concise and actionable
- Show success, warning, error, and info messages

```csharp
// Success
Snackbar.Add("Operation completed successfully", Severity.Success);

// Warning
Snackbar.Add("This action cannot be undone", Severity.Warning);

// Error
Snackbar.Add($"Error: {ex.Message}", Severity.Error);

// Info
Snackbar.Add("Processing may take a few moments", Severity.Info);
```

---

## 9. Styling & Theming

### 9.1 MudBlazor Components

✅ **DO:**
- Use MudBlazor components exclusively
- Follow MudBlazor naming conventions
- Use built-in variants and colors
- Leverage MudBlazor's responsive features

```razor
<MudPaper Elevation="2" Class="pa-4 mb-4">
    <MudStack Row="true" Spacing="2" Wrap="Wrap.Wrap">
        <MudButton Color="Color.Primary" 
                   Variant="Variant.Filled" 
                   StartIcon="@Icons.Material.Filled.Add">
            Create New
        </MudButton>
    </MudStack>
</MudPaper>
```

### 9.2 Color Usage

✅ **DO:**
- Use semantic colors from MudBlazor
- Create helper methods for dynamic colors
- Be consistent across similar features

```csharp
/// <summary>
/// Gets the status color based on invoice status.
/// </summary>
private static Color GetStatusColor(string? status) => status switch
{
    "Draft" => Color.Default,
    "Sent" => Color.Info,
    "Paid" => Color.Success,
    "Overdue" => Color.Error,
    "Cancelled" => Color.Warning,
    "Voided" => Color.Dark,
    _ => Color.Default
};
```

### 9.3 Status Display

✅ **DO:**
- Use MudChip for status display
- Apply appropriate colors
- Keep status text concise

```razor
<MudChip T="string" Color="@GetStatusColor(context.Status)" 
         Size="Size.Small" 
         Variant="Variant.Filled">
    @context.Status
</MudChip>
```

### 9.4 Spacing and Layout

✅ **DO:**
- Use MudBlazor's spacing classes (pa, ma, etc.)
- Use MudStack for layouts
- Use MudGrid for responsive layouts
- Keep spacing consistent

```razor
@* Padding and margin *@
<MudPaper Class="pa-4 mb-4">
    @* Content *@
</MudPaper>

@* Stack for vertical/horizontal layout *@
<MudStack Row="true" Spacing="2">
    <MudButton>Button 1</MudButton>
    <MudButton>Button 2</MudButton>
</MudStack>

@* Grid for responsive layout *@
<MudGrid>
    <MudItem xs="12" sm="6" md="4">
        <MudTextField Label="Field 1" />
    </MudItem>
    <MudItem xs="12" sm="6" md="4">
        <MudTextField Label="Field 2" />
    </MudItem>
</MudGrid>
```

---

## 10. Navigation & Routing

### 10.1 Route Definition

✅ **DO:**
- Use clear, hierarchical routes
- Follow module/feature pattern
- Use kebab-case for multi-word routes
- Keep routes predictable

```razor
@* ✅ CORRECT *@
@page "/accounting/invoices"
@page "/accounting/purchase-orders"
@page "/store/inventory-transfers"
@page "/warehouse/cycle-counts"
```

### 10.2 Navigation Menu

✅ **DO:**
- Group related features
- Use descriptive menu titles
- Add appropriate icons
- Support page status indicators

```csharp
new MenuItem
{
    Title = "Invoices",
    Icon = Icons.Material.Filled.Receipt,
    Href = "/accounting/invoices",
    PageStatus = PageStatus.Completed
}
```

### 10.3 Breadcrumbs

✅ **DO:**
- Use PageHeader component
- Provide clear navigation path
- Include module context

```razor
<PageHeader Title="Invoice Management" 
            Header="Invoice Management" 
            SubHeader="Manage customer invoices, track receivables, and process payments." />
```

---

## 11. Security & Authorization

### 11.1 Permission Checking

✅ **DO:**
- Check permissions using EntityResource
- Hide unauthorized features
- Check permissions in code-behind
- Use FshActions constants

```csharp
Context = new EntityServerTableContext<InvoiceResponse, DefaultIdType, InvoiceViewModel>(
    entityName: "Invoice",
    entityNamePlural: "Invoices",
    entityResource: FshResources.Accounting, // Required for permissions
    searchAction: FshActions.Search,
    createAction: FshActions.Create,
    updateAction: FshActions.Update,
    deleteAction: FshActions.Delete,
    // ...
);
```

### 11.2 Action Authorization

✅ **DO:**
- Check permissions before showing actions
- Disable buttons for unauthorized actions
- Provide appropriate feedback

```razor
@if (_canApprove)
{
    <MudButton Color="Color.Success" 
               StartIcon="@Icons.Material.Filled.Check"
               OnClick="@(() => ApproveInvoice(context.Id))">
        Approve
    </MudButton>
}
```

---

## 12. Performance Optimization

### 12.1 Efficient Rendering

✅ **DO:**
- Use `@key` for list items
- Minimize re-renders with `ShouldRender()`
- Use virtualization for large lists
- Avoid unnecessary state changes

```razor
@foreach (var item in items)
{
    <div @key="item.Id">
        @* Content *@
    </div>
}
```

### 12.2 Lazy Loading

✅ **DO:**
- Load data on demand
- Use server-side pagination
- Lazy load dropdown data
- Cache frequently accessed data

```csharp
// Load suppliers only when needed
private async Task LoadSuppliersAsync()
{
    if (_suppliers.Any()) return; // Already loaded
    
    var result = await Client.SearchSuppliersEndpointAsync("1", new SearchSuppliersCommand
    {
        PageNumber = 1,
        PageSize = 500,
        OrderBy = ["Name"]
    });
    _suppliers = result.Items?.ToList() ?? [];
}
```

### 12.3 Debouncing

✅ **DO:**
- Debounce search inputs
- Debounce filter changes
- Use appropriate delays

```csharp
private System.Timers.Timer? _debounceTimer;

private void OnSearchChanged(string value)
{
    _debounceTimer?.Stop();
    _debounceTimer = new System.Timers.Timer(300);
    _debounceTimer.Elapsed += async (sender, e) =>
    {
        _debounceTimer?.Stop();
        await InvokeAsync(async () =>
        {
            SearchString = value;
            await _table.ReloadDataAsync();
        });
    };
    _debounceTimer.Start();
}
```

---

## 13. Accessibility

### 13.1 ARIA Labels

✅ **DO:**
- Add aria-label to icon-only buttons
- Use proper heading hierarchy
- Provide text alternatives for images

```razor
<MudButton aria-label="Approve invoice" 
           StartIcon="@Icons.Material.Filled.Check"
           OnClick="@(() => ApproveInvoice(context.Id))">
    Approve
</MudButton>
```

### 13.2 Keyboard Navigation

✅ **DO:**
- Ensure all interactive elements are keyboard accessible
- Support Tab navigation
- Support Enter/Space for activation
- Provide keyboard shortcuts for common actions

### 13.3 Screen Reader Support

✅ **DO:**
- Use semantic HTML
- Provide descriptive labels
- Announce dynamic changes
- Use proper form labels

---

## 14. Code Documentation

### 14.1 XML Documentation

✅ **DO:**
- Document all public members
- Use `<summary>` for descriptions
- Use `<param>` for parameters
- Use `<returns>` for return values

```csharp
/// <summary>
/// Approves the specified invoice and updates its status.
/// </summary>
/// <param name="id">The invoice identifier.</param>
/// <returns>A task representing the asynchronous operation.</returns>
private async Task ApproveInvoice(DefaultIdType id)
{
    // Implementation
}
```

### 14.2 Code Comments

✅ **DO:**
- Comment complex logic
- Explain "why" not "what"
- Update comments when code changes
- Remove obsolete comments

```csharp
// Calculate total including tax
// Tax rate is applied to subtotal (usage + basic service + demand)
var taxableAmount = UsageCharge + BasicServiceCharge + DemandCharge;
var totalAmount = taxableAmount + (taxableAmount * TaxRate);
```

---

## 15. Testing Guidelines

### 15.1 Component Testing

✅ **DO:**
- Test component rendering
- Test user interactions
- Test validation logic
- Test error scenarios

### 15.2 Integration Testing

✅ **DO:**
- Test API integration
- Test navigation flows
- Test authentication/authorization
- Test data persistence

---

## 16. Common Patterns

### 16.1 Dialog Pattern

```csharp
// In parent component
private async Task ShowDetailsDialog(DefaultIdType id)
{
    var parameters = new DialogParameters<InvoiceDetailsDialog>
    {
        { x => x.InvoiceId, id }
    };
    
    var options = new DialogOptions
    {
        CloseOnEscapeKey = true,
        MaxWidth = MaxWidth.Large,
        FullWidth = true
    };
    
    var dialog = await DialogService.ShowAsync<InvoiceDetailsDialog>(
        "Invoice Details",
        parameters,
        options);
    
    var result = await dialog.Result;
    
    if (!result.Canceled)
    {
        await _table.ReloadDataAsync();
    }
}

// In dialog component
[CascadingParameter]
public IMudDialogInstance MudDialog { get; set; } = null!;

private void Cancel()
{
    MudDialog.Cancel();
}

private void Submit()
{
    MudDialog.Close(DialogResult.Ok(true));
}
```

### 16.2 Autocomplete Pattern

```csharp
public class AutocompleteSupplier : AutocompleteBase<SupplierResponse, IClient, DefaultIdType>
{
    protected override async Task<SupplierResponse?> GetItem(DefaultIdType id)
    {
        return await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.GetSupplierEndpointAsync("1", id));
    }

    protected override async Task<IEnumerable<DefaultIdType>> SearchText(string? value, CancellationToken token)
    {
        var result = await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.SearchSuppliersEndpointAsync("1", new SearchSuppliersCommand
            {
                PageNumber = 1,
                PageSize = 10,
                Keyword = value
            }), token);
        
        if (result?.Items is null) return [];
        
        foreach (var item in result.Items)
        {
            _dictionary[item.Id] = item;
        }
        
        return result.Items.Select(x => x.Id);
    }

    protected override string GetTextValue(DefaultIdType? id)
    {
        return id.HasValue && _dictionary.TryGetValue(id.Value, out var item)
            ? item.Name ?? string.Empty
            : string.Empty;
    }
}
```

### 16.3 Advanced Search Pattern

```razor
<EntityTable @ref="_table" TEntity="InvoiceResponse" TId="DefaultIdType" TRequest="InvoiceViewModel" Context="@Context">
    <AdvancedSearchContent>
        <MudItem xs="12" sm="6" md="4">
            <MudTextField @bind-Value="@InvoiceNumber"
                          Label="Invoice Number"
                          Placeholder="Search by invoice number"
                          Variant="Variant.Outlined" />
        </MudItem>
        <MudItem xs="12" sm="6" md="4">
            <MudSelect T="string"
                       @bind-Value="@Status"
                       Label="Status"
                       Variant="Variant.Outlined"
                       Clearable="true">
                <MudSelectItem Value="@("Draft")">Draft</MudSelectItem>
                <MudSelectItem Value="@("Sent")">Sent</MudSelectItem>
                <MudSelectItem Value="@("Paid")">Paid</MudSelectItem>
            </MudSelect>
        </MudItem>
        <MudItem xs="12" sm="6" md="4">
            <MudDatePicker @bind-Date="@InvoiceDateFrom"
                           Label="Invoice Date From"
                           Variant="Variant.Outlined" />
        </MudItem>
    </AdvancedSearchContent>
</EntityTable>
```

### 16.4 Action Buttons Pattern

```razor
<ActionsContent>
    @* View Details *@
    <MudTooltip Text="View Details">
        <MudIconButton Icon="@Icons.Material.Filled.Visibility"
                       Size="Size.Small"
                       Color="Color.Info"
                       OnClick="@(() => ViewDetails(context.Id))" />
    </MudTooltip>
    
    @* Edit *@
    @if (_canUpdate && CanUpdateEntity(context))
    {
        <MudTooltip Text="Edit">
            <MudIconButton Icon="@Icons.Material.Filled.Edit"
                           Size="Size.Small"
                           Color="Color.Primary"
                           OnClick="@(() => Edit(context.Id))" />
        </MudTooltip>
    }
    
    @* Delete *@
    @if (_canDelete && CanDeleteEntity(context))
    {
        <MudTooltip Text="Delete">
            <MudIconButton Icon="@Icons.Material.Filled.Delete"
                           Size="Size.Small"
                           Color="Color.Error"
                           OnClick="@(() => Delete(context.Id))" />
        </MudTooltip>
    }
    
    @* Workflow actions with conditions *@
    @if (context.Status == "Draft")
    {
        <MudTooltip Text="Approve">
            <MudIconButton Icon="@Icons.Material.Filled.Check"
                           Size="Size.Small"
                           Color="Color.Success"
                           OnClick="@(() => Approve(context.Id))" />
        </MudTooltip>
    }
</ActionsContent>
```

---

## 📋 Checklist for New Pages

When creating a new page, ensure:

- [ ] Follows file organization structure
- [ ] Uses EntityServerTableContext for CRUD pages
- [ ] Has code-behind (.razor.cs) for all logic
- [ ] Uses proper XML documentation
- [ ] Implements search with proper Request object
- [ ] Maps ViewModels to Commands/Requests explicitly
- [ ] Handles errors with try-catch and user feedback
- [ ] Uses appropriate status colors
- [ ] Implements permission checking
- [ ] Has loading states for async operations
- [ ] Uses `.ConfigureAwait(false)` for async calls
- [ ] Has advanced search filters (if applicable)
- [ ] Implements workflow operations (if applicable)
- [ ] Shows success/error Snackbar messages
- [ ] Reloads table after operations
- [ ] Uses MudBlazor components consistently
- [ ] Has proper dialog patterns for details/actions
- [ ] Implements keyboard navigation
- [ ] Has ARIA labels for accessibility
- [ ] Follows naming conventions
- [ ] Groups code sections logically

---

## 🔄 Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | November 10, 2025 | Initial baseline document |

---

## 📚 References

- [MudBlazor Documentation](https://mudblazor.com/)
- [Blazor Documentation](https://docs.microsoft.com/aspnet/core/blazor/)
- [C# Coding Conventions](https://docs.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)

---

## ✅ Enforcement

This document is **MANDATORY** for:
- All new feature development
- All bug fixes involving UI changes
- All code reviews
- All pull requests

**Non-compliance** will result in:
- Code review rejection
- Required refactoring
- Documentation of technical debt

---

**Document Status:** ✅ **OFFICIAL BASELINE**  
**Approval:** Required for all UI development  
**Review Cycle:** Quarterly or as needed for major changes

---

*This is a living document and will be updated as new patterns emerge and best practices evolve.*

