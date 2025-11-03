# Customers Module - Paginated Search Implementation

## Overview
This document describes the implementation of paginated search for the Customers module in the Accounting system. The Customer entity now has proper pagination support in both the backend API and Blazor client.

---

## Backend API Changes

### 1. New Files Created

#### `/Accounting.Application/Customers/Search/v1/CustomerSearchQuery.cs`
- Paginated search query with filtering support
- Extends `PaginationFilter` 
- Filter properties:
  - `CustomerNumber` - Partial match search
  - `CustomerName` - Partial match search
  - `CustomerType` - Exact match (Individual, Business, Government, etc.)
  - `Status` - Exact match (Active, Inactive, CreditHold, etc.)
  - `IsActive` - Boolean filter
  - `IsOnCreditHold` - Boolean filter
  - `TaxExempt` - Boolean filter
- Returns: `PagedList<CustomerSearchResponse>`

#### `/Accounting.Application/Customers/Search/v1/CustomerSearchResponse.cs`
- DTO for paginated search results
- Contains essential customer information:
  - Customer identification (Id, CustomerNumber, CustomerName)
  - Contact info (Email, Phone, BillingAddress)
  - Financial info (CreditLimit, CurrentBalance, AvailableCredit)
  - Status fields (Status, IsActive, IsOnCreditHold, TaxExempt)
  - Additional fields (CustomerType, PaymentTerms, Description, Notes)

#### `/Accounting.Application/Customers/Search/v1/CustomerSearchSpecs.cs`
- Specification for querying customers with filters
- Extends `EntitiesByPaginationFilterSpec<Customer, CustomerSearchResponse>`
- Implements:
  - Filtering by all query properties
  - Keyword search across CustomerNumber, CustomerName, Email, Phone
  - Default ordering by CustomerName

#### `/Accounting.Application/Customers/Search/v1/CustomerSearchHandler.cs`
- MediatR handler for `CustomerSearchQuery`
- Implements pagination logic:
  - Uses `CustomerSearchSpecs` for filtering
  - Retrieves filtered list and total count
  - Returns `PagedList<CustomerSearchResponse>`

### 2. Modified Files

#### `/Accounting.Infrastructure/Endpoints/Customers/v1/CustomerSearchEndpoint.cs`
- Updated to use `CustomerSearchQuery` instead of `SearchCustomersRequest`
- Now returns `PagedList<CustomerSearchResponse>`
- Supports pagination parameters (PageNumber, PageSize)
- Maintains backward compatibility

---

## Blazor Client Changes

### 1. Modified Files

#### `/Pages/Accounting/Customers/Customers.razor`
- Updated `TEntity` from `CustomerDto` to `CustomerSearchResponse`
- Form fields remain unchanged (all customer properties)

#### `/Pages/Accounting/Customers/Customers.razor.cs`
- Updated entity type to `CustomerSearchResponse`
- Updated search function to use `CustomerSearchQuery`:
  ```csharp
  var paginationFilter = filter.Adapt<CustomerSearchQuery>();
  var result = await Client.CustomerSearchEndpointAsync("1", paginationFilter);
  return result.Adapt<PaginationResponse<CustomerSearchResponse>>();
  ```
- Removed manual pagination logic (now handled by API)

---

## API Endpoint

### POST `/api/accounting/customers/search`

**Request Body** (`CustomerSearchQuery`):
```json
{
  "pageNumber": 1,
  "pageSize": 10,
  "keyword": "search term",
  "customerNumber": "CUST-001",
  "customerName": "ABC Corp",
  "customerType": "Business",
  "status": "Active",
  "isActive": true,
  "isOnCreditHold": false,
  "taxExempt": false,
  "orderBy": ["CustomerName"],
  "sortOrder": "asc"
}
```

**Response** (`PagedList<CustomerSearchResponse>`):
```json
{
  "data": [
    {
      "id": "guid",
      "customerNumber": "CUST-001",
      "customerName": "ABC Corporation",
      "customerType": "Business",
      "email": "billing@abc.com",
      "phone": "(555) 123-4567",
      "billingAddress": "123 Main St",
      "creditLimit": 50000.00,
      "currentBalance": 12500.50,
      "availableCredit": 37499.50,
      "status": "Active",
      "taxExempt": false,
      "paymentTerms": "Net 30",
      "isActive": true,
      "isOnCreditHold": false,
      "description": "Major client",
      "notes": "VIP customer"
    }
  ],
  "totalCount": 150,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 15
}
```

---

## Features

### ✅ Pagination
- Server-side pagination
- Configurable page size
- Page navigation support

### ✅ Filtering
- Customer number (partial match)
- Customer name (partial match)
- Customer type (exact match)
- Status (exact match)
- Active state
- Credit hold state
- Tax exempt status

### ✅ Keyword Search
- Searches across multiple fields:
  - Customer Number
  - Customer Name
  - Email
  - Phone

### ✅ Sorting
- Default sort by Customer Name
- Configurable sort fields
- Ascending/descending order

---

## Next Steps

### Required Actions

1. **Regenerate API Client**
   - Run NSwag to generate updated API client
   - This will add `CustomerSearchQuery` and `CustomerSearchResponse` types
   - Command (from project root):
     ```bash
     # Navigate to scripts location and run NSwag
     cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/apps/blazor/scripts
     ./generate-api-client.sh  # or .ps1 on Windows
     ```

2. **Test Backend API**
   - Start the API server
   - Test the search endpoint with Swagger/Postman
   - Verify pagination works correctly
   - Test all filter combinations

3. **Test Blazor Client**
   - After API client regeneration
   - Start the Blazor app
   - Navigate to `/accounting/customers`
   - Test CRUD operations
   - Test search and pagination

### Optional Enhancements

1. **Add Validation**
   - Create `CustomerSearchQueryValidator`
   - Validate page numbers, page sizes
   - Validate filter values

2. **Add Caching**
   - Consider caching frequently searched results
   - Implement cache invalidation on updates

3. **Add Export**
   - Export filtered results to Excel/CSV
   - Respect current filters

4. **Add Advanced Filters**
   - Date range filters (AccountOpenDate)
   - Balance range filters
   - Multiple customer type selection

---

## Migration Notes

### Breaking Changes
- ❌ Old `SearchCustomersRequest` endpoint still exists but deprecated
- ✅ New `CustomerSearchQuery` endpoint is now the recommended approach
- ⚠️ API clients must be regenerated to use new types

### Backward Compatibility
- The old non-paginated search endpoint (`SearchCustomersRequest`) remains functional
- Recommended to migrate to paginated search for better performance
- Large customer lists will perform better with pagination

---

## Performance Considerations

### Before (Non-Paginated)
- Loaded ALL customers into memory
- Client-side pagination
- Poor performance with large datasets
- High memory usage

### After (Paginated)
- Only loads requested page
- Server-side pagination
- Excellent performance with any dataset size
- Minimal memory footprint

### Database Optimization
- Ensure indexes exist on:
  - `CustomerNumber`
  - `CustomerName`
  - `Status`
  - `IsActive`
  - `CustomerType`

---

## Testing Checklist

### Backend API
- [ ] Create customer succeeds
- [ ] Update customer succeeds
- [ ] Get customer by ID succeeds
- [ ] Search without filters returns paginated results
- [ ] Search with customer number filter works
- [ ] Search with customer name filter works
- [ ] Search with type filter works
- [ ] Search with status filter works
- [ ] Search with boolean filters works
- [ ] Keyword search works across multiple fields
- [ ] Pagination works (next/previous pages)
- [ ] Sorting works
- [ ] Empty results handled correctly
- [ ] Invalid page numbers handled correctly

### Blazor Client
- [ ] Page loads without errors
- [ ] Table displays customers correctly
- [ ] Create customer modal opens
- [ ] Create customer saves successfully
- [ ] Edit customer modal opens with data
- [ ] Update customer saves successfully
- [ ] Delete disabled (as designed)
- [ ] Search box filters results
- [ ] Pagination controls work
- [ ] Page size selector works
- [ ] Column sorting works
- [ ] Advanced search works (if enabled)

---

## Documentation

### API Documentation
- Swagger/OpenAPI docs automatically updated
- Endpoint available at: `/swagger`
- Search for "Customers" endpoints

### Code Comments
- All classes have XML documentation
- All public methods documented
- All properties documented
- Usage examples included

---

## Support

### Common Issues

**Issue**: API client doesn't have CustomerSearchQuery  
**Solution**: Regenerate the API client using NSwag

**Issue**: Pagination not working  
**Solution**: Ensure PageNumber >= 1 and PageSize > 0

**Issue**: No results returned  
**Solution**: Check filters, verify data exists in database

**Issue**: Performance slow  
**Solution**: Check database indexes, reduce page size

---

**Status**: ✅ Backend Implementation Complete  
**Status**: ⏳ Awaiting API Client Regeneration  
**Next Action**: Regenerate NSwag API client  
**Date**: November 3, 2025

