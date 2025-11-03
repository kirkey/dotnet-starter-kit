namespace Accounting.Application.Customers.Search.v1;

/// <summary>
/// Specification for searching customers with pagination and filtering.
/// </summary>
public class CustomerSearchSpecs : EntitiesByPaginationFilterSpec<Customer, CustomerSearchResponse>
{
    /// <summary>
    /// Creates a specification for searching customers with optional filters.
    /// Supports filtering by customer number, name, type, status, active state, credit hold, and tax exempt status.
    /// </summary>
    public CustomerSearchSpecs(CustomerSearchQuery request)
        : base(request)
    {
        // Filter by customer number (partial match)
        Query.Where(c => request.CustomerNumber == null || c.CustomerNumber.Contains(request.CustomerNumber), request.CustomerNumber != null);

        // Filter by customer name (partial match)
        Query.Where(c => request.CustomerName == null || c.CustomerName.Contains(request.CustomerName), request.CustomerName != null);

        // Filter by customer type (exact match)
        Query.Where(c => request.CustomerType == null || c.CustomerType == request.CustomerType, request.CustomerType != null);

        // Filter by status (exact match)
        Query.Where(c => request.Status == null || c.Status == request.Status, request.Status != null);

        // Filter by active state
        Query.Where(c => request.IsActive == null || c.IsActive == request.IsActive, request.IsActive != null);

        // Filter by credit hold state
        Query.Where(c => request.IsOnCreditHold == null || c.IsOnCreditHold == request.IsOnCreditHold, request.IsOnCreditHold != null);

        // Filter by tax exempt status
        Query.Where(c => request.TaxExempt == null || c.TaxExempt == request.TaxExempt, request.TaxExempt != null);

        // Apply keyword search across multiple fields if Keyword is provided
        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            Query.Search(c => c.CustomerNumber, $"%{request.Keyword}%")
                 .Search(c => c.CustomerName, $"%{request.Keyword}%")
                 .Search(c => c.Email, $"%{request.Keyword}%")
                 .Search(c => c.Phone, $"%{request.Keyword}%");
        }

        // Default ordering
        Query.OrderBy(c => c.CustomerName, !request.OrderBy.Any());
    }
}

