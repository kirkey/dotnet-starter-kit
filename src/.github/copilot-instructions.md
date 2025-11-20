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
- [ ] Map all CRUD operations (Create, Get, List, Update, Delete)
- [ ] Organize in `/v1/` folder structure
- [ ] Use CarterModule for endpoint mapping
- [ ] Group related endpoints (products, brands)
- [ ] Add tags for OpenAPI documentation

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

## ✅ VERIFICATION STATUS

**Catalog Module**: ✅ A+ 100% Compliant  
**Todo Module**: ✅ A+ 100% Compliant  
**Overall**: ✅ A+ 100% - Production Ready

Last verified: November 20, 2025
