
namespace Accounting.Application.PurchaseOrders.Queries;

/// <summary>
/// Specification to find purchase order by order number.
/// </summary>
public class PurchaseOrderByNumberSpec : Specification<PurchaseOrder>
{
    public PurchaseOrderByNumberSpec(string orderNumber)
    {
        Query.Where(po => po.OrderNumber == orderNumber);
    }
}

/// <summary>
/// Specification to find purchase order by ID.
/// </summary>
public class PurchaseOrderByIdSpec : Specification<PurchaseOrder>
{
    public PurchaseOrderByIdSpec(DefaultIdType id)
    {
        Query.Where(po => po.Id == id);
    }
}

/// <summary>
/// Specification for searching purchase orders with filters.
/// </summary>
public class PurchaseOrderSearchSpec : Specification<PurchaseOrder>
{
    public PurchaseOrderSearchSpec(
        string? orderNumber = null,
        DefaultIdType? vendorId = null,
        string? status = null,
        bool? isFullyReceived = null,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        if (!string.IsNullOrWhiteSpace(orderNumber))
        {
            Query.Where(po => po.OrderNumber.Contains(orderNumber));
        }

        if (vendorId.HasValue)
        {
            Query.Where(po => po.VendorId == vendorId.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            Query.Where(po => po.Status.ToString() == status);
        }

        if (isFullyReceived.HasValue)
        {
            Query.Where(po => po.IsFullyReceived == isFullyReceived.Value);
        }

        if (fromDate.HasValue)
        {
            Query.Where(po => po.OrderDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            Query.Where(po => po.OrderDate <= toDate.Value);
        }

        Query.OrderByDescending(po => po.OrderDate);
    }
}

