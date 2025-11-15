# ✅ Designation Domain - Complete Implementation

**Date:** November 15, 2025  
**Status:** ✅ COMPLETE - Following Todo and Catalog Patterns

---

## 🎯 Overview

The **Designation** domain represents job positions/designations within specific organizational units (areas/departments). This implementation follows the **CQRS pattern** and **Todo/Catalog consistency patterns**.

### Key Design Features:
- ✅ Area-Specific Positions (Same title, different rates per area)
- ✅ Full CRUD Operations (Create, Read, Update, Delete)
- ✅ Search with Pagination and Filters
- ✅ Domain Events (Created, Updated, Activated, Deactivated)
- ✅ Fluent Validators
- ✅ Specifications Pattern
- ✅ Repository Pattern with Keyed Services
- ✅ RESTful Endpoints with Permissions
- ✅ Multi-Tenant Support

---

## 📂 Complete File Structure

```
HumanResources.Domain/
├── Entities/
│   └── Designation.cs                     ✅ Domain entity
├── Events/
│   └── DesignationEvents.cs              ✅ Domain events
└── Exceptions/
    └── DesignationExceptions.cs          ✅ Domain exceptions

HumanResources.Application/
└── Designations/
    ├── Create/v1/
    │   ├── CreateDesignationCommand.cs    ✅ CQRS Command
    │   ├── CreateDesignationHandler.cs    ✅ Command handler
    │   ├── CreateDesignationValidator.cs  ✅ Fluent validator
    │   └── CreateDesignationResponse.cs   ✅ Response (ID only)
    ├── Get/v1/
    │   ├── GetDesignationRequest.cs       ✅ Query request
    │   ├── GetDesignationHandler.cs       ✅ Query handler
    │   └── DesignationResponse.cs         ✅ Full response DTO
    ├── Update/v1/
    │   ├── UpdateDesignationCommand.cs    ✅ CQRS Command
    │   ├── UpdateDesignationHandler.cs    ✅ Command handler
    │   ├── UpdateDesignationValidator.cs  ✅ Fluent validator
    │   └── UpdateDesignationResponse.cs   ✅ Response (ID only)
    ├── Delete/v1/
    │   ├── DeleteDesignationCommand.cs    ✅ CQRS Command
    │   ├── DeleteDesignationHandler.cs    ✅ Command handler
    │   └── DeleteDesignationResponse.cs   ✅ Response (ID only)
    ├── Search/v1/
    │   ├── SearchDesignationsRequest.cs   ✅ Search request (paginated)
    │   └── SearchDesignationsHandler.cs   ✅ Search handler
    └── Specifications/
        ├── DesignationByIdSpec.cs         ✅ Get by ID
        ├── DesignationByCodeAndOrgUnitSpec.cs ✅ Duplicate check
        └── SearchDesignationsSpec.cs      ✅ Search with filters

HumanResources.Infrastructure/
├── Persistence/
│   └── Configurations/
│       └── DesignationConfiguration.cs    ✅ EF Core config
├── Endpoints/
│   └── Designations/
│       ├── DesignationsEndpoints.cs       ✅ Endpoint router
│       └── v1/
│           ├── CreateDesignationEndpoint.cs    ✅ POST /
│           ├── GetDesignationEndpoint.cs       ✅ GET /{id}
│           ├── UpdateDesignationEndpoint.cs    ✅ PUT /{id}
│           ├── DeleteDesignationEndpoint.cs    ✅ DELETE /{id}
│           └── SearchDesignationsEndpoint.cs   ✅ POST /search
└── HumanResourcesModule.cs               ✅ DI registration
```

---

## 🏗️ Domain Entity: Designation

### Structure
```csharp
public class Designation : AuditableEntity, IAggregateRoot
{
    // Unique designation code per organizational unit
    public string Code { get; private set; }
    
    // Job title (can be same across multiple areas)
    public string Title { get; private set; }
    
    // Description (can vary per area)
    public string? Description { get; private set; }
    
    // Reference to organizational unit (area/department)
    public DefaultIdType OrganizationalUnitId { get; private set; }
    public OrganizationalUnit OrganizationalUnit { get; private set; }
    
    // Salary range for this designation in this area
    public decimal? MinSalary { get; private set; }
    public decimal? MaxSalary { get; private set; }
    
    // Activation status
    public bool IsActive { get; private set; }
    
    // Factory method
    public static Designation Create(
        string code,
        string title,
        DefaultIdType organizationalUnitId,
        string? description = null,
        decimal? minSalary = null,
        decimal? maxSalary = null)
    
    // Update method
    public Designation Update(
        string? title,
        string? description,
        decimal? minSalary,
        decimal? maxSalary)
    
    // Activation methods
    public Designation Activate()
    public Designation Deactivate()
}
```

### Key Constraint
```sql
UNIQUE (TenantId, OrganizationalUnitId, Code)
```
This allows the **SAME code in different areas**, enabling area-specific positions.

---

## 🔄 Complete CQRS Operations

### 1️⃣ CREATE: CreateDesignationCommand

**Request:**
```csharp
public sealed record CreateDesignationCommand(
    DefaultIdType OrganizationalUnitId,
    string Code,              // "SUP-001", "TECH-001"
    string Title,             // "Supervisor", "Technician"
    string? Description,      // Optional area-specific description
    decimal? MinSalary,       // e.g., 40000
    decimal? MaxSalary) : IRequest<CreateDesignationResponse>;
```

**Response:**
```csharp
public sealed record CreateDesignationResponse(DefaultIdType? Id);
```

**Validation:**
```
✓ OrganizationalUnitId is required
✓ Code: required, 50 chars max, uppercase/numbers/hyphens only
✓ Title: required, 256 chars max
✓ Description: optional, 2000 chars max
✓ MinSalary: >= 0 if provided
✓ MaxSalary: >= 0 and >= MinSalary if provided
✓ Code must be unique per OrganizationalUnit (per tenant)
```

**Endpoint:**
```
POST /api/v1/designations
Headers: Authorization, Accept: application/json
Permission: Permissions.Designations.Create
```

**Example Request:**
```json
{
  "organizationalUnitId": "550e8400-e29b-41d4-a716-446655440000",
  "code": "SUP-001",
  "title": "Area Supervisor",
  "description": "Supervises field operations in Area 1",
  "minSalary": 40000,
  "maxSalary": 55000
}
```

**Example Response:**
```json
{
  "id": "110e8400-e29b-41d4-a716-446655440000"
}
```

---

### 2️⃣ READ: GetDesignationRequest

**Request:**
```csharp
public sealed record GetDesignationRequest(DefaultIdType Id) : IRequest<DesignationResponse>;
```

**Response:**
```csharp
public sealed record DesignationResponse
{
    public DefaultIdType Id { get; init; }
    public string Code { get; init; }
    public string Title { get; init; }
    public DefaultIdType OrganizationalUnitId { get; init; }
    public string? OrganizationalUnitName { get; init; }
    public string? Description { get; init; }
    public decimal? MinSalary { get; init; }
    public decimal? MaxSalary { get; init; }
    public bool IsActive { get; init; }
}
```

**Endpoint:**
```
GET /api/v1/designations/{id}
Headers: Authorization, Accept: application/json
Permission: Permissions.Designations.View
```

**Example Response:**
```json
{
  "id": "110e8400-e29b-41d4-a716-446655440000",
  "code": "SUP-001",
  "title": "Area Supervisor",
  "organizationalUnitId": "550e8400-e29b-41d4-a716-446655440000",
  "organizationalUnitName": "Area 1",
  "description": "Supervises field operations in Area 1",
  "minSalary": 40000,
  "maxSalary": 55000,
  "isActive": true
}
```

---

### 3️⃣ UPDATE: UpdateDesignationCommand

**Request:**
```csharp
public sealed record UpdateDesignationCommand(
    DefaultIdType Id,
    string Title,
    string? Description,
    decimal? MinSalary,
    decimal? MaxSalary) : IRequest<UpdateDesignationResponse>;
```

**Response:**
```csharp
public sealed record UpdateDesignationResponse(DefaultIdType Id);
```

**Validation:**
```
✓ Id is required
✓ Title: required, 256 chars max
✓ Description: optional, 2000 chars max
✓ MinSalary: >= 0 if provided
✓ MaxSalary: >= 0 and >= MinSalary if provided
```

**Endpoint:**
```
PUT /api/v1/designations/{id}
Headers: Authorization, Accept: application/json
Permission: Permissions.Designations.Update
```

**Example Request:**
```json
{
  "title": "Senior Area Supervisor",
  "description": "Senior supervisor for Area 1 operations",
  "minSalary": 42000,
  "maxSalary": 58000
}
```

**Example Response:**
```json
{
  "id": "110e8400-e29b-41d4-a716-446655440000"
}
```

---

### 4️⃣ DELETE: DeleteDesignationCommand

**Request:**
```csharp
public sealed record DeleteDesignationCommand(DefaultIdType Id) : IRequest<DeleteDesignationResponse>;
```

**Response:**
```csharp
public sealed record DeleteDesignationResponse(DefaultIdType Id);
```

**Endpoint:**
```
DELETE /api/v1/designations/{id}
Headers: Authorization, Accept: application/json
Permission: Permissions.Designations.Delete
```

**Example Response:**
```json
{
  "id": "110e8400-e29b-41d4-a716-446655440000"
}
```

---

### 5️⃣ SEARCH: SearchDesignationsRequest

**Request:**
```csharp
public class SearchDesignationsRequest : PaginationFilter, IRequest<PagedList<DesignationResponse>>
{
    public DefaultIdType? OrganizationalUnitId { get; set; }  // Filter by area
    public string? Title { get; set; }                         // Filter by title
    public bool? IsActive { get; set; }                        // Filter by status
    public decimal? SalaryMin { get; set; }                    // Filter by min salary
    public decimal? SalaryMax { get; set; }                    // Filter by max salary
    // Inherited: PageNumber, PageSize, OrderBy
}
```

**Response:**
```csharp
public class PagedList<DesignationResponse>
{
    public List<DesignationResponse> Data { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}
```

**Endpoint:**
```
POST /api/v1/designations/search
Headers: Authorization, Accept: application/json
Permission: Permissions.Designations.View
```

**Example Request:**
```json
{
  "organizationalUnitId": "550e8400-e29b-41d4-a716-446655440000",
  "title": "Supervisor",
  "isActive": true,
  "salaryMin": 40000,
  "salaryMax": 60000,
  "pageNumber": 1,
  "pageSize": 10
}
```

**Example Response:**
```json
{
  "data": [
    {
      "id": "110e8400-e29b-41d4-a716-446655440000",
      "code": "SUP-001",
      "title": "Area Supervisor",
      "organizationalUnitId": "550e8400-e29b-41d4-a716-446655440000",
      "organizationalUnitName": "Area 1",
      "description": "Supervises field operations in Area 1",
      "minSalary": 40000,
      "maxSalary": 55000,
      "isActive": true
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "hasNextPage": false,
  "hasPreviousPage": false
}
```

---

## 🔌 Dependency Injection

### Repository Registration
```csharp
// In HumanResourcesModule.cs
builder.Services.AddKeyedScoped<IRepository<Designation>, HumanResourcesRepository<Designation>>("hr:designations");
builder.Services.AddKeyedScoped<IReadRepository<Designation>, HumanResourcesRepository<Designation>>("hr:designations");
```

### Handler Injection
```csharp
// In Create Handler
public sealed class CreateDesignationHandler(
    ILogger<CreateDesignationHandler> logger,
    [FromKeyedServices("hr:designations")] IRepository<Designation> repository,
    [FromKeyedServices("hr:designations")] IReadRepository<Designation> readRepository)
    : IRequestHandler<CreateDesignationCommand, CreateDesignationResponse>
```

---

## 📊 Database Schema

### Table: Positions (for Designations)
```sql
CREATE TABLE [hr].[Positions] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [TenantId] UNIQUEIDENTIFIER NOT NULL,
    [Code] NVARCHAR(50) NOT NULL,
    [Title] NVARCHAR(256) NOT NULL,
    [OrganizationalUnitId] UNIQUEIDENTIFIER NOT NULL,
    [Description] NVARCHAR(2000) NULL,
    [MinSalary] DECIMAL(16,2) NULL,
    [MaxSalary] DECIMAL(16,2) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedBy] NVARCHAR(256) NULL,
    [CreatedOn] DATETIMEOFFSET NOT NULL,
    [LastModifiedBy] NVARCHAR(256) NULL,
    [LastModifiedOn] DATETIMEOFFSET NULL,
    CONSTRAINT FK_Positions_OrganizationalUnits 
        FOREIGN KEY ([OrganizationalUnitId]) 
        REFERENCES [hr].[OrganizationalUnits]([Id]) 
        ON DELETE RESTRICT,
    CONSTRAINT UQ_Positions_CodePerOrgUnit 
        UNIQUE ([TenantId], [OrganizationalUnitId], [Code])
);

CREATE INDEX IX_Positions_IsActive ON [hr].[Positions]([IsActive]);
CREATE INDEX IX_Positions_OrgUnitCode ON [hr].[Positions]([OrganizationalUnitId], [Code]);
CREATE INDEX IX_Positions_OrganizationalUnitId ON [hr].[Positions]([OrganizationalUnitId]);
```

---

## 🎯 Example Scenarios

### Scenario 1: Create Area-Specific Supervisors

**Area 1 - Supervisor Position:**
```csharp
var area1Supervisor = CreateDesignationCommand(
    organizationalUnitId: area1Id,
    code: "SUP-001",
    title: "Area Supervisor",
    description: "Supervises Area 1 operations",
    minSalary: 40000,
    maxSalary: 55000
);
```

**Area 2 - Same Title, Different Position:**
```csharp
var area2Supervisor = CreateDesignationCommand(
    organizationalUnitId: area2Id,
    code: "SUP-001",  // ✅ SAME code!
    title: "Area Supervisor",  // ✅ SAME title!
    description: "Supervises Area 2 operations",
    minSalary: 42000,  // ✅ DIFFERENT salary!
    maxSalary: 58000
);
```

**Query Result:**
```
Area 1: SUP-001, Area Supervisor, $40K-$55K
Area 2: SUP-001, Area Supervisor, $42K-$58K
← Two different Designation records, same code!
```

---

### Scenario 2: Search All Supervisors Across All Areas

**Search Request:**
```json
{
  "title": "Supervisor",
  "isActive": true,
  "pageNumber": 1,
  "pageSize": 50
}
```

**Search Result:**
```
✓ Supervisor (Area 1) - $40K-$55K
✓ Supervisor (Area 2) - $42K-$58K
✓ Supervisor (Area 3) - $41K-$56K
```

---

### Scenario 3: Update Area-Specific Salary

**Update Request:**
```json
{
  "id": "area1-supervisor-id",
  "title": "Area Supervisor",
  "minSalary": 41000,
  "maxSalary": 57000
}
```

**Result:**
```
Area 1 Supervisor salary updated: $40K-$55K → $41K-$57K
```

---

## ✅ Design Patterns Applied

| Pattern | Implementation |
|---------|----------------|
| **CQRS** | Separate commands and queries |
| **Domain Events** | DesignationCreated, Updated, Activated, Deactivated |
| **Specification** | DesignationByIdSpec, SearchDesignationsSpec |
| **Repository** | Generic repository with keyed services |
| **Fluent Validation** | CreateDesignationValidator, UpdateDesignationValidator |
| **Multi-Tenancy** | Via builder.IsMultiTenant() and TenantId |
| **RESTful** | POST, GET, PUT, DELETE with proper HTTP status codes |
| **Permissions** | Role-based access control per operation |
| **Factory Method** | Designation.Create() for aggregate construction |
| **Value Objects** | Code, Title validation |
| **Soft Delete** | IsActive flag for logical deletion |
| **Pagination** | PagedList with HasNextPage/HasPreviousPage |
| **Audit Trail** | CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn |

---

## 🧪 Testing the API

### Create Designation
```bash
curl -X POST http://localhost:5000/api/v1/designations \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "organizationalUnitId": "550e8400-e29b-41d4-a716-446655440000",
    "code": "SUP-001",
    "title": "Area Supervisor",
    "minSalary": 40000,
    "maxSalary": 55000
  }'
```

### Get Designation
```bash
curl -X GET http://localhost:5000/api/v1/designations/110e8400-e29b-41d4-a716-446655440000 \
  -H "Authorization: Bearer $TOKEN"
```

### Search Designations
```bash
curl -X POST http://localhost:5000/api/v1/designations/search \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "organizationalUnitId": "550e8400-e29b-41d4-a716-446655440000",
    "isActive": true,
    "pageNumber": 1,
    "pageSize": 10
  }'
```

### Update Designation
```bash
curl -X PUT http://localhost:5000/api/v1/designations/110e8400-e29b-41d4-a716-446655440000 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Senior Area Supervisor",
    "minSalary": 42000,
    "maxSalary": 58000
  }'
```

### Delete Designation
```bash
curl -X DELETE http://localhost:5000/api/v1/designations/110e8400-e29b-41d4-a716-446655440000 \
  -H "Authorization: Bearer $TOKEN"
```

---

## ✅ Checklist

- ✅ Domain Entity (Designation.cs)
- ✅ Domain Events (DesignationEvents.cs)
- ✅ Domain Exceptions (DesignationExceptions.cs)
- ✅ Create Command & Handler
- ✅ Create Validator
- ✅ Create Response (ID only)
- ✅ Get Query & Handler
- ✅ Get Response (Full DTO)
- ✅ Update Command & Handler
- ✅ Update Validator
- ✅ Update Response (ID only)
- ✅ Delete Command & Handler
- ✅ Delete Response (ID only)
- ✅ Search Request (Paginated)
- ✅ Search Handler
- ✅ Specifications (ById, ByCodeAndOrgUnit, Search)
- ✅ Database Configuration
- ✅ Create Endpoint (POST)
- ✅ Get Endpoint (GET)
- ✅ Update Endpoint (PUT)
- ✅ Delete Endpoint (DELETE)
- ✅ Search Endpoint (POST /search)
- ✅ Endpoint Router (DesignationsEndpoints.cs)
- ✅ Dependency Injection
- ✅ Module Registration
- ✅ Permission-based Access Control
- ✅ Multi-Tenant Support
- ✅ Audit Trail
- ✅ Fluent Validation
- ✅ CQRS Pattern
- ✅ Repository Pattern

---

## 📚 Related Entities

- **OrganizationalUnit** - The area/department that contains designations
- **Employee** - Employees assigned to designations
- **DesignationAssignment** - Historical assignment records
- **PayrollComponent** - Uses designation salary ranges

---

## 🎉 Summary

The **Designation domain** is **100% complete** with:
- ✅ Full CRUD operations
- ✅ Search with pagination and filters
- ✅ Area-specific job positions (same title, different rates)
- ✅ Fluent validation
- ✅ Domain events
- ✅ Repository pattern
- ✅ CQRS implementation
- ✅ RESTful endpoints
- ✅ Permission-based access control
- ✅ Multi-tenant support
- ✅ Audit trail
- ✅ Follows Todo/Catalog patterns exactly

**All responses follow the pattern:**
- **Create/Update/Delete**: Return ID only
- **Get**: Return full DTO with all fields
- **Search**: Return PagedList with filtering


