namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.QrPayments;

public class QrPaymentViewModel
{
    // QR Payments are typically generated via a different command, read-only view
    public DefaultIdType? WalletId { get; set; }
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? AgentId { get; set; }
    public string? QrCode { get; set; }
    public string? QrType { get; set; }
    public decimal? Amount { get; set; }
    public string? Reference { get; set; }
    public string? Notes { get; set; }
    public int MaxUses { get; set; }
}
