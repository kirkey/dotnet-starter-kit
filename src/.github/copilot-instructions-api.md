# 🏗️ COPILOT INSTRUCTIONS - COMPREHENSIVE BEST PRACTICES GUIDE

**Last Updated**: November 20, 2025  
**Status**: ✅ Production Ready - Verified with TODO & Catalog Modules  
**Compliance**: 100% - All Best Practices Implemented

---

## 🎯 CORE PRINCIPLES

### 1. **Implement CQRS and DRY Principles**
- ✅ **Commands** for all write operations (Create, Update, Delete)
- ✅ **Requests** for all read operations (Get, Search, List)
- ✅ **Responses** for API contracts (immutable records, never DTOs externally)
- ✅ **Domain Constants** defined once, reused everywhere (no hardcoded values)
- ✅ **Specifications** encapsulate all query logic (reusable, no duplication)

**Examples:**
- `CreateBrandCommand` (write) → `CreateBrandHandler` → `CreateBrandResponse`
- `GetBrandRequest` (read) → `GetBrandHandler` → `BrandResponse`
- `Brand.NameMaxLength = 256` (constant) used in validator, configuration, database

### 2. **Each Class in Separate File**
- ✅ One class per file (Command, Validator, Handler, Response, Request, etc.)
- ✅ Organized by operation: `/Create/v1/`, `/Update/v1/`, `/Delete/v1/`, `/Get/v1/`
- ✅ Specifications in `/Specifications/` folder
- ✅ Follow folder structure: `[Module]/[Entity]/[Operation]/v1/ClassName.cs`

### 3. **Stricter and Tighter Validations**
- ✅ **Multi-layer validation**: Request layer + Domain layer + Database constraints
- ✅ **Repository-based checks** for uniqueness (database queries via specifications)
- ✅ **Domain constant usage** in all validators (no hardcoded limits)
- ✅ **Specification pattern** for reusable validation logic
- ✅ **FluentValidation** with comprehensive rules on all commands/requests

### 4. **Follow Catalog and Todo Projects for Code Consistency**
- ✅ Use existing patterns as templates for all new features
- ✅ Match folder structure, naming conventions, and implementation patterns
- ✅ Maintain consistency across all modules

### 5. **Add XML Documentation**
- ✅ **Classes**: Summary explaining purpose and responsibility
- ✅ **Methods**: Summary of what the method does
- ✅ **Parameters**: Description of each parameter
- ✅ **Return Values**: Explanation of return type
- ✅ **Properties**: Description of field/property purpose
- ✅ 100% coverage on all public members

### 6. **Only Use String as Enums**
- ✅ No numeric enums
- ✅ String comparisons use `StringComparison.OrdinalIgnoreCase`
- ✅ String representations for status/type fields

---

## 📋 BEST PRACTICES RULES (CQRS/DDD/SOLID)

### **Commands for Writes**
- ✅ `CreateBrandCommand` - Create new entity
- ✅ `UpdateBrandCommand` - Update existing entity
- ✅ `DeleteBrandCommand` - Delete entity
- ✅ All write operations use Command pattern
- ✅ All commands have dedicated validators and handlers

### **Requests for Reads**
- ✅ `GetBrandRequest` - Get single entity
- ✅ `SearchBrandsCommand` - Search/filter with pagination
- ✅ `GetTodoListRequest` - List with pagination
- ✅ All read operations use Request pattern
- ✅ Separate handlers for each read operation

### **Response for Output**
- ✅ `CreateBrandResponse` - API contract for creation result
- ✅ `BrandResponse` - API contract for single item retrieval
- ✅ `UpdateBrandResponse` - API contract for update result
- ✅ All responses are immutable records
- ✅ Never expose entities directly to API clients

### **DTO Internal Only**
- ✅ Use DTOs only for internal data transfer (list items, heavy responses)
- ✅ Example: `TodoDto` for list view (lighter than full response)
- ✅ Never expose DTO as API response contract

### **ID in URL, Not Request Body**
- ✅ `GET /api/brands/{id}` - ID from route
- ✅ `PUT /api/brands/{id}` - ID from route, not in body
- ✅ `DELETE /api/brands/{id}` - ID from route, not in body
- ✅ POST operations: ID generated server-side
- ✅ Search/List: Use query parameters, not body

### **Property-based Parameters**
- ✅ Use property-based command/request construction
- ✅ NOT positional parameters (for NSwag compatibility)

---

## 🎨 CODE PATTERNS IMPLEMENTED

### **✅ Primary Constructor Syntax**
- Use modern C# primary constructor patterns (C# 12+)
- Automatic parameter capture (no field assignments needed)
- Applied to all handlers and services

### **✅ Keyed Services - Proper DI Isolation**
- Use `[FromKeyedServices("key")]` attribute for dependency resolution
- Prevents cross-module contamination
- Each module has unique keys: "catalog:brands", "catalog:products", "todo"

### **✅ SaveChangesAsync - Transaction Handling**
- All handlers use `SaveChangesAsync()` for persistence
- Ensures transactional consistency
- Called after all business logic completes

### **✅ ConfigureAwait(false) on All Awaits**
- Apply to ALL async calls: `.ConfigureAwait(false)`
- Prevents unnecessary context switching
- Safe for all execution contexts (UI, console, library)

### **✅ Specification Pattern - Query Encapsulation**
- All query logic in specification classes (reusable, testable)
- Located in `/Specifications/` folder
- Used in validators for uniqueness checks
- Used in handlers for data retrieval

**Specifications Created:**
- `BrandByNameSpec` - Find brand by name (case-insensitive)
- `ProductByNameSpec` - Find product by name (case-insensitive)
- `ProductsByBrandSpec` - Find all products for a brand
- `TodoByNameSpec` - Find todo by name (case-insensitive)

### **✅ Pagination - Repository Layer Handling**
- Pagination handled in repository layer, NOT in specifications
- Full support: page number, page size, sorting, filtering
- Generic `PaginationFilter` base class for all list requests
- Returns `PagedList<T>` with total count

### **✅ Domain Events - Event-Driven Architecture**
- Entities raise domain events on state changes
- Immutable, sealed event classes
- Events raised only when actual changes detected
- Event handlers in `/Events/` folder

### **✅ Error Handling - Custom Exceptions**
- Create custom exception classes for domain-specific errors
- Clear error messages with context
- Proper exception propagation

### **✅ Versioning - Clean API Structure**
- All endpoints organized in `/v1/` folders
- Ready for future versioning (`/v2/`, etc.)
- API versioning via `.MapToApiVersion(1)`

### **✅ Response Pattern - Consistent API Contracts**
- Create response classes for each operation
- Immutable records for all responses
- Clear naming: `CreateXyzResponse`, `XyzResponse`, `UpdateXyzResponse`

### **✅ ICarterModule Pattern - STANDARD FOR ALL NEW ENDPOINTS**

**STATUS: ✅ ALL MODULES CONVERTED** (Catalog & Todo remain with extension methods, all others use ICarterModule)

#### **Core Rules - ALWAYS Follow These**
- **MANDATORY**: All new endpoints MUST implement `ICarterModule` interface
- Endpoints are auto-discovered by Carter - NO manual registration needed
- Each module typically has 1-3 endpoint classes per logical grouping
- Each endpoint class maps all operations for a resource via `AddRoutes(IEndpointRouteBuilder app)`
- Use helper extension methods from individual endpoint handler files (e.g., `MapCreateItemEndpoint()`)
- No explicit registration in Program.cs or Module files required

#### **Example 1: Single Entity with Multiple Operations (Store Module)**
```csharp
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Store.Infrastructure.Endpoints.Items;

/// <summary>
/// Endpoint configuration for Items module.
/// </summary>
public class ItemsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/items").WithTags("items");

        // Create operation
        group.MapPost("/", async (CreateItemCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/store/items/{response.Id}", response);
            })
            .WithName("CreateItem")
            .WithSummary("Create a new item")
            .Produces<CreateItemResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
            .MapToApiVersion(1);

        // Get operation
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetItemCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetItem")
            .WithSummary("Get item by ID")
            .Produces<ItemResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);

        // Search operation
        group.MapPost("/search", async (SearchItemsCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchItems")
            .WithSummary("Search items")
            .Produces<PagedList<ItemResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);

        // Update operation
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateItemCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateItem")
            .WithSummary("Update item")
            .Produces<UpdateItemResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);

        // Delete operation
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteItemCommand { Id = id }).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteItem")
            .WithSummary("Delete item")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
            .MapToApiVersion(1);
    }
}
```

#### **Example 2: Multiple Related Resources (Messaging Module)**
When a module has multiple related resources, create separate endpoint classes per resource:

```csharp
// ConversationsEndpoints.cs
namespace FSH.Starter.WebApi.Messaging.Features.Conversations;

public class ConversationsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("conversations").WithTags("conversations");
        
        // Create conversation
        group.MapCreateConversationEndpoint();
        
        // Get conversation
        group.MapGetConversationEndpoint();
        
        // Get conversations list
        group.MapGetConversationListEndpoint();
        
        // Add member
        group.MapAddMemberEndpoint();
        
        // Remove member
        group.MapRemoveMemberEndpoint();
    }
}

// MessagesEndpoints.cs
namespace FSH.Starter.WebApi.Messaging.Features.Messages;

public class MessagesEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("messages").WithTags("messages");
        
        group.MapCreateMessageEndpoint();
        group.MapGetMessageListEndpoint();
        group.MapUpdateMessageEndpoint();
        group.MapDeleteMessageEndpoint();
    }
}
```

**Key Benefits:**
- ✅ Auto-discovery: No manual registration required
- ✅ Clean Architecture: One class per resource type
- ✅ Maintainable: Easy to locate and modify endpoints
- ✅ Scalable: Simple to add new resources or operations
- ✅ Consistent: Same pattern across ALL modules
- ✅ Testable: Each endpoint class is isolated and testable

**Implementation Rules:**
- **Class Naming**: `{Resource}Endpoints` (e.g., `ItemsEndpoints`, `ConversationsEndpoints`, `EmployeesEndpoints`)
- **File Location**: `{Module}/Features/{Resource}/{Resource}Endpoints.cs` OR `{Module}.Infrastructure/Endpoints/{Resource}/{Resource}Endpoints.cs`
- **Interface**: Implement `ICarterModule` (from Carter library)
- **Method**: Implement `void AddRoutes(IEndpointRouteBuilder app)`
- **Routing**: Use `MapGroup("{module}/{resource}")` with module prefix
- **Tags**: Use `.WithTags()` for OpenAPI grouping
- **Helpers**: Call helper extension methods from individual endpoint files (e.g., `MapCreateItemEndpoint()`)
- **Documentation**: Include XML documentation on class
- **Operations**: All CRUD operations inline (Create, Read, Update, Delete, Search)
- **MediatR**: Use `ISender` for all command/query dispatch
- **Async**: Apply `.ConfigureAwait(false)` on all awaits
- **Status Codes**: Include `.Produces<>()` with proper HTTP status
- **Auth**: Add `.RequirePermission()` checks
- **Versioning**: Add `.MapToApiVersion(1)` to all endpoints

---

## 🗄️ DATABASE CONFIGURATION BEST PRACTICES

### **✅ Domain Constant Usage**
- All MaxLength from domain constants
- No hardcoded database limits
- Single source of truth

### **✅ Decimal Precision**
- Use `HasPrecision(18, 2)` for financial fields
- Supports range: 0.01 to 999,999,999.99
- Example: Product.Price

### **✅ Performance Indexes**
- Create indexes on frequently queried fields
- Index on search fields (Name, BrandId)
- Index on filter fields (Price, CreatedOn)
- Proper naming: `IX_TableName_ColumnName`

### **✅ Multi-Tenancy Support**
- Call `builder.IsMultiTenant()` on all entities
- Automatic tenant isolation
- All queries automatically scoped by tenant

### **✅ Relationships & Referential Integrity**
- Configure foreign key relationships
- Set delete behavior (Cascade, Restrict, SetNull, etc.)
- Use Restrict to prevent orphaned records

### **❌ NEVER Add builder.HasCheckConstraint**
- Do NOT use `HasCheckConstraint` in configurations
- Use domain validation instead
- Use property constraints (MaxLength, Precision)
- Use specifications for complex business rules

---

## 🎯 IMPLEMENTATION CHECKLIST FOR NEW FEATURES

### **Domain Layer**
- [ ] Create entity with factory method (private constructor)
- [ ] Define domain constants (binary limits: 2^8, 2^11, 2^12)
- [ ] Add input validation in factory method
- [ ] Add XML documentation (class, properties, methods)
- [ ] Add domain events for state changes
- [ ] Implement change detection logic

### **Application Layer - Commands**
- [ ] Create Command class (inherits IRequest<Response>)
- [ ] Create CommandValidator (FluentValidation)
- [ ] Create CommandHandler (implements IRequestHandler)
- [ ] Inject repository via [FromKeyedServices]
- [ ] Use ConfigureAwait(false) on all awaits
- [ ] Call SaveChangesAsync() for persistence
- [ ] Add XML documentation on all classes

### **Application Layer - Requests (Reads)**
- [ ] Create Request class (inherits IRequest<Response>)
- [ ] Create RequestHandler
- [ ] Inject repository via [FromKeyedServices]
- [ ] Use ConfigureAwait(false) on all awaits
- [ ] Return Response object (not entity)
- [ ] Add XML documentation

### **Specifications**
- [ ] Create specification if query logic is reusable
- [ ] Encapsulate where clauses
- [ ] Use StringComparison.OrdinalIgnoreCase for strings
- [ ] Located in /Specifications/ folder

### **Validators**
- [ ] Use domain constants (no hardcoded values)
- [ ] Include repository-based uniqueness checks
- [ ] Use specifications for query logic
- [ ] Apply multi-layer validation
- [ ] Provide clear error messages

### **Endpoints**
- [ ] Create endpoint class implementing ICarterModule
- [ ] Implement AddRoutes(IEndpointRouteBuilder app) method
- [ ] Use MapGroup() with module prefix (e.g., "store/items")
- [ ] Add .WithTags() for OpenAPI grouping
- [ ] Map all CRUD operations inline (Create, Get, Update, Delete, Search)
- [ ] Use ISender (MediatR) for all command/query dispatch
- [ ] Apply .ConfigureAwait(false) on all async calls
- [ ] Include .WithName() for each endpoint
- [ ] Add .WithSummary() and .WithDescription() for documentation
- [ ] Set proper HTTP status codes with .Produces<>()
- [ ] Apply .RequirePermission() for authorization
- [ ] Add XML documentation on endpoint class

### **Configuration**
- [ ] Add MaxLength constraints from domain constants
- [ ] Add indexes on frequently queried fields
- [ ] Configure relationships with proper delete behavior
- [ ] Call IsMultiTenant() for tenant isolation
- [ ] Do NOT add HasCheckConstraint

### **Responses/DTOs**
- [ ] Create immutable record responses
- [ ] Use naming: CreateXyzResponse, XyzResponse, UpdateXyzResponse
- [ ] Add XML documentation on all properties
- [ ] Use nullable annotations appropriately

---

---

## ✅ MODULE IMPLEMENTATION STATUS - ALL ENDPOINTS

### **ICarterModule Implementation (AUTO-DISCOVERED)**
| Module | Status | Endpoint Classes | Auto-Discovery |
|--------|--------|------------------|-----------------|
| **MicroFinance** | ✅ Complete | All entities | ✅ Yes |
| **Store** | ✅ Complete | 20 entities (BinsEndpoints, CategoriesEndpoints, ItemsEndpoints, etc.) | ✅ Yes |
| **HR** | ✅ Complete | 37+ entities (EmployeesEndpoints, AttendanceEndpoints, etc.) | ✅ Yes |
| **Accounting** | ✅ Complete | 45 entities (ChartOfAccountsEndpoints, InvoicesEndpoints, etc.) | ✅ Yes |
| **Messaging** | ✅ Complete | ConversationsEndpoints, MessagesEndpoints, MessagingUtilityEndpoints | ✅ Yes |

### **Extension Method Pattern (Legacy - Maintained for Compatibility)**
| Module | Status | Notes |
|--------|--------|-------|
| **Catalog** | ✅ Maintained | Individual endpoint extension methods + CatalogModule.Endpoints |
| **Todo** | ✅ Maintained | Individual endpoint extension methods + TodoModule.Endpoints |

### **Registration Pattern**
- **ICarterModule modules**: AUTO-DISCOVERED by Carter (no registration needed)
- **Legacy modules** (Catalog, Todo): Explicitly registered via `config.WithModule<CatalogModule.Endpoints>()` and `config.WithModule<TodoModule.Endpoints>()`
- **Program.cs**: Single `endpoints.MapCarter()` call handles all auto-discovery

**Overall Status**: ✅ **PRODUCTION READY - 100% COMPLIANT**

Last verified: December 4, 2025
