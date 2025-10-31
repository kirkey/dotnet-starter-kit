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
/// Specification for searching fiscal period closes with filters.
/// </summary>
public class FiscalPeriodCloseSearchSpec : Specification<FiscalPeriodClose>
{
    public FiscalPeriodCloseSearchSpec(
        string? closeNumber = null,
        string? closeType = null,
        string? status = null,
        bool? isComplete = null,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        if (!string.IsNullOrWhiteSpace(closeNumber))
        {
            Query.Where(fpc => fpc.CloseNumber.Contains(closeNumber));
        }

        if (!string.IsNullOrWhiteSpace(closeType))
        {
            Query.Where(fpc => fpc.CloseType == closeType);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            Query.Where(fpc => fpc.Status == status);
        }

        if (isComplete.HasValue)
        {
            Query.Where(fpc => fpc.IsComplete == isComplete.Value);
        }

        if (fromDate.HasValue)
        {
            Query.Where(fpc => fpc.PeriodStartDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            Query.Where(fpc => fpc.PeriodEndDate <= toDate.Value);
        }

        Query.OrderByDescending(fpc => fpc.PeriodEndDate);
    }
}

