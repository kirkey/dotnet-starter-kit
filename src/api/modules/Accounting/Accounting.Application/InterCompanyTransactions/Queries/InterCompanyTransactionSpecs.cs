
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
/// Specification for searching inter-company transactions with filters and pagination.
/// </summary>
public class InterCompanyTransactionSearchSpec : EntitiesByPaginationFilterSpec<InterCompanyTransaction, Responses.InterCompanyTransactionResponse>
{
    public InterCompanyTransactionSearchSpec(Search.v1.SearchInterCompanyTransactionsRequest request)
        : base(request)
    {
        if (!string.IsNullOrWhiteSpace(request.TransactionNumber))
        {
            Query.Where(t => t.TransactionNumber.Contains(request.TransactionNumber));
        }

        if (request.FromEntityId.HasValue)
        {
            Query.Where(t => t.FromEntityId == request.FromEntityId.Value);
        }

        if (request.ToEntityId.HasValue)
        {
            Query.Where(t => t.ToEntityId == request.ToEntityId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.TransactionType))
        {
            Query.Where(t => t.TransactionType == request.TransactionType);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            Query.Where(t => t.Status == request.Status);
        }

        if (request.IsReconciled.HasValue)
        {
            Query.Where(t => t.IsReconciled == request.IsReconciled.Value);
        }

        Query.OrderByDescending(t => t.TransactionDate);
    }
}

