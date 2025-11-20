# 🎨 COPILOT INSTRUCTIONS - UI FOUNDATION

**Last Updated**: November 20, 2025  
**Status**: ✅ Production Ready - Core UI Principles & Patterns  
**Scope**: Foundation for all Blazor UI Development  

> **📌 For module-specific patterns, see:**
> - `copilot-instructions-ui-components.md` - EntityTable, Autocomplete, Dialogs
> - `copilot-instructions-ui-accounting.md` - Accounting module patterns
> - `copilot-instructions-ui-store.md` - Store module patterns
> - `copilot-instructions-ui-hr.md` - HR module patterns

---

## 🎯 CORE UI PRINCIPLES

### 1. **Component-Based Architecture**
- ✅ **Reusable Components**: Build once, use everywhere
- ✅ **Single Responsibility**: Each component does one thing well
- ✅ **Props/Parameters**: Pass data via component parameters, not global state
- ✅ **Composition**: Combine simple components to build complex UIs
- ✅ **Isolation**: Components should not depend on each other directly
- ❌ **No Redundant Injects**: Do NOT add [Inject] attributes for services already included in _Imports.razor

**Example Pattern:**
```csharp
// Bad: Component with multiple responsibilities
<div>
  <h1>@Title</h1>
  <p>@Description</p>
  <button @onclick="Save">Save</button>
  <button @onclick="Delete">Delete</button>
  <!-- ... 200 lines of code -->
</div>

// Good: Separated into focused components
<PageHeader Title="@Title" />
<PageDescription Text="@Description" />
<ActionButtons OnSave="HandleSave" OnDelete="HandleDelete" />
```

### 2. **State Management Best Practices**
- ✅ **Lift State Up**: Move state to parent when needed by siblings
- ✅ **Single Source of Truth**: One place for each piece of state
- ✅ **Local First**: Keep state as local as possible
- ✅ **Immutable Updates**: Don't mutate state directly
- ✅ **Clear Data Flow**: Unidirectional data binding

**State Hierarchy:**
```
Application State (Global/Services)
  ↓
Feature State (Feature Level)
  ↓
Component State (Local/Private)
```

### 3. **Event Handling & Communication**
- ✅ **Callback Functions**: Use EventCallback for parent-child communication
- ✅ **Event Bubbling**: Let events flow up, not down
- ✅ **Named Events**: Clear, descriptive event names (OnItemSelected, OnFormSubmitted)
- ✅ **No Direct Parent Access**: Children don't access parent properties
- ✅ **Parameter Validation**: Validate all input parameters

**Example Pattern:**
```csharp
// Parent Component
<ChildComponent Item="@selectedItem" OnItemChanged="HandleItemChange" />

@code {
    private Item selectedItem;
    
    private async Task HandleItemChange(Item newItem)
    {
        selectedItem = newItem;
        StateHasChanged();
    }
}

// Child Component
@if (Item != null)
{
    <button @onclick="() => OnItemChanged.InvokeAsync(new Item())">
        Change Item
    </button>
}

@code {
    [Parameter]
    public Item Item { get; set; }
    
    [Parameter]
    public EventCallback<Item> OnItemChanged { get; set; }
}
```

### 4. **Performance Optimization**
- ✅ **Lazy Loading**: Load components/data only when needed
- ✅ **Virtual Scrolling**: Use for large lists (100+ items)
- ✅ **Code Splitting**: Break UI into logical chunks
- ✅ **Asset Optimization**: Minimize and compress images, CSS, JS
- ✅ **Change Detection**: Use @key directive for efficient re-rendering

**Performance Patterns:**
```csharp
// Lazy loading example
@if (isLoaded)
{
    <HeavyComponent Data="@data" />
}
else
{
    <LoadingSpinner />
}

@code {
    protected override async Task OnInitializedAsync()
    {
        await Task.Delay(100); // Simulate async loading
        isLoaded = true;
    }
}

// Virtual scrolling for large lists
<Virtualize Items="@largeList">
    <ItemTemplate>
        <ListItem Item="@context" />
    </ItemTemplate>
</Virtualize>
```

### 5. **Accessibility (A11y) Requirements**
- ✅ **Semantic HTML**: Use `<button>`, `<nav>`, `<main>`, not just `<div>`
- ✅ **ARIA Attributes**: Add aria-labels, aria-live for dynamic content
- ✅ **Keyboard Navigation**: All interactions must work with keyboard
- ✅ **Color Contrast**: Minimum 4.5:1 ratio for text
- ✅ **Focus Management**: Clear focus indicators

**Accessibility Checklist:**
```html
<!-- Semantic HTML -->
<nav aria-label="Main navigation">
    <a href="/">Home</a>
    <a href="/about">About</a>
</nav>

<!-- ARIA Labels -->
<button aria-label="Close dialog">×</button>

<!-- Keyboard accessible -->
<input type="text" @onkeypress="HandleEnter" />

<!-- Focus visible -->
<button class="btn btn-primary" tabindex="0">
    Click Me
</button>
```

### 6. **Responsive Design**
- ✅ **Mobile First**: Design for mobile, enhance for desktop
- ✅ **Breakpoints**: Use consistent breakpoints (xs, sm, md, lg, xl)
- ✅ **Flexible Layouts**: Use Flexbox/Grid, not fixed widths
- ✅ **Touch Targets**: Min 48x48px for touch
- ✅ **Viewport Meta**: Set proper viewport for responsive behavior

**Responsive Pattern:**
```css
/* Mobile First */
.container {
    display: flex;
    flex-direction: column;
    gap: 1rem;
}

/* Tablet and up */
@media (min-width: 768px) {
    .container {
        flex-direction: row;
        gap: 2rem;
    }
}

/* Desktop and up */
@media (min-width: 1024px) {
    .container {
        max-width: 1200px;
        margin: 0 auto;
    }
}
```

---

## 📦 IMPORTS & DEPENDENCY INJECTION PATTERN

### **✅ _Imports.razor Pattern**

**CRITICAL RULE**: ❌ **Do NOT add [Inject] attributes for services already included in _Imports.razor**

The _Imports.razor file provides global directives and dependency injection for ALL components in its scope. Adding redundant [Inject] attributes creates:
- ❌ Code duplication
- ❌ Maintenance overhead
- ❌ Confusion about service source
- ❌ Inconsistent patterns

**Common Services in _Imports.razor:**
```csharp
// _Imports.razor typically includes:
@using System.Net.Http
@using System.Net.Http.Json
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.JSInterop
@using MudBlazor

// Common service injections in _Imports:
@inject IDialogService DialogService
@inject ISnackbar Snackbar
@inject NavigationManager NavigationManager
@inject IStringLocalizer<SharedResource> L
@inject HttpClient Http
@inject IClient Client
```

**Pattern to Follow:**

```csharp
// ❌ BAD - Redundant inject for service in _Imports.razor
@page "/my-page"
@inject IDialogService DialogService  // ❌ Already in _Imports.razor
@inject ISnackbar Snackbar            // ❌ Already in _Imports.razor
@inject ICustomService CustomService  // ✅ OK - Not in _Imports.razor

// ✅ GOOD - Only inject services NOT in _Imports.razor
@page "/my-page"
@inject ICustomService CustomService  // ✅ Only this is needed

<h1>My Page</h1>

@code {
    // Both DialogService and Snackbar are available without explicit injection
    private async Task ShowDialog()
    {
        await DialogService.ShowMessageBox("Title", "Message");
    }
    
    private void ShowNotification()
    {
        Snackbar.Add("Notification", Severity.Info);
    }
}
```

**How to Check:**
1. Before adding [Inject], check _Imports.razor in your component's folder hierarchy
2. Look for @inject directives in parent _Imports.razor files
3. Only inject services NOT already provided by _Imports.razor

**Benefits:**
- ✅ Cleaner component code
- ✅ Less duplication
- ✅ Consistent patterns across codebase
- ✅ Easier refactoring when service injection changes

---

## 🎨 CODE PATTERNS - BLAZOR COMPONENTS

### **✅ Component Structure**

**IMPORTANT**: ❌ **Do NOT add [Inject] attributes for services already in _Imports.razor**

```csharp
@page "/page-path"
@implements IAsyncDisposable
// ❌ BAD - Don't inject if already in _Imports.razor
// @inject IDialogService DialogService
// @inject ISnackbar Snackbar

// ✅ GOOD - Only inject services NOT in _Imports.razor
@inject ICustomService CustomService
@using YourNamespace.Components

<!-- Template Section -->
<div class="component-container">
    <h1>@Title</h1>
    @if (isLoading)
    {
        <LoadingSpinner />
    }
    else
    {
        <YourContent />
    }
</div>

<!-- Code Section -->
@code {
    [Parameter]
    public string Title { get; set; } = string.Empty;
    
    [Parameter]
    public EventCallback<ItemModel> OnItemSelected { get; set; }
    
    private bool isLoading;
    private List<ItemModel> items = new();
    
    protected override async Task OnInitializedAsync()
    {
        isLoading = true;
        items = await ServiceInstance.GetItemsAsync();
        isLoading = false;
    }
    
    private async Task HandleItemSelection(ItemModel item)
    {
        await OnItemSelected.InvokeAsync(item);
    }
    
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        // Cleanup resources
    }
}
```

### **✅ Parameter Validation**
```csharp
@code {
    private string _title = string.Empty;
    
    [Parameter]
    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Title cannot be empty");
            _title = value;
        }
    }
    
    [Parameter]
    public int ItemsPerPage
    {
        get => _itemsPerPage;
        set
        {
            if (value < 1 || value > 100)
                throw new ArgumentException("ItemsPerPage must be between 1 and 100");
            _itemsPerPage = value;
        }
    }
}
```

### **✅ Event Callbacks Pattern**
```csharp
// Parent Component
<ChildComponent Items="@items" 
                 OnItemClicked="HandleItemClick"
                 OnItemDeleted="HandleItemDelete"
                 OnRefresh="HandleRefresh" />

@code {
    private async Task HandleItemClick(ItemModel item)
    {
        // Handle click
    }
    
    private async Task HandleItemDelete(int itemId)
    {
        // Handle delete
    }
    
    private async Task HandleRefresh()
    {
        // Handle refresh
    }
}

// Child Component
<div class="item-list">
    @foreach (var item in Items)
    {
        <div class="item" @key="item.Id">
            <span @onclick="() => OnItemClicked.InvokeAsync(item)">
                @item.Name
            </span>
            <button @onclick="() => OnItemDeleted.InvokeAsync(item.Id)">
                Delete
            </button>
        </div>
    }
    <button @onclick="() => OnRefresh.InvokeAsync()">
        Refresh
    </button>
</div>

@code {
    [Parameter]
    public List<ItemModel> Items { get; set; } = new();
    
    [Parameter]
    public EventCallback<ItemModel> OnItemClicked { get; set; }
    
    [Parameter]
    public EventCallback<int> OnItemDeleted { get; set; }
    
    [Parameter]
    public EventCallback OnRefresh { get; set; }
}
```

### **✅ Loading States**
```csharp
@code {
    private enum LoadingState
    {
        Idle,
        Loading,
        Loaded,
        Error
    }
    
    private LoadingState state = LoadingState.Idle;
    private string? errorMessage;
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            state = LoadingState.Loading;
            // Load data
            state = LoadingState.Loaded;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            state = LoadingState.Error;
        }
    }
}

<!-- Template -->
@switch (state)
{
    case LoadingState.Loading:
        <LoadingSpinner />
        break;
    case LoadingState.Loaded:
        <Content />
        break;
    case LoadingState.Error:
        <ErrorAlert Message="@errorMessage" />
        break;
}
```

---

## 📐 STYLING & CSS PATTERNS

### **✅ CSS Organization**
- ✅ **Component Scoped CSS**: Use `.razor.css` for component-specific styles
- ✅ **Global CSS**: Shared styles in global stylesheet
- ✅ **CSS Classes**: Use BEM naming (Block__Element--Modifier)
- ✅ **CSS Variables**: Use for theming and consistency
- ✅ **Utility Classes**: Use framework utilities (Bootstrap, Tailwind)

**Example Structure:**
```
/Components
  /Dashboard
    Dashboard.razor
    Dashboard.razor.css
  /UserProfile
    UserProfile.razor
    UserProfile.razor.css
/Styles
  global.css
  variables.css
  typography.css
```

### **✅ CSS Variables for Theming**
```css
/* variables.css */
:root {
    /* Colors */
    --primary-color: #0066cc;
    --secondary-color: #6c757d;
    --success-color: #28a745;
    --danger-color: #dc3545;
    
    /* Spacing */
    --spacing-xs: 0.25rem;
    --spacing-sm: 0.5rem;
    --spacing-md: 1rem;
    --spacing-lg: 1.5rem;
    --spacing-xl: 2rem;
    
    /* Typography */
    --font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto;
    --font-size-sm: 0.875rem;
    --font-size-md: 1rem;
    --font-size-lg: 1.25rem;
}
```

### **✅ BEM CSS Naming**
```css
/* Block */
.card {
    background: white;
    padding: var(--spacing-md);
    border-radius: 8px;
}

/* Block__Element */
.card__header {
    font-weight: bold;
    margin-bottom: var(--spacing-md);
}

.card__content {
    font-size: var(--font-size-md);
}

/* Block__Element--Modifier */
.card__content--highlighted {
    background: var(--primary-color);
    color: white;
}
```

---

## 🔄 DATA BINDING & FORMS

### **✅ Form Handling**
```csharp
@using System.ComponentModel.DataAnnotations

<EditForm Model="@model" OnValidSubmit="HandleSubmit">
    <DataAnnotationsValidator />
    <ValidationSummary />
    
    <div class="form-group">
        <label for="name">Name:</label>
        <InputText id="name" @bind-Value="model.Name" />
        <ValidationMessage For="@(() => model.Name)" />
    </div>
    
    <div class="form-group">
        <label for="email">Email:</label>
        <InputEmail id="email" @bind-Value="model.Email" />
        <ValidationMessage For="@(() => model.Email)" />
    </div>
    
    <button type="submit" disabled="@isSubmitting">
        @(isSubmitting ? "Submitting..." : "Submit")
    </button>
</EditForm>

@code {
    [Parameter]
    public EventCallback<FormModel> OnSubmit { get; set; }
    
    private FormModel model = new();
    private bool isSubmitting;
    
    private async Task HandleSubmit()
    {
        isSubmitting = true;
        await OnSubmit.InvokeAsync(model);
        isSubmitting = false;
    }
}
```

### **✅ Two-Way Binding**
```csharp
<!-- Bad: Direct manipulation -->
<input @bind="value" />

<!-- Good: Use Events for clarity -->
<input value="@value" @onchange="@((ChangeEventArgs e) => value = e.Value?.ToString())" />

<!-- Best: Use EditForm with validation -->
<EditForm Model="@model">
    <InputText @bind-Value="model.Property" />
</EditForm>
```

---

## 🧩 COMPONENT LIBRARY PATTERNS

### **✅ Creating Reusable Components**
- ✅ **Clear Parameters**: Document all @Parameter properties
- ✅ **Default Values**: Provide sensible defaults
- ✅ **Callbacks**: Use EventCallback for events
- ✅ **Slots (RenderFragment)**: Allow content injection
- ✅ **Composability**: Components work together

**Example Reusable Component:**
```csharp
@* Modal.razor *@
<div class="modal @(IsOpen ? "modal--open" : "")">
    <div class="modal__content">
        <button class="modal__close" @onclick="() => OnClose.InvokeAsync()">
            ×
        </button>
        <div class="modal__header">
            @if (HeaderContent != null)
            {
                @HeaderContent
            }
            else
            {
                <h2>@Title</h2>
            }
        </div>
        <div class="modal__body">
            @ChildContent
        </div>
        <div class="modal__footer">
            @if (FooterContent != null)
            {
                @FooterContent
            }
            else
            {
                <button @onclick="() => OnClose.InvokeAsync()">Close</button>
            }
        </div>
    </div>
</div>

@code {
    [Parameter]
    public bool IsOpen { get; set; }
    
    [Parameter]
    public string? Title { get; set; }
    
    [Parameter]
    public RenderFragment? HeaderContent { get; set; }
    
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public RenderFragment? FooterContent { get; set; }
    
    [Parameter]
    public EventCallback OnClose { get; set; }
}
```

**Usage:**
```csharp
<Modal IsOpen="@isModalOpen" 
        Title="Confirm Action"
        OnClose="HandleModalClose">
    <p>Are you sure?</p>
</Modal>
```

---

## 📱 RESPONSIVE & MOBILE

### **✅ Mobile-First Approach**
```csharp
@if (Device.IsMobile)
{
    <MobileLayout>
        @ChildContent
    </MobileLayout>
}
else
{
    <DesktopLayout>
        @ChildContent
    </DesktopLayout>
}

@code {
    [Inject]
    public IDeviceService Device { get; set; }
}
```

### **✅ Touch-Friendly UI**
```html
<!-- Ensure touch targets are 48x48px minimum -->
<button class="btn btn-lg" style="min-width: 48px; min-height: 48px;">
    Touch Friendly Button
</button>

<!-- Use @ontouchstart, @ontouchend for touch events -->
<div @ontouchstart="HandleTouchStart" @ontouchend="HandleTouchEnd">
    Touch Area
</div>
```

---

## 📚 BEST PRACTICES CHECKLIST

### **Component Development**
- [ ] Component has single responsibility
- [ ] All @Parameter properties documented with XML comments
- [ ] Component accepts data via @Parameter, not direct service calls
- [ ] Events use EventCallback pattern
- [ ] Component implements IAsyncDisposable if needed
- [ ] Loading states handled properly
- [ ] Error handling with user-friendly messages
- [ ] 100% accessible (keyboard, screen reader, ARIA)

### **Styling**
- [ ] Responsive design (mobile-first)
- [ ] CSS scoped to component (.razor.css)
- [ ] BEM naming convention used
- [ ] CSS variables for theming
- [ ] Dark mode support considered
- [ ] Print styles considered

### **Performance**
- [ ] Lazy loading implemented for heavy components
- [ ] Virtual scrolling for long lists (100+)
- [ ] Images optimized and lazy-loaded
- [ ] Code splitting done
- [ ] Change detection (@key) used correctly
- [ ] Memory leaks prevented (IAsyncDisposable)

### **Forms & Validation**
- [ ] EditForm used with validation
- [ ] Validation messages displayed
- [ ] Form submission loading state shown
- [ ] Error messages user-friendly
- [ ] CAPTCHA if needed for security

### **Accessibility**
- [ ] Semantic HTML used
- [ ] ARIA labels where needed
- [ ] Keyboard navigation works
- [ ] Color contrast 4.5:1+
- [ ] Focus visible and managed
- [ ] Screen reader tested

---

## 🎯 IMPLEMENTATION CHECKLIST FOR NEW UI FEATURES

### **Page/Component Creation**
- [ ] Create new component file in appropriate folder
- [ ] Add @page directive if page component
- [ ] Add XML documentation
- [ ] **Check _Imports.razor before adding [Inject] attributes**
- [ ] Only inject services NOT in _Imports.razor
- [ ] Set up parameter validation
- [ ] Handle loading/error states
- [ ] Add accessibility attributes
- [ ] Create .razor.css scoped stylesheet (if needed)
- [ ] Test on mobile and desktop
- [ ] Test with keyboard navigation

### **Styling**
- [ ] Use CSS variables for colors
- [ ] Follow BEM naming convention
- [ ] Mobile-first responsive design
- [ ] Dark mode support
- [ ] Hover/focus states defined
- [ ] Touch-friendly target sizes

### **State Management**
- [ ] State kept as local as possible
- [ ] Single source of truth per data item
- [ ] Immutable updates only
- [ ] Clear data flow (one direction)
- [ ] No circular dependencies

### **Performance**
- [ ] Lazy load if component is heavy
- [ ] Virtual scroll for long lists
- [ ] Images optimized and lazy
- [ ] @key directive used in loops
- [ ] Memory properly released

---

## ✅ VERIFICATION STATUS

**Foundation**: ✅ A+ Production Ready  
**Mobile Considerations**: ✅ A+ Implemented  
**Accessibility**: ✅ A+ Compliant  
**Performance**: ✅ A+ Optimized  

**Last verified**: November 20, 2025

