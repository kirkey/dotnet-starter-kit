# Blazor Client UI Best Practices - Quick Reference

**Version:** 1.0  
**Last Updated:** November 10, 2025

---

## 🎯 Core Principles

1. **Component-Based Architecture** - Reusable, focused components
2. **Separation of Concerns** - Logic in .razor.cs, markup in .razor
3. **CQRS Alignment** - Commands for writes, Requests for reads
4. **Consistency** - Follow established patterns across all modules

---

## 📁 File Structure

```
/Pages/[Module]/[Feature]/
├── [Feature].razor              # Markup only
├── [Feature].razor.cs           # All logic here
├── [Feature]DetailsDialog.razor
├── [Feature]DetailsDialog.razor.cs
└── Components/
    └── [SubComponent].razor
```

---

## 🔧 EntityTable Pattern (CRUD Pages)

### Code-Behind Structure
```csharp
public partial class Invoices
{
    // 1. EntityTable context and reference
    protected EntityServerTableContext<Response, Id, ViewModel> Context { get; set; } = null!;
    private EntityTable<Response, Id, ViewModel> _table = null!;
    
    // 2. Search filters
    private string? SearchField { get; set; }
    
    // 3. Dialog state
    private bool _dialogVisible;
    private readonly DialogOptions _dialogOptions = new() { ... };
    
    // 4. OnInitializedAsync - Setup Context
    // 5. Workflow methods
    // 6. Helper methods
}
```

### Context Configuration
```csharp
Context = new EntityServerTableContext<Response, Id, ViewModel>(
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
```

---

## 🔍 Search Pattern

```csharp
searchFunc: async filter =>
{
    var request = new SearchRequest
    {
        PageNumber = filter.PageNumber,
        PageSize = filter.PageSize,
        Keyword = filter.Keyword,
        OrderBy = filter.OrderBy,
        // Custom filters
        CustomField = CustomFilterValue
    };
    
    var result = await Client.SearchEndpointAsync("1", request).ConfigureAwait(false);
    return result.Adapt<PaginationResponse<Response>>();
}
```

---

## 💾 CRUD Operations

```csharp
// CREATE
createFunc: async viewModel =>
{
    var command = new CreateCommand { /* map properties */ };
    await Client.CreateEndpointAsync("1", command);
    Snackbar.Add("Created successfully", Severity.Success);
},

// UPDATE
updateFunc: async (id, viewModel) =>
{
    var command = new UpdateCommand { /* map properties */ };
    await Client.UpdateEndpointAsync("1", id, command);
    Snackbar.Add("Updated successfully", Severity.Success);
},

// DELETE
deleteFunc: async id =>
{
    await Client.DeleteEndpointAsync("1", id);
    Snackbar.Add("Deleted successfully", Severity.Success);
}
```

---

## 🔄 Workflow Operations

```csharp
private async Task ApproveEntity(DefaultIdType id)
{
    var confirm = await DialogService.ShowMessageBox(
        "Confirm",
        "Are you sure?",
        yesText: "Yes",
        cancelText: "Cancel");
    
    if (confirm != true) return;

    try
    {
        await Client.ApproveEndpointAsync("1", id);
        Snackbar.Add("Approved successfully", Severity.Success);
        await _table.ReloadDataAsync();
    }
    catch (Exception ex)
    {
        Snackbar.Add($"Error: {ex.Message}", Severity.Error);
    }
}
```

---

## 💬 Dialog Pattern

```csharp
// Show Dialog
private async Task ShowDialog(DefaultIdType id)
{
    var parameters = new DialogParameters<MyDialog>
    {
        { x => x.EntityId, id }
    };
    
    var options = new DialogOptions
    {
        CloseOnEscapeKey = true,
        MaxWidth = MaxWidth.Large,
        FullWidth = true
    };
    
    var dialog = await DialogService.ShowAsync<MyDialog>(
        "Dialog Title",
        parameters,
        options);
    
    var result = await dialog.Result;
    if (!result.Canceled)
    {
        await _table.ReloadDataAsync();
    }
}

// Dialog Component
[CascadingParameter]
public IMudDialogInstance MudDialog { get; set; } = null!;

private void Cancel() => MudDialog.Cancel();
private void Submit() => MudDialog.Close(DialogResult.Ok(true));
```

---

## 🎨 Razor Markup Structure

```razor
@page "/module/feature"
@using Namespace.Components

<PageHeader Title="Title" Header="Header" SubHeader="Description" />

<!-- Action Buttons (Optional) -->
<MudPaper Elevation="2" Class="pa-4 mb-4">
    <MudStack Row="true" Spacing="2">
        <MudButton Color="Color.Primary" OnClick="@CreateNew">
            Create New
        </MudButton>
    </MudStack>
</MudPaper>

<!-- EntityTable -->
<EntityTable @ref="_table" 
             TEntity="Response" 
             TId="DefaultIdType" 
             TRequest="ViewModel" 
             Context="@Context">
    <AdvancedSearchContent>
        <!-- Search filters -->
    </AdvancedSearchContent>
    <ActionsContent>
        <!-- Row actions -->
    </ActionsContent>
</EntityTable>
```

---

## 🎯 Status Color Helper

```csharp
private static Color GetStatusColor(string? status) => status switch
{
    "Draft" => Color.Default,
    "Active" => Color.Info,
    "Approved" => Color.Success,
    "Rejected" => Color.Error,
    "Cancelled" => Color.Warning,
    _ => Color.Default
};
```

---

## ⚠️ Error Handling

```csharp
try
{
    await Client.SomeOperationAsync("1", command);
    Snackbar.Add("Success", Severity.Success);
}
catch (ApiException ex) when (ex.StatusCode == 409)
{
    Snackbar.Add("Conflict: Already exists", Severity.Warning);
}
catch (ApiException ex)
{
    Snackbar.Add($"API Error: {ex.Message}", Severity.Error);
}
catch (Exception ex)
{
    Snackbar.Add($"Error: {ex.Message}", Severity.Error);
}
```

---

## 📝 Documentation Template

```csharp
/// <summary>
/// [Component/method description]
/// </summary>
/// <param name="paramName">Parameter description</param>
/// <returns>Return value description</returns>
public async Task MethodName(Type paramName)
{
    // Implementation
}
```

---

## ✅ Pre-Commit Checklist

- [ ] Code-behind (.razor.cs) has all logic
- [ ] XML documentation on public members
- [ ] Search uses proper Request object
- [ ] Commands/Requests mapped explicitly
- [ ] Error handling with try-catch
- [ ] Success/error Snackbar messages
- [ ] `.ConfigureAwait(false)` on async calls
- [ ] EntityResource for permissions
- [ ] Loading states for async ops
- [ ] Table reload after operations
- [ ] MudBlazor components used
- [ ] Proper dialog patterns
- [ ] Status colors defined
- [ ] Advanced search filters (if needed)

---

## 🚫 Common Mistakes to Avoid

❌ **DON'T:**
- Put logic in .razor files
- Forget `.ConfigureAwait(false)`
- Forget to reload table after operations
- Use hardcoded strings for actions/resources
- Forget error handling
- Forget loading states
- Mix presentation and business logic
- Create monolithic components
- Duplicate code

---

## 📚 Key Components

| Component | Usage |
|-----------|-------|
| `EntityTable` | Standard CRUD pages |
| `EntityServerTableContext` | Configure EntityTable |
| `PageHeader` | Page title and description |
| `MudPaper` | Card/panel container |
| `MudStack` | Layout (Row/Column) |
| `MudGrid/MudItem` | Responsive grid |
| `MudButton` | Action buttons |
| `MudIconButton` | Icon-only buttons |
| `MudTextField` | Text input |
| `MudSelect` | Dropdown |
| `MudDatePicker` | Date selection |
| `MudChip` | Status display |
| `MudDialog` | Modal dialogs |
| `MudTooltip` | Tooltips |

---

## 🔐 Permissions

```csharp
Context = new EntityServerTableContext<...>(
    entityResource: FshResources.Accounting, // Module resource
    searchAction: FshActions.Search,
    createAction: FshActions.Create,
    updateAction: FshActions.Update,
    deleteAction: FshActions.Delete
);
```

---

## 📦 Common Imports

```csharp
// In _Imports.razor or component
@using MudBlazor
@using FSH.Starter.Blazor.Client.Components.EntityTable
@using FSH.Starter.Blazor.Client.Infrastructure.Api
@using FSH.Starter.Blazor.Client.Shared
```

---

## 🎨 MudBlazor Variants

```razor
<!-- Variant Options -->
<MudButton>Text</MudButton>
<MudButton Variant="Variant.Filled">Filled</MudButton>
<MudButton Variant="Variant.Outlined">Outlined</MudButton>

<!-- Color Options -->
<MudButton Color="Color.Primary">Primary</MudButton>
<MudButton Color="Color.Secondary">Secondary</MudButton>
<MudButton Color="Color.Success">Success</MudButton>
<MudButton Color="Color.Error">Error</MudButton>
<MudButton Color="Color.Warning">Warning</MudButton>
<MudButton Color="Color.Info">Info</MudButton>

<!-- Size Options -->
<MudButton Size="Size.Small">Small</MudButton>
<MudButton Size="Size.Medium">Medium</MudButton>
<MudButton Size="Size.Large">Large</MudButton>
```

---

## 🔗 References

- Full Document: `BLAZOR_CLIENT_UI_BEST_PRACTICES.md`
- MudBlazor: https://mudblazor.com/
- Blazor Docs: https://docs.microsoft.com/aspnet/core/blazor/

---

**Status:** ✅ Official Quick Reference  
**Last Updated:** November 10, 2025

