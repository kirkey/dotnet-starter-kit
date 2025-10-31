namespace Accounting.Application.WriteOffs.Queries;

/// <summary>
/// Specification to find write-off by reference number.
/// </summary>
public class WriteOffByReferenceNumberSpec : Specification<WriteOff>
{
    public WriteOffByReferenceNumberSpec(string referenceNumber)
    {
        Query.Where(w => w.ReferenceNumber == referenceNumber);
    }
}

/// <summary>
/// Specification to find write-off by ID.
/// </summary>
public class WriteOffByIdSpec : Specification<WriteOff>
{
    public WriteOffByIdSpec(DefaultIdType id)
    {
        Query.Where(w => w.Id == id);
    }
}

/// <summary>
/// Specification for searching write-offs with filters.
/// </summary>
public class WriteOffSearchSpec : Specification<WriteOff>
{
    public WriteOffSearchSpec(
        string? referenceNumber = null,
        DefaultIdType? customerId = null,
        string? writeOffType = null,
        string? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        bool? isRecovered = null)
    {
        if (!string.IsNullOrWhiteSpace(referenceNumber))
        {
            Query.Where(w => w.ReferenceNumber.Contains(referenceNumber));
        }

        if (customerId.HasValue)
        {
            Query.Where(w => w.CustomerId == customerId.Value);
        }

        if (!string.IsNullOrWhiteSpace(writeOffType))
        {
            Query.Where(w => w.WriteOffType.ToString() == writeOffType);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            Query.Where(w => w.Status.ToString() == status);
        }

        if (fromDate.HasValue)
        {
            Query.Where(w => w.WriteOffDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            Query.Where(w => w.WriteOffDate <= toDate.Value);
        }

        if (isRecovered.HasValue)
        {
            Query.Where(w => w.IsRecovered == isRecovered.Value);
        }

        Query.OrderByDescending(w => w.WriteOffDate);
    }
}

