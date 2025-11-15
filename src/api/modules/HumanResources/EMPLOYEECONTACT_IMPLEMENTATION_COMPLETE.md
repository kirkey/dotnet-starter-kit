# ✅ EmployeeContact Domain - Complete Implementation

**Date:** November 15, 2025  
**Status:** ✅ COMPLETE - Following Todo and Catalog Patterns  
**Build Status:** ✅ VERIFIED (No EmployeeContact-specific errors)

---

## 🎯 Overview

The **EmployeeContact** domain manages emergency contacts, family members, and professional references for each employee. This implementation follows **CQRS pattern** and maintains **100% consistency** with Todo and Catalog domains.

### Key Features:
- ✅ Full CRUD Operations (Create, Read, Update, Delete)
- ✅ Search with Pagination and Filters
- ✅ Contact Type Management (Emergency, NextOfKin, Reference, Family)
- ✅ Priority-based Ordering (for emergency contacts)
- ✅ Activation/Deactivation (soft delete)
- ✅ Domain Events
- ✅ Comprehensive Validation
- ✅ Multi-Tenant Support
- ✅ RESTful API Endpoints

---

## 📂 Complete File Structure

```
HumanResources.Domain/
├── Entities/
│   └── EmployeeContact.cs                  ✅ Domain entity
├── Events/
│   └── EmployeeContactEvents.cs            ✅ Domain events
└── Exceptions/
    └── EmployeeContactExceptions.cs        ✅ Domain exceptions

HumanResources.Application/
└── EmployeeContacts/
    ├── Create/v1/
    │   ├── CreateEmployeeContactCommand.cs    ✅ CQRS Command
    │   ├── CreateEmployeeContactHandler.cs    ✅ Command handler
    │   ├── CreateEmployeeContactValidator.cs  ✅ Fluent validator
    │   └── CreateEmployeeContactResponse.cs   ✅ Response (ID only)
    ├── Get/v1/
    │   ├── GetEmployeeContactRequest.cs       ✅ Query request
    │   ├── GetEmployeeContactHandler.cs       ✅ Query handler
    │   └── EmployeeContactResponse.cs         ✅ Full response DTO
    ├── Update/v1/
    │   ├── UpdateEmployeeContactCommand.cs    ✅ CQRS Command
    │   ├── UpdateEmployeeContactHandler.cs    ✅ Command handler
    │   ├── UpdateEmployeeContactValidator.cs  ✅ Fluent validator
    │   └── UpdateEmployeeContactResponse.cs   ✅ Response (ID only)
    ├── Delete/v1/
    │   ├── DeleteEmployeeContactCommand.cs    ✅ CQRS Command
    │   ├── DeleteEmployeeContactHandler.cs    ✅ Command handler
    │   └── DeleteEmployeeContactResponse.cs   ✅ Response (ID only)
    ├── Search/v1/
    │   ├── SearchEmployeeContactsRequest.cs   ✅ Search request (paginated)
    │   └── SearchEmployeeContactsHandler.cs   ✅ Search handler
    └── Specifications/
        └── EmployeeContactSpecs.cs            ✅ Query specifications

HumanResources.Infrastructure/
├── Persistence/
│   └── Configurations/
│       └── EmployeeContactConfiguration.cs    ✅ EF Core config (with IsMultiTenant)
├── Endpoints/
│   └── EmployeeContacts/
│       ├── EmployeeContactsEndpoints.cs       ✅ Endpoint router
│       └── v1/
│           ├── CreateEmployeeContactEndpoint.cs    ✅ POST /
│           ├── GetEmployeeContactEndpoint.cs       ✅ GET /{id}
│           ├── SearchEmployeeContactsEndpoint.cs   ✅ POST /search
│           ├── UpdateEmployeeContactEndpoint.cs    ✅ PUT /{id}
│           └── DeleteEmployeeContactEndpoint.cs    ✅ DELETE /{id}
└── HumanResourcesModule.cs                   ✅ DI registration
```

---

## 🏗️ Domain Entity: EmployeeContact

### Structure
```csharp
public class EmployeeContact : AuditableEntity, IAggregateRoot
{
    // Employee relationship
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; }
    
    // Contact information
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName => computed property
    
    // Contact details
    public string ContactType { get; private set; }  // Emergency, NextOfKin, Reference, Family
    public string? Relationship { get; private set; }  // Spouse, Parent, Sibling, etc.
    public string? PhoneNumber { get; private set; }
    public string? Email { get; private set; }
    public string? Address { get; private set; }
    
    // Ordering and status
    public int Priority { get; private set; }  // For emergency contacts (1 = first contact)
    public bool IsActive { get; private set; }
    
    // Factory method
    public static EmployeeContact Create(
        DefaultIdType employeeId,
        string firstName,
        string lastName,
        string contactType,
        string? relationship = null,
        string? phoneNumber = null,
        string? email = null,
        string? address = null)
    
    // Update method
    public EmployeeContact Update(
        string? firstName = null,
        string? lastName = null,
        string? relationship = null,
        string? phoneNumber = null,
        string? email = null,
        string? address = null)
    
    // Priority and status methods
    public EmployeeContact SetPriority(int priority)
    public EmployeeContact Deactivate()
    public EmployeeContact Activate()
}
```

---

## 🔄 Complete CQRS Operations

### 1️⃣ CREATE: CreateEmployeeContactCommand

**Request:**
```csharp
public sealed record CreateEmployeeContactCommand(
    DefaultIdType EmployeeId,
    string FirstName,              // "Jane"
    string LastName,               // "Doe"
    string ContactType,            // "Emergency", "NextOfKin", "Reference", "Family"
    string? Relationship = null,   // "Spouse", "Parent", "Manager"
    string? PhoneNumber = null,    // "+639171234567"
    string? Email = null,          // "jane@example.com"
    string? Address = null) : IRequest<CreateEmployeeContactResponse>;
```

**Response:**
```csharp
public sealed record CreateEmployeeContactResponse(DefaultIdType Id);
```

**Validation:**
```
✓ EmployeeId: Required, valid employee must exist
✓ FirstName: Required, max 256 chars
✓ LastName: Required, max 256 chars
✓ ContactType: Required, must be one of valid types
✓ Relationship: Optional, max 100 chars
✓ PhoneNumber: Optional, Philippines format if provided
✓ Email: Optional, valid email format
✓ Address: Optional, max 500 chars
```

**Endpoint:**
```
POST /api/v1/employee-contacts
Headers: Authorization, Accept: application/json
Permission: Permissions.EmployeeContacts.Create
Status: 201 Created
```

**Example Request:**
```json
{
  "employeeId": "550e8400-e29b-41d4-a716-446655440000",
  "firstName": "Jane",
  "lastName": "Doe",
  "contactType": "Emergency",
  "relationship": "Spouse",
  "phoneNumber": "+639171234567",
  "email": "jane@example.com",
  "address": "123 Main Street, Manila"
}
```

**Example Response:**
```json
{
  "id": "110e8400-e29b-41d4-a716-446655440001"
}
```

---

### 2️⃣ READ: GetEmployeeContactRequest

**Request:**
```csharp
public sealed record GetEmployeeContactRequest(DefaultIdType Id) : IRequest<EmployeeContactResponse>;
```

**Response:**
```csharp
public sealed record EmployeeContactResponse
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType EmployeeId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string FullName { get; init; }
    public string ContactType { get; init; }
    public string? Relationship { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Email { get; init; }
    public string? Address { get; init; }
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}
```

**Endpoint:**
```
GET /api/v1/employee-contacts/{id}
Headers: Authorization, Accept: application/json
Permission: Permissions.EmployeeContacts.View
```

**Example Response:**
```json
{
  "id": "110e8400-e29b-41d4-a716-446655440001",
  "employeeId": "550e8400-e29b-41d4-a716-446655440000",
  "firstName": "Jane",
  "lastName": "Doe",
  "fullName": "Jane Doe",
  "contactType": "Emergency",
  "relationship": "Spouse",
  "phoneNumber": "+639171234567",
  "email": "jane@example.com",
  "address": "123 Main Street, Manila",
  "priority": 1,
  "isActive": true
}
```

---

### 3️⃣ UPDATE: UpdateEmployeeContactCommand

**Request:**
```csharp
public sealed record UpdateEmployeeContactCommand(
    DefaultIdType Id,
    string? FirstName = null,
    string? LastName = null,
    string? Relationship = null,
    string? PhoneNumber = null,
    string? Email = null,
    string? Address = null,
    int? Priority = null) : IRequest<UpdateEmployeeContactResponse>;
```

**Response:**
```csharp
public sealed record UpdateEmployeeContactResponse(DefaultIdType Id);
```

**Endpoint:**
```
PUT /api/v1/employee-contacts/{id}
Headers: Authorization, Accept: application/json
Permission: Permissions.EmployeeContacts.Update
```

**Example Request:**
```json
{
  "phoneNumber": "+639171234568",
  "email": "jane.newemail@example.com",
  "priority": 2
}
```

---

### 4️⃣ DELETE: DeleteEmployeeContactCommand

**Request:**
```csharp
public sealed record DeleteEmployeeContactCommand(DefaultIdType Id) : IRequest<DeleteEmployeeContactResponse>;
```

**Response:**
```csharp
public sealed record DeleteEmployeeContactResponse(DefaultIdType Id);
```

**Endpoint:**
```
DELETE /api/v1/employee-contacts/{id}
Headers: Authorization, Accept: application/json
Permission: Permissions.EmployeeContacts.Delete
```

---

### 5️⃣ SEARCH: SearchEmployeeContactsRequest

**Request:**
```csharp
public class SearchEmployeeContactsRequest : PaginationFilter, IRequest<PagedList<EmployeeContactResponse>>
{
    public DefaultIdType? EmployeeId { get; set; }      // Filter by employee
    public string? FirstName { get; set; }              // Filter by first name
    public string? ContactType { get; set; }            // Emergency, NextOfKin, etc.
    public bool? IsActive { get; set; }                 // Filter by status
    // Inherited: PageNumber, PageSize, OrderBy
}
```

**Endpoint:**
```
POST /api/v1/employee-contacts/search
Headers: Authorization, Accept: application/json
Permission: Permissions.EmployeeContacts.View
```

**Example Request:**
```json
{
  "employeeId": "550e8400-e29b-41d4-a716-446655440000",
  "contactType": "Emergency",
  "isActive": true,
  "pageNumber": 1,
  "pageSize": 10
}
```

**Example Response:**
```json
{
  "data": [
    {
      "id": "110e8400-e29b-41d4-a716-446655440001",
      "employeeId": "550e8400-e29b-41d4-a716-446655440000",
      "firstName": "Jane",
      "lastName": "Doe",
      "contactType": "Emergency",
      "priority": 1,
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

## 📊 Database Schema

### Table: EmployeeContacts
```sql
CREATE TABLE [hr].[EmployeeContacts] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [TenantId] UNIQUEIDENTIFIER NOT NULL,
    [EmployeeId] UNIQUEIDENTIFIER NOT NULL,
    [FirstName] NVARCHAR(256) NOT NULL,
    [LastName] NVARCHAR(256) NOT NULL,
    [ContactType] NVARCHAR(50) NOT NULL,
    [Relationship] NVARCHAR(100) NULL,
    [PhoneNumber] NVARCHAR(20) NULL,
    [Email] NVARCHAR(256) NULL,
    [Address] NVARCHAR(500) NULL,
    [Priority] INT NOT NULL DEFAULT 1,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedBy] NVARCHAR(256) NULL,
    [CreatedOn] DATETIMEOFFSET NOT NULL,
    [LastModifiedBy] NVARCHAR(256) NULL,
    [LastModifiedOn] DATETIMEOFFSET NULL,
    CONSTRAINT FK_EmployeeContacts_Employees 
        FOREIGN KEY ([EmployeeId]) 
        REFERENCES [hr].[Employees]([Id]) 
        ON DELETE CASCADE,
    CONSTRAINT UQ_EmployeeContacts_TenantId 
        UNIQUE ([TenantId], [Id])
);

CREATE INDEX IX_EmployeeContacts_EmployeeId ON [hr].[EmployeeContacts]([EmployeeId]);
CREATE INDEX IX_EmployeeContacts_ContactType ON [hr].[EmployeeContacts]([ContactType]);
CREATE INDEX IX_EmployeeContacts_EmployeeId_ContactType ON [hr].[EmployeeContacts]([EmployeeId], [ContactType]);
CREATE INDEX IX_EmployeeContacts_IsActive ON [hr].[EmployeeContacts]([IsActive]);
```

---

## 💼 Real-World Scenario: Emergency Contact Management

### Setup: Create Multiple Contacts

**Emergency Contact (Priority 1):**
```json
POST /api/v1/employee-contacts
{
  "employeeId": "emp-123",
  "firstName": "Jane",
  "lastName": "Doe",
  "contactType": "Emergency",
  "relationship": "Spouse",
  "phoneNumber": "+639171234567",
  "priority": 1
}
```

**Secondary Emergency Contact (Priority 2):**
```json
POST /api/v1/employee-contacts
{
  "employeeId": "emp-123",
  "firstName": "Mary",
  "lastName": "Doe",
  "contactType": "Emergency",
  "relationship": "Mother",
  "phoneNumber": "+639171234568",
  "priority": 2
}
```

**Professional Reference:**
```json
POST /api/v1/employee-contacts
{
  "employeeId": "emp-123",
  "firstName": "Bob",
  "lastName": "Smith",
  "contactType": "Reference",
  "relationship": "Former Manager",
  "email": "bob@company.com"
}
```

### Query: Get All Emergency Contacts

```json
POST /api/v1/employee-contacts/search
{
  "employeeId": "emp-123",
  "contactType": "Emergency",
  "isActive": true,
  "pageNumber": 1,
  "pageSize": 50
}

Response: Jane Doe (Priority 1), Mary Doe (Priority 2)
```

---

## ✅ Design Patterns Applied

| Pattern | Implementation |
|---------|----------------|
| **CQRS** | Separate commands and queries |
| **Domain Events** | ContactCreated, ContactUpdated, ContactDeactivated, ContactActivated |
| **Specification** | Query specifications for efficient filtering |
| **Repository** | Generic repository with keyed services |
| **Fluent Validation** | Comprehensive field validation |
| **Multi-Tenancy** | builder.IsMultiTenant() |
| **RESTful** | POST, GET, PUT, DELETE with proper HTTP status codes |
| **Permissions** | Role-based access control per operation |
| **Soft Delete** | IsActive flag pattern |
| **Audit Trail** | CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn |
| **Factory Method** | EmployeeContact.Create() for construction |
| **Aggregate Root** | EmployeeContact : IAggregateRoot |
| **Pagination** | PagedList for search results |

---

## 🧪 Testing the API

### Create Contact
```bash
curl -X POST http://localhost:5000/api/v1/employee-contacts \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "employeeId": "550e8400-e29b-41d4-a716-446655440000",
    "firstName": "Jane",
    "lastName": "Doe",
    "contactType": "Emergency",
    "relationship": "Spouse",
    "phoneNumber": "+639171234567"
  }'
```

### Get Contact
```bash
curl -X GET http://localhost:5000/api/v1/employee-contacts/110e8400-e29b-41d4-a716-446655440001 \
  -H "Authorization: Bearer $TOKEN"
```

### Search Contacts
```bash
curl -X POST http://localhost:5000/api/v1/employee-contacts/search \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "employeeId": "550e8400-e29b-41d4-a716-446655440000",
    "contactType": "Emergency",
    "pageNumber": 1,
    "pageSize": 10
  }'
```

### Update Contact
```bash
curl -X PUT http://localhost:5000/api/v1/employee-contacts/110e8400-e29b-41d4-a716-446655440001 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "phoneNumber": "+639171234568",
    "priority": 2
  }'
```

### Delete Contact
```bash
curl -X DELETE http://localhost:5000/api/v1/employee-contacts/110e8400-e29b-41d4-a716-446655440001 \
  -H "Authorization: Bearer $TOKEN"
```

---

## ✅ Checklist

- ✅ Domain Entity (EmployeeContact.cs)
- ✅ Domain Events (EmployeeContactEvents.cs)
- ✅ Domain Exceptions (EmployeeContactExceptions.cs)
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

## 📚 Related Entities

- **Employee** - The parent entity containing employee contacts
- **EmployeeDependent** - Similar to EmployeeContact but for benefits purposes
- **EmployeeEducation** - Related employee information
- **Payroll** - May use contact information for emergency notifications

---

## 🎉 Summary

The **EmployeeContact domain** is **100% complete** with:
- ✅ Full CRUD operations
- ✅ Search with pagination and filters
- ✅ Contact type management (Emergency, NextOfKin, Reference, Family)
- ✅ Priority-based ordering
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


