namespace Accounting.Application.FiscalPeriodCloses.Queries;

/// <summary>
/// Specification to find fiscal period close by close number.
/// </summary>
public class FiscalPeriodCloseByNumberSpec : Specification<FiscalPeriodClose>
{
    public FiscalPeriodCloseByNumberSpec(string closeNumber)
    {
        Query.Where(fpc => fpc.CloseNumber == closeNumber);
    }
}

/// <summary>
/// Specification to find fiscal period close by period ID.
/// </summary>
public class FiscalPeriodCloseByPeriodSpec : Specification<FiscalPeriodClose>
{
    public FiscalPeriodCloseByPeriodSpec(DefaultIdType periodId)
    {
        Query.Where(fpc => fpc.PeriodId == periodId);
    }
}

/// <summary>
/// Specification to find fiscal period close by ID.
/// </summary>
public class FiscalPeriodCloseByIdSpec : Specification<FiscalPeriodClose>
{
    public FiscalPeriodCloseByIdSpec(DefaultIdType id)
    {
        Query.Where(fpc => fpc.Id == id);
    }
}

/// <summary>
/// Specification for searching fiscal period closes with filters and pagination.
/// </summary>
public class FiscalPeriodCloseSearchSpec : Specification<FiscalPeriodClose>
{
    public FiscalPeriodCloseSearchSpec(Search.SearchFiscalPeriodClosesRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.CloseNumber))
        {
            Query.Where(fpc => fpc.CloseNumber.Contains(request.CloseNumber));
        }

        if (!string.IsNullOrWhiteSpace(request.CloseType))
        {
            Query.Where(fpc => fpc.CloseType == request.CloseType);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            Query.Where(fpc => fpc.Status == request.Status);
        }

        // Apply sorting
        if (request.OrderBy?.Any() == true)
        {
            // Handle ordering if provided
        }
        else
        {
            Query.OrderByDescending(fpc => fpc.PeriodEndDate);
        }
    }
}

