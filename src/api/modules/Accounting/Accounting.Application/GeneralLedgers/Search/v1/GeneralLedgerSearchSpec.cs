namespace Accounting.Application.GeneralLedgers.Search.v1;

/// <summary>
/// Specification for searching general ledger entries.
/// </summary>
public sealed class GeneralLedgerSearchSpec : Specification<GeneralLedger>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralLedgerSearchSpec"/> class.
    /// </summary>
    public GeneralLedgerSearchSpec(GeneralLedgerSearchRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Filter by journal entry ID
        if (request.EntryId.HasValue && request.EntryId.Value != DefaultIdType.Empty)
        {
            Query.Where(gl => gl.EntryId == request.EntryId.Value);
        }

        // Filter by account ID
        if (request.AccountId.HasValue && request.AccountId.Value != DefaultIdType.Empty)
        {
            Query.Where(gl => gl.AccountId == request.AccountId.Value);
        }

        // Filter by period ID
        if (request.PeriodId.HasValue && request.PeriodId.Value != DefaultIdType.Empty)
        {
            Query.Where(gl => gl.PeriodId == request.PeriodId.Value);
        }

        // Filter by USOA class
        if (!string.IsNullOrWhiteSpace(request.UsoaClass))
        {
            Query.Where(gl => gl.UsoaClass == request.UsoaClass);
        }

        // Filter by transaction date range
        if (request.StartDate.HasValue)
        {
            Query.Where(gl => gl.TransactionDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            Query.Where(gl => gl.TransactionDate <= request.EndDate.Value);
        }

        // Filter by debit amount range
        if (request.MinDebit.HasValue)
        {
            Query.Where(gl => gl.Debit >= request.MinDebit.Value);
        }

        if (request.MaxDebit.HasValue)
        {
            Query.Where(gl => gl.Debit <= request.MaxDebit.Value);
        }

        // Filter by credit amount range
        if (request.MinCredit.HasValue)
        {
            Query.Where(gl => gl.Credit >= request.MinCredit.Value);
        }

        if (request.MaxCredit.HasValue)
        {
            Query.Where(gl => gl.Credit <= request.MaxCredit.Value);
        }

        // Filter by reference number (partial match)
        if (!string.IsNullOrWhiteSpace(request.ReferenceNumber))
        {
            Query.Where(gl => gl.ReferenceNumber != null && gl.ReferenceNumber.Contains(request.ReferenceNumber));
        }

        // Apply pagination
        Query.Skip(request.PageNumber * request.PageSize)
             .Take(request.PageSize);

        // Order by transaction date descending, then by account ID
        Query.OrderByDescending(gl => gl.TransactionDate)
             .ThenBy(gl => gl.AccountId);
    }
}

