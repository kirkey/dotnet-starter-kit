namespace FSH.Starter.Blazor.Client.Pages.Accounting.AccountReconciliations;

/// <summary>
/// View model for account reconciliation operations.
/// </summary>
public class AccountReconciliationViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType GeneralLedgerAccountId { get; set; }
    public DefaultIdType AccountingPeriodId { get; set; }
    public decimal GlBalance { get; set; }
    public decimal SubsidiaryLedgerBalance { get; set; }
    public decimal Variance { get; set; }
    public string ReconciliationStatus { get; set; } = "Pending";
    public DateTime ReconciliationDate { get; set; } = DateTime.UtcNow;
    public string? VarianceExplanation { get; set; }
    public string SubsidiaryLedgerSource { get; set; } = string.Empty;
    public int LineItemCount { get; set; }
    public bool AdjustingEntriesRecorded { get; set; }
    public string? Status { get; set; }
    public string? ApproverName { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public string? Remarks { get; set; }
}

