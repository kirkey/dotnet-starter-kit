namespace Accounting.Application.Checks.Search.v1;

/// <summary>
/// Specification for searching checks with filtering and pagination.
/// </summary>
public class CheckSearchSpec : EntitiesByPaginationFilterSpec<Check, CheckSearchResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckSearchSpec"/> class.
    /// </summary>
    /// <param name="query">The check search query containing filter criteria and pagination parameters.</param>
    public CheckSearchSpec(CheckSearchQuery query)
        : base(query)
    {
        Query
            .Where(c => c.CheckNumber.Contains(query.CheckNumber!), !string.IsNullOrWhiteSpace(query.CheckNumber))
            .Where(c => c.BankAccountCode == query.BankAccountCode!, !string.IsNullOrWhiteSpace(query.BankAccountCode))
            .Where(c => c.Status == query.Status!, !string.IsNullOrWhiteSpace(query.Status))
            .Where(c => c.PayeeName!.Contains(query.PayeeName!), !string.IsNullOrWhiteSpace(query.PayeeName))
            .Where(c => c.VendorId == query.VendorId, query.VendorId.HasValue)
            .Where(c => c.PayeeId == query.PayeeId, query.PayeeId.HasValue)
            .Where(c => c.IssuedDate >= query.IssuedDateFrom, query.IssuedDateFrom.HasValue)
            .Where(c => c.IssuedDate <= query.IssuedDateTo, query.IssuedDateTo.HasValue)
            .Where(c => c.ClearedDate >= query.ClearedDateFrom, query.ClearedDateFrom.HasValue)
            .Where(c => c.ClearedDate <= query.ClearedDateTo, query.ClearedDateTo.HasValue)
            .Where(c => c.IsPrinted == query.IsPrinted, query.IsPrinted.HasValue)
            .Where(c => c.IsStopPayment == query.IsStopPayment, query.IsStopPayment.HasValue)
            .Where(c => c.Amount >= query.AmountFrom, query.AmountFrom.HasValue)
            .Where(c => c.Amount <= query.AmountTo, query.AmountTo.HasValue)
            .OrderByDescending(c => c.IssuedDate, !query.HasOrderBy());
    }
}

