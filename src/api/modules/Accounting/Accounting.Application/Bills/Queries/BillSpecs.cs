using Accounting.Domain.Entities;

namespace Accounting.Application.Bills.Queries;

/// <summary>
/// Specification to find bill by bill number.
/// </summary>
public class BillByNumberSpec : Specification<Bill>
{
    public BillByNumberSpec(string billNumber)
    {
        Query.Where(b => b.BillNumber == billNumber);
    }
}

/// <summary>
/// Specification to find bill by ID with related data.
/// </summary>
public class BillByIdSpec : Specification<Bill>
{
    public BillByIdSpec(DefaultIdType id)
    {
        Query.Where(b => b.Id == id);
    }
}

/// <summary>
/// Specification for searching bills with filters.
/// </summary>
public class BillSearchSpec : Specification<Bill>
{
    public BillSearchSpec(
        string? billNumber = null,
        DefaultIdType? vendorId = null,
        string? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        bool? isOverdue = null)
    {
        if (!string.IsNullOrWhiteSpace(billNumber))
        {
            Query.Where(b => b.BillNumber.Contains(billNumber));
        }

        if (vendorId.HasValue)
        {
            Query.Where(b => b.VendorId == vendorId.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            Query.Where(b => b.Status == status);
        }

        if (fromDate.HasValue)
        {
            Query.Where(b => b.BillDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            Query.Where(b => b.BillDate <= toDate.Value);
        }

        if (isOverdue.HasValue && isOverdue.Value)
        {
            var today = DateTime.UtcNow.Date;
            Query.Where(b => b.DueDate < today && b.Status != "Paid" && b.Status != "Void");
        }

        Query.OrderByDescending(b => b.BillDate);
    }
}

