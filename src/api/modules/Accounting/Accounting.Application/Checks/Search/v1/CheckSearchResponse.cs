namespace Accounting.Application.Checks.Search.v1;

/// <summary>
/// Response containing check search results.
/// </summary>
public record CheckSearchResponse(
    DefaultIdType Id,
    string CheckNumber,
    string BankAccountCode,
    string? BankAccountName,
    DefaultIdType? BankId,
    string? BankName,
    string Status,
    decimal? Amount,
    string? PayeeName,
    DefaultIdType? VendorId,
    DefaultIdType? PayeeId,
    DateTime? IssuedDate,
    DateTime? ClearedDate,
    bool IsPrinted,
    bool IsStopPayment,
    string? Memo,
    DateTimeOffset CreatedOn
);

