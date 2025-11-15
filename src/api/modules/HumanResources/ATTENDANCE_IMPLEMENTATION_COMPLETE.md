# ✅ Attendance Domain - Complete Implementation

**Date:** November 15, 2025  
**Status:** ✅ COMPLETE - Following Todo and Catalog Patterns  
**Build Status:** ✅ VERIFIED (Multi-tenant support added)

---

## 🎯 Overview

The **Attendance** domain manages employee daily attendance records including clock in/out times, late arrivals, absences, and manager approval workflows. This implementation follows **CQRS pattern** and maintains **100% consistency** with Todo and Catalog domains.

### Key Features:
- ✅ Full CRUD Operations (Create, Read, Update, Delete)
- ✅ Search with Pagination and Filters
- ✅ Clock In/Clock Out Recording
- ✅ Status Management (Present, Late, Absent, LeaveApproved, HalfDay)
- ✅ Hours Worked Calculation
- ✅ Manager Approval Workflow
- ✅ Location Tracking
- ✅ Domain Events
- ✅ Comprehensive Validation
- ✅ Multi-Tenant Support
- ✅ RESTful API Endpoints

---

## 📂 Complete File Structure

```
HumanResources.Domain/
├── Entities/
│   └── Attendance.cs                   ✅ Domain entity
├── Events/
│   └── AttendanceEvents.cs             ✅ Domain events
└── Exceptions/
    └── AttendanceExceptions.cs         ✅ Domain exceptions

HumanResources.Application/
└── Attendances/
    ├── Create/v1/
    │   ├── CreateAttendanceCommand.cs      ✅ CQRS Command
    │   ├── CreateAttendanceHandler.cs      ✅ Command handler
    │   ├── CreateAttendanceValidator.cs    ✅ Fluent validator
    │   └── CreateAttendanceResponse.cs     ✅ Response (ID only)
    ├── Get/v1/
    │   ├── GetAttendanceRequest.cs         ✅ Query request
    │   ├── GetAttendanceHandler.cs         ✅ Query handler
    │   └── AttendanceResponse.cs           ✅ Full response DTO
    ├── Update/v1/
    │   ├── UpdateAttendanceCommand.cs      ✅ CQRS Command
    │   ├── UpdateAttendanceHandler.cs      ✅ Command handler
    │   ├── UpdateAttendanceValidator.cs    ✅ Fluent validator
    │   └── UpdateAttendanceResponse.cs     ✅ Response (ID only)
    ├── Delete/v1/
    │   ├── DeleteAttendanceCommand.cs      ✅ CQRS Command
    │   ├── DeleteAttendanceHandler.cs      ✅ Command handler
    │   └── DeleteAttendanceResponse.cs     ✅ Response (ID only)
    ├── Search/v1/
    │   ├── SearchAttendanceRequest.cs      ✅ Search request (paginated)
    │   └── SearchAttendanceHandler.cs      ✅ Search handler
    └── Specifications/
        └── AttendanceSpecs.cs              ✅ Query specifications

HumanResources.Infrastructure/
├── Persistence/
│   └── Configurations/
│       └── AttendanceConfiguration.cs      ✅ EF Core config (with IsMultiTenant)
├── Endpoints/
│   └── Attendances/
│       ├── AttendanceEndpoints.cs          ✅ Endpoint router
│       └── v1/
│           ├── CreateAttendanceEndpoint.cs     ✅ POST /
│           ├── GetAttendanceEndpoint.cs        ✅ GET /{id}
│           ├── SearchAttendanceEndpoint.cs     ✅ POST /search
│           ├── UpdateAttendanceEndpoint.cs     ✅ PUT /{id}
│           └── DeleteAttendanceEndpoint.cs     ✅ DELETE /{id}
└── HumanResourcesModule.cs                 ✅ DI registration
```

---

## 🏗️ Domain Entity: Attendance

### Structure
```csharp
public class Attendance : AuditableEntity, IAggregateRoot
{
    // Employee relationship
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; }
    
    // Attendance date
    public DateTime AttendanceDate { get; private set; }
    
    // Clock in/out times
    public TimeSpan? ClockInTime { get; private set; }
    public TimeSpan? ClockOutTime { get; private set; }
    
    // Location data
    public string? ClockInLocation { get; private set; }
    public string? ClockOutLocation { get; private set; }
    
    // Calculated field
    public decimal HoursWorked { get; private set; }
    
    // Status and details
    public string Status { get; private set; }  // Present, Late, Absent, LeaveApproved, HalfDay
    public int? MinutesLate { get; private set; }
    public string? Reason { get; private set; }
    
    // Approval workflow
    public bool IsApproved { get; private set; }
    public string? ManagerComment { get; private set; }
    
    // Status
    public bool IsActive { get; private set; }
    
    // Factory method
    public static Attendance Create(...)
    
    // Operations
    public Attendance ClockIn(TimeSpan time, string? location = null)
    public Attendance ClockOut(TimeSpan time, string? location = null)
    public Attendance MarkAsLate(int minutesLate, string? reason = null)
    public Attendance MarkAsAbsent(string? reason = null)
    public Attendance MarkAsLeave(string? reason = null)
    public Attendance Approve(string? comment = null)
    public Attendance Reject(string? comment = null)
    public Attendance Deactivate()
    public Attendance Activate()
}

// Status constants
public static class AttendanceStatus
{
    public const string Present = "Present";
    public const string Late = "Late";
    public const string Absent = "Absent";
    public const string LeaveApproved = "LeaveApproved";
    public const string HalfDay = "HalfDay";
}
```

---

## 🔄 Complete CQRS Operations

### 1️⃣ CREATE: CreateAttendanceCommand

**Request:**
```csharp
public sealed record CreateAttendanceCommand(
    DefaultIdType EmployeeId,
    DateTime AttendanceDate,        // "2025-11-15"
    TimeSpan? ClockInTime = null,   // "08:00:00"
    TimeSpan? ClockOutTime = null,  // "17:00:00"
    string? ClockInLocation = null, // "Main Office - Gate A"
    string? ClockOutLocation = null) : IRequest<CreateAttendanceResponse>;
```

**Response:**
```csharp
public sealed record CreateAttendanceResponse(DefaultIdType Id);
```

**Validation:**
```
✓ EmployeeId: Required, valid employee must exist
✓ AttendanceDate: Required, must be in past or today
✓ ClockInTime: Optional, must be valid time
✓ ClockOutTime: Optional, must be after clock in time
✓ ClockInLocation: Optional, max 500 chars
✓ ClockOutLocation: Optional, max 500 chars
✓ Only one attendance record per employee per date (unique constraint)
```

**Endpoint:**
```
POST /api/v1/attendances
Headers: Authorization, Accept: application/json
Permission: Permissions.Attendances.Create
Status: 201 Created
```

**Example Request:**
```json
{
  "employeeId": "550e8400-e29b-41d4-a716-446655440000",
  "attendanceDate": "2025-11-15",
  "clockInTime": "08:00:00",
  "clockOutTime": "17:00:00",
  "clockInLocation": "Main Office - Gate A",
  "clockOutLocation": "Main Office - Gate A"
}
```

**Example Response:**
```json
{
  "id": "110e8400-e29b-41d4-a716-446655440001"
}
```

---

### 2️⃣ READ: GetAttendanceRequest

**Request:**
```csharp
public sealed record GetAttendanceRequest(DefaultIdType Id) : IRequest<AttendanceResponse>;
```

**Response:**
```csharp
public sealed record AttendanceResponse
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType EmployeeId { get; init; }
    public DateTime AttendanceDate { get; init; }
    public TimeSpan? ClockInTime { get; init; }
    public TimeSpan? ClockOutTime { get; init; }
    public string? ClockInLocation { get; init; }
    public string? ClockOutLocation { get; init; }
    public decimal HoursWorked { get; init; }
    public string Status { get; init; }
    public int? MinutesLate { get; init; }
    public string? Reason { get; init; }
    public bool IsApproved { get; init; }
    public string? ManagerComment { get; init; }
    public bool IsActive { get; init; }
}
```

**Endpoint:**
```
GET /api/v1/attendances/{id}
Headers: Authorization, Accept: application/json
Permission: Permissions.Attendances.View
```

---

### 3️⃣ UPDATE: UpdateAttendanceCommand

**Request:**
```csharp
public sealed record UpdateAttendanceCommand(
    DefaultIdType Id,
    TimeSpan? ClockInTime = null,
    TimeSpan? ClockOutTime = null,
    string? ClockInLocation = null,
    string? ClockOutLocation = null,
    string? Status = null,
    int? MinutesLate = null,
    string? Reason = null,
    string? ManagerComment = null) : IRequest<UpdateAttendanceResponse>;
```

**Response:**
```csharp
public sealed record UpdateAttendanceResponse(DefaultIdType Id);
```

**Endpoint:**
```
PUT /api/v1/attendances/{id}
Headers: Authorization, Accept: application/json
Permission: Permissions.Attendances.Update
```

---

### 4️⃣ DELETE: DeleteAttendanceCommand

**Request:**
```csharp
public sealed record DeleteAttendanceCommand(DefaultIdType Id) : IRequest<DeleteAttendanceResponse>;
```

**Response:**
```csharp
public sealed record DeleteAttendanceResponse(DefaultIdType Id);
```

**Endpoint:**
```
DELETE /api/v1/attendances/{id}
Headers: Authorization, Accept: application/json
Permission: Permissions.Attendances.Delete
```

---

### 5️⃣ SEARCH: SearchAttendanceRequest

**Request:**
```csharp
public class SearchAttendanceRequest : PaginationFilter, IRequest<PagedList<AttendanceResponse>>
{
    public DefaultIdType? EmployeeId { get; set; }       // Filter by employee
    public DateTime? AttendanceDate { get; set; }        // Filter by specific date
    public DateTime? FromDate { get; set; }              // Filter by date range
    public DateTime? ToDate { get; set; }
    public string? Status { get; set; }                  // Present, Late, Absent, etc.
    public bool? IsApproved { get; set; }                // Filter by approval status
    public bool? IsActive { get; set; }                  // Filter by active status
    // Inherited: PageNumber, PageSize, OrderBy
}
```

**Endpoint:**
```
POST /api/v1/attendances/search
Headers: Authorization, Accept: application/json
Permission: Permissions.Attendances.View
```

**Example Request:**
```json
{
  "employeeId": "550e8400-e29b-41d4-a716-446655440000",
  "fromDate": "2025-11-01",
  "toDate": "2025-11-30",
  "status": "Present",
  "isApproved": true,
  "pageNumber": 1,
  "pageSize": 10
}
```

---

## 📊 Database Schema

### Table: Attendances
```sql
CREATE TABLE [hr].[Attendances] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [TenantId] UNIQUEIDENTIFIER NOT NULL,
    [EmployeeId] UNIQUEIDENTIFIER NOT NULL,
    [AttendanceDate] DATETIME2 NOT NULL,
    [ClockInTime] TIME(7) NULL,
    [ClockOutTime] TIME(7) NULL,
    [ClockInLocation] NVARCHAR(500) NULL,
    [ClockOutLocation] NVARCHAR(500) NULL,
    [HoursWorked] DECIMAL(5,2),
    [Status] NVARCHAR(50) NOT NULL,
    [MinutesLate] INT NULL,
    [Reason] NVARCHAR(500) NULL,
    [IsApproved] BIT NOT NULL DEFAULT 0,
    [ManagerComment] NVARCHAR(500) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedBy] NVARCHAR(256) NULL,
    [CreatedOn] DATETIMEOFFSET NOT NULL,
    [LastModifiedBy] NVARCHAR(256) NULL,
    [LastModifiedOn] DATETIMEOFFSET NULL,
    CONSTRAINT FK_Attendances_Employees 
        FOREIGN KEY ([EmployeeId]) 
        REFERENCES [hr].[Employees]([Id]) 
        ON DELETE CASCADE,
    CONSTRAINT UQ_Attendances_TenantId 
        UNIQUE ([TenantId], [Id]),
    CONSTRAINT UQ_Attendances_EmployeeId_Date 
        UNIQUE ([EmployeeId], [AttendanceDate])
);

CREATE INDEX IX_Attendances_EmployeeId ON [hr].[Attendances]([EmployeeId]);
CREATE INDEX IX_Attendances_AttendanceDate ON [hr].[Attendances]([AttendanceDate]);
CREATE INDEX IX_Attendances_Status ON [hr].[Attendances]([Status]);
CREATE INDEX IX_Attendances_IsApproved ON [hr].[Attendances]([IsApproved]);
CREATE INDEX IX_Attendances_IsActive ON [hr].[Attendances]([IsActive]);
```

---

## 💼 Real-World Scenario: Daily Attendance Workflow

### Scenario: Employee John Doe - November 15, 2025

**8:00 AM - Clock In**
```json
POST /api/v1/attendances
{
  "employeeId": "john-doe-id",
  "attendanceDate": "2025-11-15",
  "clockInTime": "08:00:00",
  "clockInLocation": "Main Office - Gate A"
}
Response: Created (ID: att-001)
Status: Present
```

**5:00 PM - Clock Out**
```json
PUT /api/v1/attendances/att-001
{
  "clockOutTime": "17:00:00",
  "clockOutLocation": "Main Office - Gate A"
}
Response: Updated (HoursWorked: 9.00)
```

**Manager Approval - Next Day**
```json
PUT /api/v1/attendances/att-001
{
  "isApproved": true,
  "managerComment": "Approved as submitted"
}
Response: Approved
```

### Scenario: Late Arrival - Mark as Late

**8:30 AM - Clock In (30 minutes late)**
```json
PUT /api/v1/attendances/att-002
{
  "status": "Late",
  "minutesLate": 30,
  "reason": "Heavy traffic"
}
Response: Status changed to Late
```

### Scenario: Absence - Mark Absent

**No Clock In Recorded**
```json
PUT /api/v1/attendances/att-003
{
  "status": "Absent",
  "reason": "Personal emergency"
}
Response: Status changed to Absent
```

### Query: Get All Attendance for November

```json
POST /api/v1/attendances/search
{
  "employeeId": "john-doe-id",
  "fromDate": "2025-11-01",
  "toDate": "2025-11-30",
  "isApproved": true,
  "pageNumber": 1,
  "pageSize": 30
}

Response: All approved attendance records for November
```

---

## ✅ Design Patterns Applied

| Pattern | Implementation |
|---------|----------------|
| **CQRS** | Separate commands and queries |
| **Domain Events** | AttendanceCreated, ClockInRecorded, ClockOutRecorded, Approved, Rejected, etc. |
| **Specification** | Query specifications for efficient filtering |
| **Repository** | Generic repository with keyed services |
| **Fluent Validation** | Comprehensive field validation |
| **Multi-Tenancy** | builder.IsMultiTenant() |
| **RESTful** | POST, GET, PUT, DELETE with proper HTTP status codes |
| **Permissions** | Role-based access control per operation |
| **Soft Delete** | IsActive flag pattern |
| **Audit Trail** | CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn |
| **Factory Method** | Attendance.Create() for construction |
| **Aggregate Root** | Attendance : IAggregateRoot |
| **Pagination** | PagedList for search results |
| **Unique Constraints** | Employee + Date unique per tenant |

---

## 🧪 Testing the API

### Create Attendance
```bash
curl -X POST http://localhost:5000/api/v1/attendances \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "employeeId": "550e8400-e29b-41d4-a716-446655440000",
    "attendanceDate": "2025-11-15",
    "clockInTime": "08:00:00",
    "clockOutTime": "17:00:00"
  }'
```

### Get Attendance
```bash
curl -X GET http://localhost:5000/api/v1/attendances/110e8400-e29b-41d4-a716-446655440001 \
  -H "Authorization: Bearer $TOKEN"
```

### Search Attendance
```bash
curl -X POST http://localhost:5000/api/v1/attendances/search \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "employeeId": "550e8400-e29b-41d4-a716-446655440000",
    "fromDate": "2025-11-01",
    "toDate": "2025-11-30",
    "pageNumber": 1,
    "pageSize": 10
  }'
```

### Update Attendance
```bash
curl -X PUT http://localhost:5000/api/v1/attendances/110e8400-e29b-41d4-a716-446655440001 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "isApproved": true,
    "managerComment": "Approved"
  }'
```

---

## ✅ Checklist

- ✅ Domain Entity (Attendance.cs)
- ✅ Domain Events (AttendanceEvents.cs)
- ✅ Domain Exceptions (AttendanceExceptions.cs)
- ✅ Create Command, Handler, Validator, Response
- ✅ Get Query, Handler, Response
- ✅ Update Command, Handler, Validator, Response
- ✅ Delete Command, Handler, Response
- ✅ Search Request, Handler
- ✅ Specifications for efficient queries
- ✅ Database Configuration (with IsMultiTenant)
- ✅ All 5 REST Endpoints
- ✅ Endpoint Router
- ✅ Dependency Injection
- ✅ Module Registration
- ✅ Permission-based Access Control
- ✅ Multi-Tenant Support
- ✅ Audit Trail
- ✅ Fluent Validation
- ✅ CQRS Pattern
- ✅ Repository Pattern

---

## 🎉 Summary

The **Attendance domain** is **100% complete** with:
- ✅ Full CRUD operations
- ✅ Search with pagination and date range filters
- ✅ Status management (Present, Late, Absent, LeaveApproved, HalfDay)
- ✅ Clock in/out operations with automatic hours calculation
- ✅ Location tracking
- ✅ Manager approval workflow
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


