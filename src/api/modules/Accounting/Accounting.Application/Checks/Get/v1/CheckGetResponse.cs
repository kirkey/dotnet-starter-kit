namespace Accounting.Application.Checks.Get.v1;

/// <summary>
/// Response containing check details.
/// </summary>
public record CheckGetResponse(
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
    DateTime? VoidedDate,
    string? VoidReason,
    DefaultIdType? PaymentId,
    DefaultIdType? ExpenseId,
    string? Memo,
    bool IsPrinted,
    DateTime? PrintedDate,
    string? PrintedBy,
    bool IsStopPayment,
    DateTime? StopPaymentDate,
    string? StopPaymentReason,
    string? Description,
    string? Notes,
    DateTimeOffset CreatedOn,
    DefaultIdType? CreatedBy,
    DateTimeOffset? LastModifiedOn,
    DefaultIdType? LastModifiedBy
);

