
namespace Accounting.Application.InterCompanyTransactions.Queries;

/// <summary>
/// Specification to find inter-company transaction by transaction number.
/// </summary>
public class InterCompanyTransactionByNumberSpec : Specification<InterCompanyTransaction>
{
    public InterCompanyTransactionByNumberSpec(string transactionNumber)
    {
        Query.Where(t => t.TransactionNumber == transactionNumber);
    }
}

/// <summary>
/// Specification to find inter-company transaction by ID.
/// </summary>
public class InterCompanyTransactionByIdSpec : Specification<InterCompanyTransaction>
{
    public InterCompanyTransactionByIdSpec(DefaultIdType id)
    {
        Query.Where(t => t.Id == id);
    }
}

/// <summary>
/// Specification for searching inter-company transactions with filters.
/// </summary>
public class InterCompanyTransactionSearchSpec : Specification<InterCompanyTransaction>
{
    public InterCompanyTransactionSearchSpec(
        string? transactionNumber = null,
        DefaultIdType? fromEntityId = null,
        DefaultIdType? toEntityId = null,
        string? transactionType = null,
        string? status = null,
        bool? isReconciled = null,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        if (!string.IsNullOrWhiteSpace(transactionNumber))
        {
            Query.Where(t => t.TransactionNumber.Contains(transactionNumber));
        }

        if (fromEntityId.HasValue)
        {
            Query.Where(t => t.FromEntityId == fromEntityId.Value);
        }

        if (toEntityId.HasValue)
        {
            Query.Where(t => t.ToEntityId == toEntityId.Value);
        }

        if (!string.IsNullOrWhiteSpace(transactionType))
        {
            Query.Where(t => t.TransactionType == transactionType);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            Query.Where(t => t.Status == status);
        }

        if (isReconciled.HasValue)
        {
            Query.Where(t => t.IsReconciled == isReconciled.Value);
        }

        if (fromDate.HasValue)
        {
            Query.Where(t => t.TransactionDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            Query.Where(t => t.TransactionDate <= toDate.Value);
        }

        Query.OrderByDescending(t => t.TransactionDate);
    }
}

