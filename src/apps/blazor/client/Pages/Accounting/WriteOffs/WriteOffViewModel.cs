namespace FSH.Starter.Blazor.Client.Pages.Accounting.WriteOffs;

/// <summary>
/// ViewModel used for creating or editing write-offs.
/// </summary>
public sealed class WriteOffViewModel
{
    public DefaultIdType? Id { get; set; }
    public string? ReferenceNumber { get; set; }
    public DateTime? WriteOffDate { get; set; } = DateTime.Today;
    public string? WriteOffType { get; set; }
    public decimal Amount { get; set; }
    public decimal RecoveredAmount { get; set; }
    public DefaultIdType? CustomerId { get; set; }
    public DefaultIdType? InvoiceId { get; set; }
    public DefaultIdType? ReceivableAccountId { get; set; }
    public DefaultIdType? ExpenseAccountId { get; set; }
    public string? Reason { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public bool IsPosted { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ApprovalStatus { get; set; } = string.Empty;
}

