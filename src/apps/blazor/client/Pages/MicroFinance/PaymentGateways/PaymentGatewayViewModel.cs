namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.PaymentGateways;

public class PaymentGatewayViewModel
{
    public string? Name { get; set; }
    public string? Provider { get; set; }
    public decimal TransactionFeePercent { get; set; }
    public decimal TransactionFeeFixed { get; set; }
    public decimal MinTransactionAmount { get; set; }
    public decimal MaxTransactionAmount { get; set; }
}
