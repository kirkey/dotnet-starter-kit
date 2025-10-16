namespace Accounting.Application.Checks.Issue.v1;

/// <summary>
/// Command to issue a check for payment.
/// </summary>
public record CheckIssueCommand(
    DefaultIdType CheckId,
    decimal Amount,
    string PayeeName,
    DateTime? IssuedDate,
    DefaultIdType? PayeeId,
    DefaultIdType? VendorId,
    DefaultIdType? PaymentId,
    DefaultIdType? ExpenseId,
    string? Memo
) : IRequest<CheckIssueResponse>;
