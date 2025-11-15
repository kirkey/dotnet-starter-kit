# ✅ TimesheetLine Domain - Complete Implementation

**Date:** November 15, 2025  
**Status:** ✅ COMPLETE - Following Todo and Catalog Patterns  
**Build Status:** ✅ VERIFIED - No Errors

---

## 🎯 Overview

The **TimesheetLine** domain manages daily timesheet entries within a timesheet period. Each line represents one day's work with regular hours, overtime hours, project allocation, and billing information. This implementation follows **CQRS pattern** and maintains **100% consistency** with Todo and Catalog domains.

### Key Features:
- ✅ Full CRUD Operations (Create, Read, Update, Delete)
- ✅ Search with Pagination and Filters
- ✅ Daily Hours Tracking (Regular and Overtime)
- ✅ Project/Task Allocation
- ✅ Billing Rate Management
- ✅ Hours Validation (0-24 hours per day, max total 24)
- ✅ Domain Events
- ✅ Comprehensive Validation
- ✅ Multi-Tenant Support
- ✅ RESTful API Endpoints

---

## 📂 Complete File Structure

```
HumanResources.Domain/
├── Entities/
│   └── TimesheetLine.cs                   ✅ Domain entity
└── (Events in Timesheet-related files)    ✅ Shared events

HumanResources.Application/
└── TimesheetLines/
    ├── Create/v1/
    │   ├── CreateTimesheetLineCommand.cs      ✅ CQRS Command
    │   ├── CreateTimesheetLineHandler.cs      ✅ Command handler
    │   ├── CreateTimesheetLineValidator.cs    ✅ Fluent validator
    │   └── CreateTimesheetLineResponse.cs     ✅ Response (ID only)
    ├── Get/v1/
    │   ├── GetTimesheetLineRequest.cs         ✅ Query request
    │   ├── GetTimesheetLineHandler.cs         ✅ Query handler
    │   └── TimesheetLineResponse.cs           ✅ Full response DTO
    ├── Update/v1/
    │   ├── UpdateTimesheetLineCommand.cs      ✅ CQRS Command
    │   ├── UpdateTimesheetLineHandler.cs      ✅ Command handler
    │   ├── UpdateTimesheetLineValidator.cs    ✅ Fluent validator
    │   └── UpdateTimesheetLineResponse.cs     ✅ Response (ID only)
    ├── Delete/v1/
    │   ├── DeleteTimesheetLineCommand.cs      ✅ CQRS Command
    │   ├── DeleteTimesheetLineHandler.cs      ✅ Command handler
    │   └── DeleteTimesheetLineResponse.cs     ✅ Response (ID only)
    ├── Search/v1/
    │   ├── SearchTimesheetLinesRequest.cs     ✅ Search request (paginated)
    │   └── SearchTimesheetLinesHandler.cs     ✅ Search handler
    └── Specifications/
        └── TimesheetLineSpecs.cs              ✅ Query specifications

HumanResources.Infrastructure/
├── Persistence/
│   └── Configurations/
│       └── TimesheetConfiguration.cs         ✅ EF Core config (TimesheetLineConfiguration with IsMultiTenant)
├── Endpoints/
│   └── TimesheetLines/
│       ├── TimesheetLinesEndpoints.cs         ✅ Endpoint router (NEW)
│       └── v1/
│           ├── CreateTimesheetLineEndpoint.cs     ✅ POST / (NEW)
│           ├── GetTimesheetLineEndpoint.cs        ✅ GET /{id} (NEW)
│           ├── SearchTimesheetLinesEndpoint.cs    ✅ POST /search (NEW)
│           ├── UpdateTimesheetLineEndpoint.cs     ✅ PUT /{id} (NEW)
│           └── DeleteTimesheetLineEndpoint.cs     ✅ DELETE /{id} (NEW)
└── HumanResourcesModule.cs                   ✅ DI registration + endpoint routing
```

---

## 🏗️ Domain Entity: TimesheetLine

### Structure
```csharp
public class TimesheetLine : AuditableEntity, IAggregateRoot
{
    // Timesheet relationship
    public DefaultIdType TimesheetId { get; private set; }
    public Timesheet Timesheet { get; private set; }
    
    // Work date
    public DateTime WorkDate { get; private set; }
    
    // Hours tracking
    public decimal RegularHours { get; private set; }      // 0-8 typically
    public decimal OvertimeHours { get; private set; }     // >8 hours
    public decimal TotalHours => RegularHours + OvertimeHours  // Computed
    
    // Project/task allocation
    public string? ProjectId { get; private set; }
    public string? TaskDescription { get; private set; }
    
    // Billing information
    public bool IsBillable { get; private set; }
    public decimal? BillingRate { get; private set; }
    
    // Factory method
    public static TimesheetLine Create(...)
    
    // Update operations
    public TimesheetLine UpdateHours(decimal regularHours, decimal overtimeHours)
    public TimesheetLine SetProject(string projectId, string? taskDescription = null)
    public TimesheetLine MarkAsBillable(decimal billingRate)
    public TimesheetLine MarkAsNonBillable()
}
```

---

## 🔄 Complete CQRS Operations

### 1️⃣ CREATE: CreateTimesheetLineCommand

**Request:**
```csharp
public sealed record CreateTimesheetLineCommand(
    DefaultIdType TimesheetId,           // "ts-123"
    DateTime WorkDate,                   // "2025-11-15"
    decimal RegularHours = 0,            // 8.0
    decimal OvertimeHours = 0,           // 2.0
    string? ProjectId = null,            // "PROJ-001"
    string? TaskDescription = null,      // "Development Work"
    decimal? BillingRate = null) : IRequest<CreateTimesheetLineResponse>;
```

**Response:**
```csharp
public sealed record CreateTimesheetLineResponse(DefaultIdType Id);
```

**Validation:**
```
✓ TimesheetId: Required, timesheet must exist
✓ WorkDate: Required, must be within timesheet period
✓ RegularHours: ≥ 0, ≤ 24
✓ OvertimeHours: ≥ 0, ≤ 24
✓ Total hours: ≤ 24 per day
✓ Only one line per timesheet per date (unique constraint)
✓ ProjectId: Optional, max 50 chars
✓ TaskDescription: Optional, max 500 chars
✓ BillingRate: Optional, ≥ 0
```

**Endpoint:**
```
POST /api/v1/timesheet-lines
Headers: Authorization, Accept: application/json
Permission: Permissions.TimesheetLines.Create
Status: 201 Created
```

**Example Request:**
```json
{
  "timesheetId": "550e8400-e29b-41d4-a716-446655440000",
  "workDate": "2025-11-15",
  "regularHours": 8.0,
  "overtimeHours": 2.0,
  "projectId": "PROJ-001",
  "taskDescription": "Feature Development",
  "billingRate": 150.00
}
```

**Example Response:**
```json
{
  "id": "110e8400-e29b-41d4-a716-446655440001"
}
```

---

### 2️⃣ READ: GetTimesheetLineRequest

**Request:**
```csharp
public sealed record GetTimesheetLineRequest(DefaultIdType Id) : IRequest<TimesheetLineResponse>;
```

**Response:**
```csharp
public sealed record TimesheetLineResponse
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType TimesheetId { get; init; }
    public DateTime WorkDate { get; init; }
    public decimal RegularHours { get; init; }
    public decimal OvertimeHours { get; init; }
    public decimal TotalHours { get; init; }
    public string? ProjectId { get; init; }
    public string? TaskDescription { get; init; }
    public bool IsBillable { get; init; }
    public decimal? BillingRate { get; init; }
}
```

**Endpoint:**
```
GET /api/v1/timesheet-lines/{id}
Headers: Authorization, Accept: application/json
Permission: Permissions.TimesheetLines.View
```

**Example Response:**
```json
{
  "id": "110e8400-e29b-41d4-a716-446655440001",
  "timesheetId": "550e8400-e29b-41d4-a716-446655440000",
  "workDate": "2025-11-15",
  "regularHours": 8.0,
  "overtimeHours": 2.0,
  "totalHours": 10.0,
  "projectId": "PROJ-001",
  "taskDescription": "Feature Development",
  "isBillable": true,
  "billingRate": 150.00
}
```

---

### 3️⃣ UPDATE: UpdateTimesheetLineCommand

**Request:**
```csharp
public sealed record UpdateTimesheetLineCommand(
    DefaultIdType Id,
    decimal? RegularHours = null,
    decimal? OvertimeHours = null,
    string? ProjectId = null,
    string? TaskDescription = null,
    bool? IsBillable = null,
    decimal? BillingRate = null) : IRequest<UpdateTimesheetLineResponse>;
```

**Response:**
```csharp
public sealed record UpdateTimesheetLineResponse(DefaultIdType Id);
```

**Endpoint:**
```
PUT /api/v1/timesheet-lines/{id}
Headers: Authorization, Accept: application/json
Permission: Permissions.TimesheetLines.Update
```

**Example Request:**
```json
{
  "regularHours": 7.5,
  "overtimeHours": 2.5,
  "projectId": "PROJ-002"
}
```

---

### 4️⃣ DELETE: DeleteTimesheetLineCommand

**Request:**
```csharp
public sealed record DeleteTimesheetLineCommand(DefaultIdType Id) : IRequest<DeleteTimesheetLineResponse>;
```

**Response:**
```csharp
public sealed record DeleteTimesheetLineResponse(DefaultIdType Id);
```

**Endpoint:**
```
DELETE /api/v1/timesheet-lines/{id}
Headers: Authorization, Accept: application/json
Permission: Permissions.TimesheetLines.Delete
```

---

### 5️⃣ SEARCH: SearchTimesheetLinesRequest

**Request:**
```csharp
public class SearchTimesheetLinesRequest : PaginationFilter, IRequest<PagedList<TimesheetLineResponse>>
{
    public DefaultIdType? TimesheetId { get; set; }   // Filter by timesheet
    public DateTime? WorkDate { get; set; }           // Filter by specific date
    public DateTime? FromDate { get; set; }           // Filter by date range
    public DateTime? ToDate { get; set; }
    public string? ProjectId { get; set; }            // Filter by project
    public bool? IsBillable { get; set; }             // Filter by billable status
    // Inherited: PageNumber, PageSize, OrderBy
}
```

**Endpoint:**
```
POST /api/v1/timesheet-lines/search
Headers: Authorization, Accept: application/json
Permission: Permissions.TimesheetLines.View
```

**Example Request:**
```json
{
  "timesheetId": "550e8400-e29b-41d4-a716-446655440000",
  "projectId": "PROJ-001",
  "isBillable": true,
  "pageNumber": 1,
  "pageSize": 10
}
```

---

## 📊 Database Schema

### Table: TimesheetLines
```sql
CREATE TABLE [hr].[TimesheetLines] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [TenantId] UNIQUEIDENTIFIER NOT NULL,
    [TimesheetId] UNIQUEIDENTIFIER NOT NULL,
    [WorkDate] DATETIME2 NOT NULL,
    [RegularHours] DECIMAL(5,2) NOT NULL,
    [OvertimeHours] DECIMAL(5,2) NOT NULL,
    [ProjectId] NVARCHAR(50) NULL,
    [TaskDescription] NVARCHAR(500) NULL,
    [IsBillable] BIT NOT NULL DEFAULT 0,
    [BillingRate] DECIMAL(10,2) NULL,
    [CreatedBy] NVARCHAR(256) NULL,
    [CreatedOn] DATETIMEOFFSET NOT NULL,
    [LastModifiedBy] NVARCHAR(256) NULL,
    [LastModifiedOn] DATETIMEOFFSET NULL,
    CONSTRAINT FK_TimesheetLines_Timesheets 
        FOREIGN KEY ([TimesheetId]) 
        REFERENCES [hr].[Timesheets]([Id]) 
        ON DELETE CASCADE,
    CONSTRAINT UQ_TimesheetLines_TenantId 
        UNIQUE ([TenantId], [Id])
);

CREATE INDEX IX_TimesheetLine_TimesheetId ON [hr].[TimesheetLines]([TimesheetId]);
CREATE INDEX IX_TimesheetLine_WorkDate ON [hr].[TimesheetLines]([WorkDate]);
CREATE INDEX IX_TimesheetLine_ProjectId ON [hr].[TimesheetLines]([ProjectId]);
CREATE INDEX IX_TimesheetLine_IsBillable ON [hr].[TimesheetLines]([IsBillable]);
```

---

## 💼 Real-World Scenario: Weekly Timesheet Entry

**Monday (Nov 10, 2025) - 8 Regular Hours**
```json
POST /api/v1/timesheet-lines
{
  "timesheetId": "ts-001",
  "workDate": "2025-11-10",
  "regularHours": 8.0,
  "overtimeHours": 0.0,
  "projectId": "PROJ-001",
  "taskDescription": "Design Phase",
  "billingRate": 150.00
}
```

**Tuesday (Nov 11) - 8 Regular + 1 OT**
```json
POST /api/v1/timesheet-lines
{
  "timesheetId": "ts-001",
  "workDate": "2025-11-11",
  "regularHours": 8.0,
  "overtimeHours": 1.0,
  "projectId": "PROJ-001",
  "taskDescription": "Development",
  "billingRate": 150.00
}
```

**Wednesday - Update to add OT**
```json
PUT /api/v1/timesheet-lines/line-001
{
  "overtimeHours": 2.0,
  "taskDescription": "Critical bug fix"
}
```

**Search: Get all billable hours for project**
```json
POST /api/v1/timesheet-lines/search
{
  "timesheetId": "ts-001",
  "projectId": "PROJ-001",
  "isBillable": true,
  "pageNumber": 1,
  "pageSize": 20
}
```

---

## ✅ Design Patterns Applied

| Pattern | Implementation |
|---------|----------------|
| **CQRS** | Separate commands and queries |
| **Domain Events** | Related through Timesheet events |
| **Specification** | TimesheetLineSearchSpec for complex queries |
| **Repository** | Generic repository with keyed services |
| **Fluent Validation** | Comprehensive field validation |
| **Multi-Tenancy** | builder.IsMultiTenant() in configuration |
| **RESTful** | POST, GET, PUT, DELETE with proper HTTP methods |
| **Permissions** | Role-based access control per operation |
| **Audit Trail** | CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn |
| **Factory Method** | TimesheetLine.Create() for construction |
| **Aggregate Root** | TimesheetLine : IAggregateRoot |
| **Pagination** | PagedList for search results |
| **Computed Properties** | TotalHours calculated at runtime |

---

## 🧪 Testing the API

### Create Line
```bash
curl -X POST http://localhost:5000/api/v1/timesheet-lines \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "timesheetId": "550e8400-e29b-41d4-a716-446655440000",
    "workDate": "2025-11-15",
    "regularHours": 8.0,
    "overtimeHours": 2.0,
    "projectId": "PROJ-001"
  }'
```

### Get Line
```bash
curl -X GET http://localhost:5000/api/v1/timesheet-lines/110e8400-e29b-41d4-a716-446655440001 \
  -H "Authorization: Bearer $TOKEN"
```

### Search Lines
```bash
curl -X POST http://localhost:5000/api/v1/timesheet-lines/search \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "timesheetId": "550e8400-e29b-41d4-a716-446655440000",
    "isBillable": true,
    "pageNumber": 1,
    "pageSize": 10
  }'
```

---

## ✅ Implementation Checklist

- ✅ Domain Entity (TimesheetLine.cs) with all methods
- ✅ Create Command, Handler, Validator, Response
- ✅ Get Query, Handler, Response
- ✅ Update Command, Handler, Validator, Response
- ✅ Delete Command, Handler, Response
- ✅ Search Request, Handler
- ✅ Specifications (with unique date constraint check)
- ✅ Database Configuration (with IsMultiTenant)
- ✅ All 5 REST Endpoints
- ✅ Endpoint Router
- ✅ Dependency Injection in HumanResourcesModule
- ✅ Module Endpoint Mapping
- ✅ Permission-based Access Control
- ✅ Multi-Tenant Support
- ✅ Audit Trail
- ✅ Fluent Validation
- ✅ CQRS Pattern
- ✅ Repository Pattern

---

## 🎉 Summary

The **TimesheetLine domain** is **100% complete** with:
- ✅ Full CRUD operations
- ✅ Search with pagination and filters
- ✅ Hours validation (0-24 per day)
- ✅ Project and billing tracking
- ✅ Fluent validation
- ✅ Repository pattern
- ✅ CQRS implementation
- ✅ RESTful endpoints (5 total)
- ✅ Permission-based access control
- ✅ Multi-tenant support
- ✅ Audit trail
- ✅ Follows Todo/Catalog patterns exactly

**All responses follow the pattern:**
- **Create/Update/Delete**: Return ID only
- **Get**: Return full DTO with all fields (including computed TotalHours)
- **Search**: Return PagedList with filtering


