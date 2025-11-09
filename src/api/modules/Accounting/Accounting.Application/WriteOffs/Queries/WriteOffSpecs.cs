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
/// Specification for searching write-offs with filters and pagination.
/// </summary>
public class WriteOffSearchSpec : Specification<WriteOff>
{
    public WriteOffSearchSpec(Search.v1.SearchWriteOffsRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.ReferenceNumber))
        {
            Query.Where(w => w.ReferenceNumber.Contains(request.ReferenceNumber));
        }

        if (request.CustomerId.HasValue)
        {
            Query.Where(w => w.CustomerId == request.CustomerId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.WriteOffType))
        {
            Query.Where(w => w.WriteOffType.ToString() == request.WriteOffType);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            Query.Where(w => w.Status.ToString() == request.Status);
        }

        if (request.IsRecovered.HasValue)
        {
            Query.Where(w => w.IsRecovered == request.IsRecovered.Value);
        }

        // Apply sorting
        if (request.OrderBy?.Any() == true)
        {
            // Handle ordering if provided
        }
        else
        {
            Query.OrderByDescending(w => w.WriteOffDate);
        }
    }
}

