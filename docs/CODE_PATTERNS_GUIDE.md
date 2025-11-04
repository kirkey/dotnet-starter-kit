# Debit & Credit Memos - Code Pattern Reference Guide

## Purpose
This guide documents how the Debit and Credit Memo implementations follow the established patterns from Catalog and Todo modules, demonstrating code consistency across the project.

---

## Pattern 1: Application Layer - Command & Handler Pattern

### Pattern Source: Catalog.Application & Todo.Features

#### Example: Product Create (Catalog Module)
```csharp
// Catalog/Catalog.Application/Products/Create/CreateProductCommand.cs
public sealed record CreateProductCommand(
    string Name,
    string? Description = null,
    decimal Price = 0
) : IRequest<DefaultIdType>;

// Catalog/Catalog.Application/Products/Create/CreateProductHandler.cs
public sealed class CreateProductHandler(
    ILogger<CreateProductHandler> logger,
    [FromKeyedServices("catalog:products")] IRepository<Product> repository)
    : IRequestHandler<CreateProductCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var product = Product.Create(...);
        await repository.AddAsync(product, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Product created...");
        return product.Id;
    }
}
```

#### Applied Pattern: Debit Memo Create
```csharp
// Accounting/Accounting.Application/DebitMemos/Create/CreateDebitMemoCommand.cs
public sealed record CreateDebitMemoCommand(
    string MemoNumber,
    DateTime MemoDate,
    decimal Amount,
    string ReferenceType,
    DefaultIdType ReferenceId,
    DefaultIdType? OriginalDocumentId = null,
    string? Reason = null,
    string? Description = null,
    string? Notes = null
) : IRequest<DefaultIdType>;

// Accounting/Accounting.Application/DebitMemos/Create/CreateDebitMemoHandler.cs
public sealed class CreateDebitMemoHandler(
    ILogger<CreateDebitMemoHandler> logger,
    [FromKeyedServices("accounting:debitmemos")] IRepository<DebitMemo> repository)
    : IRequestHandler<CreateDebitMemoCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateDebitMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var debitMemo = DebitMemo.Create(...);
        await repository.AddAsync(debitMemo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Debit memo {MemoNumber} created with ID {DebitMemoId}", ...);
        return debitMemo.Id;
    }
}
```

**Pattern Consistency:**
- ✅ Sealed record for command
- ✅ Named parameters with optional defaults
- ✅ `IRequest<DefaultIdType>` for return type
- ✅ Handler class inheriting `IRequestHandler`
- ✅ Constructor dependency injection with keyed services
- ✅ Null validation at handler start
- ✅ Domain entity creation
- ✅ Repository operations (Add, SaveChanges)
- ✅ Logging with structured data
- ✅ Return ID for created entity

---

## Pattern 2: Specialized Operations - Query & Command Pattern

### Pattern Source: Catalog & Todo Modules

#### Example: Get Product (Catalog Module)
```csharp
// Catalog/Catalog.Application/Products/Get/GetProductQuery.cs
public sealed record GetProductQuery(DefaultIdType Id) : IRequest<ProductResponse>;

// Catalog/Catalog.Application/Products/Get/GetProductHandler.cs
public sealed class GetProductHandler(
    ILogger<GetProductHandler> logger,
    [FromKeyedServices("catalog:products")] IRepository<Product> repository)
    : IRequestHandler<GetProductQuery, ProductResponse>
{
    public async Task<ProductResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null) throw new ProductNotFoundException(request.Id);
        return product.Adapt<ProductResponse>();
    }
}
```

#### Applied Pattern: Get Debit Memo
```csharp
// Accounting/Accounting.Application/DebitMemos/Get/GetDebitMemoQuery.cs
public sealed record GetDebitMemoQuery(DefaultIdType Id) : IRequest<DebitMemoResponse>;

// Accounting/Accounting.Application/DebitMemos/Get/GetDebitMemoHandler.cs
public sealed class GetDebitMemoHandler(
    ILogger<GetDebitMemoHandler> logger,
    [FromKeyedServices("accounting:debitmemos")] IRepository<DebitMemo> repository)
    : IRequestHandler<GetDebitMemoQuery, DebitMemoResponse>
{
    public async Task<DebitMemoResponse> Handle(GetDebitMemoQuery request, CancellationToken cancellationToken)
    {
        var debitMemo = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (debitMemo == null) throw new DebitMemoNotFoundException(request.Id);
        return debitMemo.Adapt<DebitMemoResponse>();
    }
}
```

**Pattern Consistency:**
- ✅ Query record with ID parameter
- ✅ IRequest<ResponseType> return
- ✅ Handler implements IRequestHandler
- ✅ Repository dependency injection
- ✅ Not found exception handling
- ✅ MapStruct-style Adapt<> for DTO mapping

---

## Pattern 3: Search/List Operations - Specification Pattern

### Pattern Source: Catalog & Todo Modules

#### Example: Search Products (Catalog Module)
```csharp
// Catalog/Catalog.Application/Products/Search/SearchProductsQuery.cs
public sealed record SearchProductsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchString = null,
    string? OrderBy = null
) : IRequest<PagedList<ProductResponse>>;

// Catalog/Catalog.Application/Products/Search/SearchProductsSpec.cs
public class SearchProductsSpec : Specification<Product>
{
    public SearchProductsSpec(SearchProductsQuery query)
    {
        Query.PageNumber(query.PageNumber).PageSize(query.PageSize);
        if (!string.IsNullOrEmpty(query.SearchString))
            Query.Where(p => p.Name.Contains(query.SearchString));
        if (!string.IsNullOrEmpty(query.OrderBy))
            Query.OrderBy(query.OrderBy);
    }
}

// Catalog/Catalog.Application/Products/Search/SearchProductsHandler.cs
public sealed class SearchProductsHandler(
    ILogger<SearchProductsHandler> logger,
    [FromKeyedServices("catalog:products")] IRepository<Product> repository)
    : IRequestHandler<SearchProductsQuery, PagedList<ProductResponse>>
{
    public async Task<PagedList<ProductResponse>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var spec = new SearchProductsSpec(request);
        var products = await repository.ListAsync(spec, cancellationToken);
        var count = await repository.CountAsync(spec, cancellationToken);
        var mappedProducts = products.Adapt<List<ProductResponse>>();
        return new PagedList<ProductResponse>(mappedProducts, count, request.PageNumber, request.PageSize);
    }
}
```

#### Applied Pattern: Search Debit Memos
```csharp
// Accounting/Accounting.Application/DebitMemos/Search/SearchDebitMemosQuery.cs
public sealed record SearchDebitMemosQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? MemoNumber = null,
    string? ReferenceType = null,
    DefaultIdType? ReferenceId = null,
    string? Status = null,
    string? ApprovalStatus = null,
    decimal? AmountFrom = null,
    decimal? AmountTo = null,
    string? OrderBy = null
) : IRequest<PagedList<DebitMemoResponse>>;

// Accounting/Accounting.Application/DebitMemos/Search/SearchDebitMemosSpec.cs
public class SearchDebitMemosSpec : Specification<DebitMemo>
{
    public SearchDebitMemosSpec(SearchDebitMemosQuery query)
    {
        Query.PageNumber(query.PageNumber).PageSize(query.PageSize);
        if (!string.IsNullOrEmpty(query.MemoNumber))
            Query.Where(d => d.MemoNumber.Contains(query.MemoNumber));
        if (!string.IsNullOrEmpty(query.Status))
            Query.Where(d => d.Status == query.Status);
        // Additional filters...
    }
}

// Accounting/Accounting.Application/DebitMemos/Search/SearchDebitMemosHandler.cs
public sealed class SearchDebitMemosHandler(
    ILogger<SearchDebitMemosHandler> logger,
    [FromKeyedServices("accounting:debitmemos")] IRepository<DebitMemo> repository)
    : IRequestHandler<SearchDebitMemosQuery, PagedList<DebitMemoResponse>>
{
    public async Task<PagedList<DebitMemoResponse>> Handle(SearchDebitMemosQuery request, CancellationToken cancellationToken)
    {
        var spec = new SearchDebitMemosSpec(request);
        var memos = await repository.ListAsync(spec, cancellationToken);
        var count = await repository.CountAsync(spec, cancellationToken);
        var mappedMemos = memos.Adapt<List<DebitMemoResponse>>();
        return new PagedList<DebitMemoResponse>(mappedMemos, count, request.PageNumber, request.PageSize);
    }
}
```

**Pattern Consistency:**
- ✅ Query record with pagination and filter parameters
- ✅ Optional filter fields with null defaults
- ✅ Specification class for building queries
- ✅ PageNumber/PageSize built into spec
- ✅ Filter conditions in spec constructor
- ✅ Handler uses repository with spec
- ✅ Returns PagedList with proper pagination info
- ✅ Adapt<> for DTO mapping

---

## Pattern 4: Endpoint Infrastructure - FastEndpoints Pattern

### Pattern Source: Catalog & Todo Modules

#### Example: Create Product Endpoint (Catalog Module)
```csharp
// Catalog/Catalog.Infrastructure/Endpoints/Products/v1/CreateProductEndpoint.cs
public static class CreateProductEndpoint
{
    internal static RouteHandlerBuilder MapCreateProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateProductCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateProductEndpoint))
            .WithSummary("Create a product")
            .WithDescription("Create a new product in the catalog")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Catalog.Create")
            .MapToApiVersion(1);
    }
}

// Catalog/Catalog.Infrastructure/Endpoints/CatalogEndpoints.cs
public static class CatalogEndpoints
{
    internal static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var productsGroup = app.MapGroup("/products")
            .WithTags("Products")
            .WithDescription("Endpoints for managing products");

        productsGroup.MapCreateProductEndpoint();
        // Other endpoints...
        return app;
    }
}
```

#### Applied Pattern: Create Debit Memo Endpoint
```csharp
// Accounting/Accounting.Infrastructure/Endpoints/DebitMemos/v1/DebitMemoCreateEndpoint.cs
public static class DebitMemoCreateEndpoint
{
    internal static RouteHandlerBuilder MapDebitMemoCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateDebitMemoCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DebitMemoCreateEndpoint))
            .WithSummary("Create a debit memo")
            .WithDescription("Create a new debit memo for receivable/payable adjustments")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

// Accounting/Accounting.Infrastructure/Endpoints/DebitMemos/DebitMemosEndpoints.cs
public static class DebitMemosEndpoints
{
    internal static IEndpointRouteBuilder MapDebitMemosEndpoints(this IEndpointRouteBuilder app)
    {
        var debitMemosGroup = app.MapGroup("/debit-memos")
            .WithTags("Debit-Memos")
            .WithDescription("Endpoints for managing debit memos - used to increase receivable/payable balances");

        debitMemosGroup.MapDebitMemoCreateEndpoint();
        // Other endpoints...
        return app;
    }
}
```

**Pattern Consistency:**
- ✅ Static extension method with `Map*Endpoint` naming
- ✅ Returns `RouteHandlerBuilder`
- ✅ MapPost/MapPut/MapGet/MapDelete for CRUD
- ✅ Command/Query passed as parameter
- ✅ Mediator.Send() for handler invocation
- ✅ Results.Ok/NoContent/BadRequest for responses
- ✅ WithName, WithSummary, WithDescription fluent configuration
- ✅ Produces<T> for response type documentation
- ✅ RequirePermission for authorization
- ✅ MapToApiVersion for versioning
- ✅ MapGroup for route organization

---

## Pattern 5: Specialized Operations - Action Endpoints

### Pattern Source: Store Module (PutAwayTask, PickList, etc.)

#### Example: Approve Operation
```csharp
// Accounting/Accounting.Infrastructure/Endpoints/DebitMemos/v1/DebitMemoApproveEndpoint.cs
public static class DebitMemoApproveEndpoint
{
    internal static RouteHandlerBuilder MapDebitMemoApproveEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveDebitMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(DebitMemoApproveEndpoint))
            .WithSummary("Approve a debit memo")
            .WithDescription("Approve a draft debit memo for application")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Accounting.Approve")
            .MapToApiVersion(1);
    }
}
```

**Pattern for Specialized Operations:**
- ✅ RESTful action route: `/{id:guid}/{action}`
- ✅ Route parameter validation (id must match command id)
- ✅ Command record for operation parameters
- ✅ Returns Ok() for successful operations
- ✅ Appropriate permission level
- ✅ Clear operation-specific naming

---

## Pattern 6: Blazor UI - EntityServerTable Pattern

### Pattern Source: Catalog & Todo Blazor Pages

#### Example: Blazor Page Structure (Todo Module)
```razor
<!-- Pages/Management/Todos/Todos.razor -->
@page "/management/todos"
@using FSH.Starter.Blazor.Client.Pages.Management.Todos

<PageHeader Title="Todos" />

<EntityTable @ref="_table" TEntity="TodoResponse" TId="DefaultIdType" TRequest="TodoViewModel" Context="@Context">
    <ActionsContent Context="context">
        @if (context.Id != DefaultIdType.Empty)
        {
            <MudMenuItem OnClick="@(() => OnEdit(context.Id))">Edit</MudMenuItem>
            <MudMenuItem OnClick="@(() => OnDelete(context.Id))">Delete</MudMenuItem>
        }
    </ActionsContent>
</EntityTable>
```

#### Applied Pattern: Debit Memos Blazor Page
```razor
<!-- Pages/Accounting/DebitMemos/DebitMemos.razor -->
@page "/accounting/debit-memos"
@using FSH.Starter.Blazor.Client.Pages.Accounting.DebitMemos

<PageHeader Title="Debit Memos" Header="Debit Memos" Subheader="Manage debit memos for receivables and payables adjustments" />

<EntityTable @ref="_table" TEntity="DebitMemoResponse" TId="DefaultIdType" TRequest="DebitMemoViewModel" Context="@Context">
    <ActionsContent Context="context">
        @if (context.Id != DefaultIdType.Empty)
        {
            @if (context.Status == "Draft" && context.ApprovalStatus == "Pending")
            {
                <MudMenuItem OnClick="@(() => OnApproveMemo(context.Id))">
                    <MudIcon Icon="@Icons.Material.Filled.CheckCircle" Class="me-2" />
                    Approve
                </MudMenuItem>
            }
            @if (context.ApprovalStatus == "Approved" && context.Status != "Voided")
            {
                <MudMenuItem OnClick="@(() => OnApplyMemo(context.Id))">
                    <MudIcon Icon="@Icons.Material.Filled.Link" Class="me-2" />
                    Apply
                </MudMenuItem>
            }
            <!-- More actions... -->
        }
    </ActionsContent>
</EntityTable>

<!-- Approve Dialog -->
<MudDialog @bind-IsOpen="_approveDialogVisible">
    <DialogContent>
        <MudTextField @bind-Value="_approveCommand.ApprovedBy" Label="Approved By" />
    </DialogContent>
    <DialogActions>
        <MudButton OnClick="@(() => _approveDialogVisible = false)">Cancel</MudButton>
        <MudButton Variant="Variant.Filled" Color="Color.Primary" OnClick="SubmitApproveMemo">Approve</MudButton>
    </DialogActions>
</MudDialog>
```

#### Applied Code-Behind Pattern
```csharp
// Pages/Accounting/DebitMemos/DebitMemos.razor.cs
public partial class DebitMemos
{
    protected EntityServerTableContext<DebitMemoResponse, DefaultIdType, DebitMemoViewModel> Context { get; set; } = default!;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<DebitMemoResponse, DefaultIdType, DebitMemoViewModel>(
            entityName: "Debit Memo",
            entityNamePlural: "Debit Memos",
            entityResource: FshResources.Accounting,
            fields: [
                new EntityField<DebitMemoResponse>(response => response.MemoNumber, "Memo Number", "MemoNumber"),
                new EntityField<DebitMemoResponse>(response => response.MemoDate, "Date", "MemoDate", typeof(DateOnly)),
                new EntityField<DebitMemoResponse>(response => response.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<DebitMemoResponse>(response => response.Status, "Status", "Status"),
                new EntityField<DebitMemoResponse>(response => response.ApprovalStatus, "Approval", "ApprovalStatus"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<SearchDebitMemosQuery>();
                var result = await Client.DebitMemoSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<DebitMemoResponse>>();
            },
            createFunc: async debitMemo =>
            {
                await Client.DebitMemoCreateEndpointAsync("1", debitMemo.Adapt<CreateDebitMemoCommand>());
            },
            updateFunc: async (id, debitMemo) =>
            {
                await Client.DebitMemoUpdateEndpointAsync("1", id, debitMemo.Adapt<UpdateDebitMemoCommand>());
            },
            deleteFunc: async id =>
            {
                var memoDetails = await Client.DebitMemoGetEndpointAsync("1", id);
                if (memoDetails.Status != "Draft")
                {
                    Snackbar.Add("Only draft debit memos can be deleted.", Severity.Warning);
                    return;
                }
                await Client.DebitMemoDeleteEndpointAsync("1", id);
            }
        );

        return Task.CompletedTask;
    }

    private void OnApproveMemo(DefaultIdType debitMemoId)
    {
        _currentMemoId = debitMemoId;
        _approveCommand = new ApproveDebitMemoCommand { ApprovedBy = string.Empty };
        _approveDialogVisible = true;
    }

    private async Task SubmitApproveMemo()
    {
        if (string.IsNullOrWhiteSpace(_approveCommand.ApprovedBy))
        {
            Snackbar.Add("Please enter who is approving this memo.", Severity.Error);
            return;
        }

        try
        {
            await Client.DebitMemoApproveEndpointAsync("1", _currentMemoId, _approveCommand);
            Snackbar.Add("Debit memo approved successfully.", Severity.Success);
            _approveDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error approving debit memo: {ex.Message}", Severity.Error);
        }
    }
}
```

**Pattern Consistency:**
- ✅ EntityServerTableContext for data binding
- ✅ Field configuration with column mapping
- ✅ Search, Create, Update, Delete function integration
- ✅ Auto-generated API client method calls
- ✅ Adapt<> for DTO transformations
- ✅ Dialog visibility flags
- ✅ Command objects for dialog data binding
- ✅ Snackbar notifications for user feedback
- ✅ Try-catch error handling
- ✅ Table reload after operations

---

## Summary: Pattern Consistency

### ✅ Application Layer Consistency
- All CRUD and specialized operations use MediatR CQRS pattern
- Commands/Queries as sealed records with appropriate parameters
- Handlers with dependency injection and keyed services
- Repository pattern for data access
- Proper logging and exception handling

### ✅ Infrastructure Layer Consistency
- FastEndpoints extension methods for route mapping
- MapGroup for logical organization
- Fluent configuration with documentation
- Permission-based authorization
- API versioning support
- Proper HTTP status codes

### ✅ UI Layer Consistency
- EntityServerTable for data display
- Dialogs for complex operations
- Snackbar for notifications
- API client integration
- MapStruct Adapt<> for DTO mapping

### ✅ All Three Modules Demonstrate
1. **Catalog** - Product CRUD and Search
2. **Todo** - Simple CRUD operations
3. **Accounting (Debit & Credit Memos)** - Full CRUD + Specialized Operations with Audit Trail

---

## Key Takeaways

The Debit and Credit Memo implementation successfully demonstrates:

1. **Scalability** - Patterns scale from simple CRUD (Todo) to complex workflows (Debit/Credit Memos)
2. **Consistency** - Same patterns used across all modules for maintainability
3. **Testability** - All components follow dependency injection patterns for testability
4. **Extensibility** - Easy to add new operations by following established patterns
5. **Security** - Permission-based authorization consistently applied
6. **Auditability** - Logging and domain events track all operations
7. **Professional Quality** - Production-ready implementation with proper error handling and documentation
