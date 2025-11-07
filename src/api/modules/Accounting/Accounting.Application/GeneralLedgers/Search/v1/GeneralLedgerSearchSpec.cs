namespace Accounting.Application.GeneralLedgers.Search.v1;

/// <summary>
/// Specification for searching general ledger entries.
/// </summary>
public sealed class GeneralLedgerSearchSpec : Specification<GeneralLedger>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralLedgerSearchSpec"/> class.
    /// </summary>
    public GeneralLedgerSearchSpec(GeneralLedgerSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        // Filter by journal entry ID
        if (query.EntryId.HasValue && query.EntryId.Value != DefaultIdType.Empty)
        {
            Query.Where(gl => gl.EntryId == query.EntryId.Value);
        }

        // Filter by account ID
        if (query.AccountId.HasValue && query.AccountId.Value != DefaultIdType.Empty)
        {
            Query.Where(gl => gl.AccountId == query.AccountId.Value);
        }

        // Filter by period ID
        if (query.PeriodId.HasValue && query.PeriodId.Value != DefaultIdType.Empty)
        {
            Query.Where(gl => gl.PeriodId == query.PeriodId.Value);
        }

        // Filter by USOA class
        if (!string.IsNullOrWhiteSpace(query.UsoaClass))
        {
            Query.Where(gl => gl.UsoaClass == query.UsoaClass);
        }

        // Filter by transaction date range
        if (query.StartDate.HasValue)
        {
            Query.Where(gl => gl.TransactionDate >= query.StartDate.Value);
        }

        if (query.EndDate.HasValue)
        {
            Query.Where(gl => gl.TransactionDate <= query.EndDate.Value);
        }

        // Filter by debit amount range
        if (query.MinDebit.HasValue)
        {
            Query.Where(gl => gl.Debit >= query.MinDebit.Value);
        }

        if (query.MaxDebit.HasValue)
        {
            Query.Where(gl => gl.Debit <= query.MaxDebit.Value);
        }

        // Filter by credit amount range
        if (query.MinCredit.HasValue)
        {
            Query.Where(gl => gl.Credit >= query.MinCredit.Value);
        }

        if (query.MaxCredit.HasValue)
        {
            Query.Where(gl => gl.Credit <= query.MaxCredit.Value);
        }

        // Filter by reference number (partial match)
        if (!string.IsNullOrWhiteSpace(query.ReferenceNumber))
        {
            Query.Where(gl => gl.ReferenceNumber != null && gl.ReferenceNumber.Contains(query.ReferenceNumber));
        }

        // Apply pagination
        Query.Skip(query.PageNumber * query.PageSize)
             .Take(query.PageSize);

        // Order by transaction date descending, then by account ID
        Query.OrderByDescending(gl => gl.TransactionDate)
             .ThenBy(gl => gl.AccountId);
    }
}

