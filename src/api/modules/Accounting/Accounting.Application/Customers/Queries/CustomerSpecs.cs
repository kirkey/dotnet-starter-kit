namespace Accounting.Application.Customers.Queries;

/// <summary>
/// Specification to find customer by customer number.
/// </summary>
public class CustomerByNumberSpec : Specification<Customer>
{
    public CustomerByNumberSpec(string customerNumber)
    {
        Query.Where(c => c.CustomerNumber == customerNumber);
    }
}

/// <summary>
/// Specification to find customer by ID.
/// </summary>
public class CustomerByIdSpec : Specification<Customer>
{
    public CustomerByIdSpec(DefaultIdType id)
    {
        Query.Where(c => c.Id == id);
    }
}

/// <summary>
/// Specification for searching customers with filters.
/// </summary>
public class CustomerSearchSpec : Specification<Customer>
{
    public CustomerSearchSpec(
        string? customerNumber = null,
        string? customerName = null,
        string? customerType = null,
        string? status = null,
        bool? isActive = null,
        bool? isOnCreditHold = null,
        bool? taxExempt = null)
    {
        if (!string.IsNullOrWhiteSpace(customerNumber))
        {
            Query.Where(c => c.CustomerNumber.Contains(customerNumber));
        }

        if (!string.IsNullOrWhiteSpace(customerName))
        {
            Query.Where(c => c.CustomerName.Contains(customerName));
        }

        if (!string.IsNullOrWhiteSpace(customerType))
        {
            Query.Where(c => c.CustomerType == customerType);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            Query.Where(c => c.Status == status);
        }

        if (isActive.HasValue)
        {
            Query.Where(c => c.IsActive == isActive.Value);
        }

        if (isOnCreditHold.HasValue)
        {
            Query.Where(c => c.IsOnCreditHold == isOnCreditHold.Value);
        }

        if (taxExempt.HasValue)
        {
            Query.Where(c => c.TaxExempt == taxExempt.Value);
        }

        Query.OrderBy(c => c.CustomerNumber);
    }
}

