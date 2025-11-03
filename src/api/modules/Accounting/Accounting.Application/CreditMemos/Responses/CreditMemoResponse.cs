namespace Accounting.Application.CreditMemos.Responses;

/// <summary>
/// Response model for credit memo operations.
/// </summary>
public sealed record CreditMemoResponse
{
    public DefaultIdType Id { get; init; }
    public string MemoNumber { get; init; } = null!;
    public DateTime MemoDate { get; init; }
    public decimal Amount { get; init; }
    public decimal AppliedAmount { get; init; }
    public decimal RefundedAmount { get; init; }
    public decimal UnappliedAmount { get; init; }
    public string ReferenceType { get; init; } = null!;
    public DefaultIdType ReferenceId { get; init; }
    public DefaultIdType? OriginalDocumentId { get; init; }
    public string? Reason { get; init; }
    public string Status { get; init; } = null!;
    public bool IsApplied { get; init; }
    public DateTime? AppliedDate { get; init; }
    public string ApprovalStatus { get; init; } = null!;
    public string? ApprovedBy { get; init; }
    public DateTime? ApprovedDate { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public DateTimeOffset CreatedOn { get; init; }
    public DefaultIdType? CreatedBy { get; init; }
    public DateTimeOffset? LastModifiedOn { get; init; }
    public DefaultIdType? LastModifiedBy { get; init; }
}
