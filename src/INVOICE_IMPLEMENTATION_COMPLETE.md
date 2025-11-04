# Invoice and InvoiceLineItem Implementation Complete

## Overview
Comprehensive implementation of Invoice and InvoiceLineItem features for the Accounting module following CQRS principles and consistent code patterns from Catalog and Todo modules.

## Features Implemented

### Invoice Features

#### CRUD Operations
1. **Create Invoice** (`CreateInvoiceCommand`)
   - Full validation with `CreateInvoiceCommandValidator`
   - Creates invoice with all utility billing fields
   - Endpoint: `POST /accounting/invoices`

2. **Update Invoice** (`UpdateInvoiceCommand`)
   - Partial update support (only provided fields updated)
   - Validation prevents updating paid invoices
   - Endpoint: `PUT /accounting/invoices/{id}`

3. **Get Invoice** (`GetInvoiceByIdQuery`)
   - Returns complete invoice details
   - Includes line item count
   - Endpoint: `GET /accounting/invoices/{id}`

4. **Search Invoices** (`SearchInvoicesCommand`)
   - Advanced filtering by:
     - Member ID
     - Invoice number (partial match)
     - Status
     - Date ranges (invoice date, due date)
     - Billing period
     - Consumption ID
     - Rate schedule
     - Amount range (min/max)
     - Outstanding balance flag
   - Pagination support
   - Endpoint: `POST /accounting/invoices/search`

5. **Delete Invoice** (`DeleteInvoiceCommand`)
   - Only allows deletion of Draft or Cancelled invoices
   - Validation with `DeleteInvoiceCommandValidator`
   - Endpoint: `DELETE /accounting/invoices/{id}`

#### Workflow Operations
6. **Send Invoice** (`SendInvoiceCommand`)
   - Transitions from Draft to Sent status
   - Endpoint: `POST /accounting/invoices/{id}/send`

7. **Mark as Paid** (`MarkInvoiceAsPaidCommand`)
   - Marks invoice as fully paid
   - Records payment date and method
   - Endpoint: `POST /accounting/invoices/{id}/mark-paid`

8. **Apply Payment** (`ApplyInvoicePaymentCommand`)
   - Applies partial payment
   - Auto-marks as paid when total payments meet invoice amount
   - Validation: amount must be positive, date cannot be future
   - Endpoint: `POST /accounting/invoices/{id}/apply-payment`

9. **Cancel Invoice** (`CancelInvoiceCommand`)
   - Cancels unpaid invoice
   - Optional cancellation reason
   - Endpoint: `POST /accounting/invoices/{id}/cancel`

10. **Void Invoice** (`VoidInvoiceCommand`)
    - Voids invoice to reverse accounting impact
    - Maintains audit trail
    - Optional void reason
    - Endpoint: `POST /accounting/invoices/{id}/void`

### Invoice Line Item Features

1. **Add Line Item** (`AddInvoiceLineItemCommand`)
   - Adds line item to invoice
   - Automatically updates invoice total
   - Validation: description required (max 500 chars), quantity > 0, unit price >= 0
   - Endpoint: `POST /accounting/invoices/{invoiceId}/line-items`

2. **Get Line Item** (`GetInvoiceLineItemRequest`)
   - Retrieves specific line item by ID
   - Endpoint: `GET /accounting/invoices/line-items/{lineItemId}`

3. **Get Line Items** (`GetInvoiceLineItemsRequest`)
   - Retrieves all line items for an invoice
   - Ordered by creation date
   - Endpoint: `GET /accounting/invoices/{invoiceId}/line-items`

4. **Update Line Item** (`UpdateInvoiceLineItemCommand`)
   - Partial update support
   - Recalculates total price automatically
   - Validation for all fields
   - Endpoint: `PUT /accounting/invoices/line-items/{lineItemId}`

5. **Delete Line Item** (`DeleteInvoiceLineItemCommand`)
   - Deletes line item
   - Recalculates invoice totals
   - Endpoint: `DELETE /accounting/invoices/line-items/{lineItemId}`

## File Structure

### Application Layer
```
Accounting.Application/Invoices/
├── Commands/
│   ├── CreateInvoiceCommand.cs
│   └── CreateInvoiceCommandValidator.cs
├── Queries/
│   └── GetInvoiceByIdQuery.cs
├── Handlers/
│   ├── CreateInvoiceHandler.cs
│   └── GetInvoiceByIdHandler.cs
├── Responses/
│   └── InvoiceResponse.cs
├── Update/v1/
│   ├── UpdateInvoiceCommand.cs
│   ├── UpdateInvoiceCommandValidator.cs
│   ├── UpdateInvoiceHandler.cs
│   └── UpdateInvoiceResponse.cs
├── Search/v1/
│   ├── SearchInvoicesCommand.cs
│   ├── SearchInvoicesSpec.cs
│   └── SearchInvoicesHandler.cs
├── Delete/v1/
│   ├── DeleteInvoiceCommand.cs
│   ├── DeleteInvoiceCommandValidator.cs
│   ├── DeleteInvoiceHandler.cs
│   └── DeleteInvoiceResponse.cs
├── Send/v1/
│   ├── SendInvoiceCommand.cs
│   ├── SendInvoiceHandler.cs
│   └── SendInvoiceResponse.cs
├── MarkPaid/v1/
│   ├── MarkInvoiceAsPaidCommand.cs
│   ├── MarkInvoiceAsPaidHandler.cs
│   └── MarkInvoiceAsPaidResponse.cs
├── ApplyPayment/v1/
│   ├── ApplyInvoicePaymentCommand.cs
│   ├── ApplyInvoicePaymentCommandValidator.cs
│   ├── ApplyInvoicePaymentHandler.cs
│   └── ApplyInvoicePaymentResponse.cs
├── Cancel/v1/
│   ├── CancelInvoiceCommand.cs
│   ├── CancelInvoiceHandler.cs
│   └── CancelInvoiceResponse.cs
├── Void/v1/
│   ├── VoidInvoiceCommand.cs
│   ├── VoidInvoiceHandlerV1.cs
│   └── VoidInvoiceResponse.cs
└── LineItems/
    ├── Add/v1/
    │   ├── AddInvoiceLineItemCommand.cs
    │   ├── AddInvoiceLineItemCommandValidator.cs
    │   ├── AddInvoiceLineItemHandler.cs
    │   └── AddInvoiceLineItemResponse.cs
    ├── Get/v1/
    │   ├── GetInvoiceLineItemRequest.cs
    │   ├── GetInvoiceLineItemHandler.cs
    │   └── InvoiceLineItemResponse.cs
    ├── GetList/v1/
    │   ├── GetInvoiceLineItemsRequest.cs
    │   ├── GetInvoiceLineItemsSpec.cs
    │   └── GetInvoiceLineItemsHandler.cs
    ├── Update/v1/
    │   ├── UpdateInvoiceLineItemCommand.cs
    │   ├── UpdateInvoiceLineItemCommandValidator.cs
    │   ├── UpdateInvoiceLineItemHandler.cs
    │   └── UpdateInvoiceLineItemResponse.cs
    └── Delete/v1/
        ├── DeleteInvoiceLineItemCommand.cs
        ├── DeleteInvoiceLineItemCommandValidator.cs
        ├── DeleteInvoiceLineItemHandler.cs
        └── DeleteInvoiceLineItemResponse.cs
```

### Infrastructure Layer - Endpoints
```
Accounting.Infrastructure/Endpoints/Invoice/
├── InvoiceEndpoints.cs (main registration)
├── v1/
│   ├── CreateInvoiceEndpoint.cs
│   ├── UpdateInvoiceEndpoint.cs
│   ├── GetInvoiceEndpoint.cs
│   ├── SearchInvoicesEndpoint.cs
│   ├── DeleteInvoiceEndpoint.cs
│   ├── SendInvoiceEndpoint.cs
│   ├── MarkInvoiceAsPaidEndpoint.cs
│   ├── ApplyInvoicePaymentEndpoint.cs
│   ├── CancelInvoiceEndpoint.cs
│   └── VoidInvoiceEndpoint.cs
└── LineItems/v1/
    ├── AddInvoiceLineItemEndpoint.cs
    ├── GetInvoiceLineItemEndpoint.cs
    ├── GetInvoiceLineItemsEndpoint.cs
    ├── UpdateInvoiceLineItemEndpoint.cs
    └── DeleteInvoiceLineItemEndpoint.cs
```

## Domain Layer Updates

### Exceptions
- Added `InvoiceNotFoundException` 
- Added `InvoiceLineItemNotFoundException`
- Existing exceptions preserved (InvoiceByIdNotFoundException, etc.)

## Key Features

### Validation
- **Strict validators** on all commands following FluentValidation patterns
- Field length restrictions (e.g., InvoiceNumber: 50, Description: 500, Notes: 1000)
- Business rule validations (e.g., due date >= invoice date, amounts >= 0)
- Payment date cannot be in future
- Quantity must be positive for line items

### Search & Filtering
- Comprehensive search with multiple filter criteria
- Pagination support with PagedList
- Partial string matching for invoice numbers
- Date range queries for invoice and due dates
- Outstanding balance filtering

### Status Management
- Draft → Sent → Paid workflow
- Overdue detection
- Cancel and Void operations with audit trail
- Paid invoices cannot be modified or deleted

### Payment Tracking
- Full payment via MarkPaid
- Partial payments via ApplyPayment
- Outstanding amount calculation
- Payment method and date recording

### Line Items
- Dynamic line item management
- Automatic total recalculation
- Optional GL account assignment
- Full CRUD operations

## Permissions Required
- `Permissions.Accounting.Create` - Create invoices and line items
- `Permissions.Accounting.View` - View and search invoices
- `Permissions.Accounting.Update` - Update invoices, apply payments, change status
- `Permissions.Accounting.Delete` - Delete invoices and line items

## API Versioning
All endpoints support API versioning with `v1.0` via Asp.Versioning.

## Response Codes
- **200 OK** - Successful GET, PUT, POST (non-creation)
- **201 Created** - Successful POST (creation)
- **400 Bad Request** - Validation errors
- **404 Not Found** - Resource not found
- **409 Conflict** - Duplicate invoice number

## Documentation
- All classes, methods, and properties include XML documentation
- Summary and param tags on all DTOs
- Business rules documented in entity and handler comments

## Code Consistency
- Follows CQRS pattern strictly
- Consistent with Catalog and Todo module patterns
- Separate files for each class (DRY principle)
- Record types for immutable DTOs
- Async/await throughout
- Proper null checking with ArgumentNullException.ThrowIfNull

## Next Steps
1. Build the solution to ensure no compilation errors
2. Run migrations if needed for InvoiceLineItem entity
3. Test all endpoints via Swagger UI
4. Add integration tests
5. Update API documentation

## Testing Recommendations
Test the following workflows:
1. Create invoice → Add line items → Send → Apply payment → Mark paid
2. Create invoice → Cancel
3. Create paid invoice → Void
4. Search invoices by various criteria
5. Partial payment workflow
6. Update line items and verify totals recalculate

