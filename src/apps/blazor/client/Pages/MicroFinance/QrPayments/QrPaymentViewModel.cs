namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.QrPayments;

public class QrPaymentViewModel
{
    // QR Payments are typically generated via a different command, read-only view
    public Guid? WalletId { get; set; }
    public Guid? MemberId { get; set; }
    public Guid? AgentId { get; set; }
    public string? QrCode { get; set; }
    public string? QrType { get; set; }
    public decimal? Amount { get; set; }
    public string? Reference { get; set; }
    public string? Notes { get; set; }
    public int MaxUses { get; set; }
}
