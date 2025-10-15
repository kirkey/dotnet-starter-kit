using Accounting.Domain.Entities;

namespace Accounting.Application.Checks.Search.v1;

/// <summary>
/// Specification for searching checks with filtering and pagination.
/// </summary>
public class CheckSearchSpec : Specification<Check, CheckSearchResponse>
{
    public CheckSearchSpec(CheckSearchQuery query)
    {
        // Filtering
        Query.Where(c => c.CheckNumber.Contains(query.CheckNumber!), !string.IsNullOrWhiteSpace(query.CheckNumber));
        Query.Where(c => c.BankAccountCode == query.BankAccountCode!, !string.IsNullOrWhiteSpace(query.BankAccountCode));
        Query.Where(c => c.Status == query.Status!, !string.IsNullOrWhiteSpace(query.Status));
        Query.Where(c => c.PayeeName!.Contains(query.PayeeName!), !string.IsNullOrWhiteSpace(query.PayeeName));
        Query.Where(c => c.VendorId == query.VendorId, query.VendorId.HasValue);
        Query.Where(c => c.PayeeId == query.PayeeId, query.PayeeId.HasValue);
        Query.Where(c => c.IssuedDate >= query.IssuedDateFrom, query.IssuedDateFrom.HasValue);
        Query.Where(c => c.IssuedDate <= query.IssuedDateTo, query.IssuedDateTo.HasValue);
        Query.Where(c => c.ClearedDate >= query.ClearedDateFrom, query.ClearedDateFrom.HasValue);
        Query.Where(c => c.ClearedDate <= query.ClearedDateTo, query.ClearedDateTo.HasValue);
        Query.Where(c => c.IsPrinted == query.IsPrinted, query.IsPrinted.HasValue);
        Query.Where(c => c.IsStopPayment == query.IsStopPayment, query.IsStopPayment.HasValue);
        Query.Where(c => c.Amount >= query.AmountFrom, query.AmountFrom.HasValue);
        Query.Where(c => c.Amount <= query.AmountTo, query.AmountTo.HasValue);
    }
}

