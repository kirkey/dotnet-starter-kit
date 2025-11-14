# Employee Contacts, Dependents & Documents - Usage Guide

## Quick Reference

### Employee Contacts API

#### Create Contact
```csharp
var command = new CreateEmployeeContactCommand(
    EmployeeId: employeeId,
    FirstName: "Jane",
    LastName: "Smith",
    ContactType: "Emergency",    // Emergency, NextOfKin, Reference, Family
    Relationship: "Spouse",
    PhoneNumber: "+1234567890",
    Email: "jane.smith@example.com",
    Address: "123 Main St"
);
var response = await mediator.Send(command);
// Returns: CreateEmployeeContactResponse with contact Id
```

#### Search Contacts
```csharp
var request = new SearchEmployeeContactsRequest
{
    PageNumber = 1,
    PageSize = 10,
    SearchString = "Jane",                    // Optional: search by name/phone
    EmployeeId = employeeId,                  // Optional: filter by employee
    ContactType = "Emergency",                // Optional: filter by type
    IsActive = true                           // Optional: filter by status
};
var result = await mediator.Send(request);
// Returns: PagedList<EmployeeContactResponse>
```

#### Get Single Contact
```csharp
var request = new GetEmployeeContactRequest(contactId);
var response = await mediator.Send(request);
// Returns: EmployeeContactResponse with full details
```

#### Update Contact
```csharp
var command = new UpdateEmployeeContactCommand(
    Id: contactId,
    FirstName: "Jane",                        // Optional
    LastName: "Smith",                        // Optional
    Relationship: "Wife",                     // Optional
    PhoneNumber: "+1987654321",               // Optional
    Email: "jane.new@example.com",            // Optional
    Address: "456 Oak Ave",                   // Optional
    Priority: 1                               // Optional: set emergency order
);
var response = await mediator.Send(command);
// Returns: UpdateEmployeeContactResponse with contact Id
```

#### Delete Contact
```csharp
var command = new DeleteEmployeeContactCommand(contactId);
var response = await mediator.Send(command);
// Returns: DeleteEmployeeContactResponse with contact Id
```

---

### Employee Dependents API

#### Create Dependent
```csharp
var command = new CreateEmployeeDependentCommand(
    EmployeeId: employeeId,
    FirstName: "Jack",
    LastName: "Doe",
    DependentType: "Child",                   // Spouse, Child, Parent, Sibling, Other
    DateOfBirth: new DateTime(2015, 3, 20),
    Relationship: "Biological Child",         // Optional
    Email: "jack@example.com",                // Optional
    PhoneNumber: null                         // Optional
);
var response = await mediator.Send(command);
// Returns: CreateEmployeeDependentResponse with dependent Id
```

#### Search Dependents
```csharp
var request = new SearchEmployeeDependentsRequest
{
    PageNumber = 1,
    PageSize = 10,
    SearchString = "Jack",                    // Optional: search by name
    EmployeeId = employeeId,                  // Optional: filter by employee
    DependentType = "Child",                  // Optional: filter by type
    IsBeneficiary = true,                     // Optional: filter by beneficiary
    IsClaimableDependent = true,              // Optional: filter by tax status
    IsActive = true                           // Optional: filter by status
};
var result = await mediator.Send(request);
// Returns: PagedList<EmployeeDependentResponse>
```

#### Get Single Dependent
```csharp
var request = new GetEmployeeDependentRequest(dependentId);
var response = await mediator.Send(request);
// Returns: EmployeeDependentResponse with age calculated
```

#### Update Dependent
```csharp
var command = new UpdateEmployeeDependentCommand(
    Id: dependentId,
    FirstName: "Jack",                        // Optional
    LastName: "Doe",                          // Optional
    Relationship: "Biological Child",         // Optional
    Email: "jack.new@example.com",            // Optional
    PhoneNumber: null,                        // Optional
    IsBeneficiary: true,                      // Optional: set beneficiary status
    IsClaimableDependent: true                // Optional: set tax claimable
);
var response = await mediator.Send(command);
// Returns: UpdateEmployeeDependentResponse with dependent Id
```

#### Delete Dependent
```csharp
var command = new DeleteEmployeeDependentCommand(dependentId);
var response = await mediator.Send(command);
// Returns: DeleteEmployeeDependentResponse with dependent Id
```

---

### Employee Documents API

#### Create Document
```csharp
var command = new CreateEmployeeDocumentCommand(
    EmployeeId: employeeId,
    DocumentType: "Contract",                 // Contract, Certification, License, Identity, Medical, Other
    Title: "Employment Contract",
    FileName: "contract_2024.pdf",            // Optional
    FilePath: "/documents/contracts/emp123",  // Optional
    FileSize: 512000,                         // Optional: bytes
    ExpiryDate: new DateTime(2027, 12, 31),  // Optional
    IssueNumber: "CONTRACT-2024-001",         // Optional
    IssueDate: new DateTime(2024, 1, 1),     // Optional
    Notes: "Original signed contract"         // Optional
);
var response = await mediator.Send(command);
// Returns: CreateEmployeeDocumentResponse with document Id
```

#### Search Documents
```csharp
var request = new SearchEmployeeDocumentsRequest
{
    PageNumber = 1,
    PageSize = 10,
    SearchString: "contract",                 // Optional: search by title/filename
    EmployeeId: employeeId,                   // Optional: filter by employee
    DocumentType: "Contract",                 // Optional: filter by type
    IsExpired: false,                         // Optional: filter expired docs
    IsActive: true                            // Optional: filter by status
};
var result = await mediator.Send(request);
// Returns: PagedList<EmployeeDocumentResponse>
```

#### Get Single Document
```csharp
var request = new GetEmployeeDocumentRequest(documentId);
var response = await mediator.Send(request);
// Returns: EmployeeDocumentResponse with expiry calculations
```

#### Update Document
```csharp
var command = new UpdateEmployeeDocumentCommand(
    Id: documentId,
    Title: "Employment Contract 2024",        // Optional
    ExpiryDate: new DateTime(2028, 12, 31),  // Optional
    IssueNumber: "CONTRACT-2024-001",         // Optional
    IssueDate: new DateTime(2024, 6, 1),     // Optional
    Notes: "Updated notes about contract"     // Optional
);
var response = await mediator.Send(command);
// Returns: UpdateEmployeeDocumentResponse with document Id
```

#### Delete Document
```csharp
var command = new DeleteEmployeeDocumentCommand(documentId);
var response = await mediator.Send(command);
// Returns: DeleteEmployeeDocumentResponse with document Id
```

---

## Validation Rules

### EmployeeContact Validations
- **EmployeeId**: Required, must exist
- **FirstName**: Required, max 100 characters
- **LastName**: Required, max 100 characters  
- **ContactType**: Required, must be valid type
- **PhoneNumber**: Max 20 chars, valid phone format
- **Email**: Valid email format if provided
- **Address**: Max 250 characters
- **Priority**: Must be > 0

### EmployeeDependent Validations
- **EmployeeId**: Required, must exist
- **FirstName**: Required, max 100 characters
- **LastName**: Required, max 100 characters
- **DependentType**: Required, must be valid type
- **DateOfBirth**: Required, cannot be in future
- **Email**: Valid email format if provided
- **PhoneNumber**: Max 20 chars, valid phone format

### EmployeeDocument Validations
- **EmployeeId**: Required, must exist
- **DocumentType**: Required, must be valid type
- **Title**: Required, max 250 characters
- **FileName**: Max 255 characters
- **FilePath**: Max 500 characters
- **FileSize**: Must be > 0 if provided
- **ExpiryDate**: Must be in future if provided
- **Notes**: Max 1000 characters

---

## Common Use Cases

### 1. Add Emergency Contact
```csharp
// Create emergency contact with highest priority
var contact = await mediator.Send(new CreateEmployeeContactCommand(
    employeeId,
    "Emergency Contact Name",
    "Last Name",
    "Emergency",
    "Spouse",
    "+1234567890"
));

// Set as first priority
await mediator.Send(new UpdateEmployeeContactCommand(
    contact.Id,
    Priority: 1
));
```

### 2. Track Employee Family for Benefits
```csharp
// Add spouse
var spouse = await mediator.Send(new CreateEmployeeDependentCommand(
    employeeId,
    "Jane",
    "Doe",
    "Spouse",
    new DateTime(1990, 5, 15)
));

// Mark as beneficiary
await mediator.Send(new UpdateEmployeeDependentCommand(
    spouse.Id,
    IsBeneficiary: true,
    IsClaimableDependent: true
));

// Add children
var child1 = await mediator.Send(new CreateEmployeeDependentCommand(
    employeeId,
    "Jack",
    "Doe",
    "Child",
    new DateTime(2015, 3, 20)
));
```

### 3. Manage Employee Documents
```csharp
// Create employment contract
var contract = await mediator.Send(new CreateEmployeeDocumentCommand(
    employeeId,
    "Contract",
    "Employment Contract",
    "contract.pdf",
    "/docs/contracts/emp123",
    512000,
    new DateTime(2027, 12, 31),
    "CONTRACT-2024-001",
    new DateTime(2024, 1, 1),
    "Signed employment contract"
));

// Later, update with renewal
await mediator.Send(new UpdateEmployeeDocumentCommand(
    contract.Id,
    ExpiryDate: new DateTime(2028, 12, 31),
    Notes: "Contract renewed for another year"
));
```

### 4. Find Expiring Documents
```csharp
// Search for documents expiring within 30 days
var documents = await mediator.Send(new SearchEmployeeDocumentsRequest
{
    PageNumber = 1,
    PageSize = 100,
    IsExpired = false,                         // Get non-expired docs
    IsActive = true
});

// Filter in memory for those expiring soon
var expiringIn30Days = documents.Items
    .Where(d => d.DaysUntilExpiry.HasValue && d.DaysUntilExpiry <= 30)
    .OrderBy(d => d.DaysUntilExpiry)
    .ToList();
```

### 5. Search All Emergency Contacts
```csharp
var emergencyContacts = await mediator.Send(new SearchEmployeeContactsRequest
{
    PageNumber = 1,
    PageSize = 1000,
    ContactType: "Emergency",
    IsActive: true
});

// Get ordered by priority
var orderedByPriority = emergencyContacts.Items
    .OrderBy(c => c.Priority)
    .ToList();
```

---

## Error Handling

### Common Exceptions

```csharp
// Employee not found
throw new EmployeeNotFoundException(employeeId);

// Contact not found  
throw new EmployeeContactNotFoundException(contactId);

// Dependent not found
throw new EmployeeDependentNotFoundException(dependentId);

// Document not found
throw new EmployeeDocumentNotFoundException(documentId);

// Validation errors - thrown by FluentValidation
// Check ValidationException.Errors collection
```

---

## Database Queries Generated

### Search EmployeeContacts with Filters
```sql
SELECT * FROM EmployeeContacts
WHERE EmployeeId = @employeeId
  AND (FirstName LIKE '%@search%' OR LastName LIKE '%@search%' OR PhoneNumber LIKE '%@search%')
  AND ContactType = @contactType
  AND IsActive = @isActive
ORDER BY Priority, FirstName
LIMIT @pageSize OFFSET @skip
```

### Search EmployeeDependents with Expiry Check
```sql
SELECT * FROM EmployeeDependents
WHERE EmployeeId = @employeeId
  AND (FirstName LIKE '%@search%' OR LastName LIKE '%@search%')
  AND DependentType = @dependentType
  AND IsBeneficiary = @isBeneficiary
  AND IsClaimableDependent = @isClaimable
  AND IsActive = @isActive
ORDER BY FirstName, LastName
LIMIT @pageSize OFFSET @skip
```

### Find Expired Documents
```sql
SELECT * FROM EmployeeDocuments
WHERE EmployeeId = @employeeId
  AND ExpiryDate < GETDATE()
  AND IsActive = @isActive
ORDER BY ExpiryDate DESC
```

---

## Response Examples

### EmployeeContactResponse
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "employeeId": "550e8400-e29b-41d4-a716-446655440001",
  "firstName": "Jane",
  "lastName": "Smith",
  "fullName": "Jane Smith",
  "contactType": "Emergency",
  "relationship": "Spouse",
  "phoneNumber": "+1234567890",
  "email": "jane.smith@example.com",
  "address": "123 Main St",
  "priority": 1,
  "isActive": true
}
```

### EmployeeDependentResponse
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440002",
  "employeeId": "550e8400-e29b-41d4-a716-446655440001",
  "firstName": "Jack",
  "lastName": "Doe",
  "fullName": "Jack Doe",
  "dependentType": "Child",
  "dateOfBirth": "2015-03-20",
  "age": 9,
  "relationship": "Biological Child",
  "email": null,
  "phoneNumber": null,
  "isBeneficiary": true,
  "isClaimableDependent": true,
  "eligibilityEndDate": null,
  "isActive": true
}
```

### EmployeeDocumentResponse
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440003",
  "employeeId": "550e8400-e29b-41d4-a716-446655440001",
  "documentType": "Contract",
  "title": "Employment Contract",
  "fileName": "contract_2024.pdf",
  "filePath": "/documents/contracts/emp123",
  "fileSize": 512000,
  "expiryDate": "2027-12-31",
  "isExpired": false,
  "daysUntilExpiry": 1342,
  "issueNumber": "CONTRACT-2024-001",
  "issueDate": "2024-01-01",
  "uploadedDate": "2024-01-02",
  "version": 1,
  "notes": "Original signed contract",
  "isActive": true
}
```

---

**Last Updated:** November 14, 2025  
**Status:** ✅ Implementation Complete

