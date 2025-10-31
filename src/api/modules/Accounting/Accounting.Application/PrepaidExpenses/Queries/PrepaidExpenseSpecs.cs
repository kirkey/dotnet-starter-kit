using Accounting.Domain.Entities;

namespace Accounting.Application.PrepaidExpenses.Queries;

/// <summary>
/// Specification to find prepaid expense by prepaid number.
/// </summary>
public class PrepaidExpenseByNumberSpec : Specification<PrepaidExpense>
{
    public PrepaidExpenseByNumberSpec(string prepaidNumber)
    {
        Query.Where(p => p.PrepaidNumber == prepaidNumber);
    }
}

/// <summary>
/// Specification to find prepaid expense by ID.
/// </summary>
public class PrepaidExpenseByIdSpec : Specification<PrepaidExpense>
{
    public PrepaidExpenseByIdSpec(DefaultIdType id)
    {
        Query.Where(p => p.Id == id);
    }
}

/// <summary>
/// Specification for searching prepaid expenses with filters.
/// </summary>
public class PrepaidExpenseSearchSpec : Specification<PrepaidExpense>
{
    public PrepaidExpenseSearchSpec(
        string? prepaidNumber = null,
        string? status = null,
        bool? isFullyAmortized = null,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        if (!string.IsNullOrWhiteSpace(prepaidNumber))
        {
            Query.Where(p => p.PrepaidNumber.Contains(prepaidNumber));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            Query.Where(p => p.Status == status);
        }

        if (isFullyAmortized.HasValue)
        {
            Query.Where(p => p.IsFullyAmortized == isFullyAmortized.Value);
        }

        if (fromDate.HasValue)
        {
            Query.Where(p => p.StartDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            Query.Where(p => p.EndDate <= toDate.Value);
        }

        Query.OrderByDescending(p => p.StartDate);
    }
}

