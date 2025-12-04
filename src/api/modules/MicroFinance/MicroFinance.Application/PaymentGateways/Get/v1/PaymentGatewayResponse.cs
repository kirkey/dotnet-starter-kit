namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Get.v1;

public sealed record PaymentGatewayResponse(
    Guid Id,
    string Name,
    string Provider,
    string Status,
    string? MerchantId,
    string? WebhookUrl,
    decimal TransactionFeePercent,
    decimal TransactionFeeFixed,
    decimal MinTransactionAmount,
    decimal MaxTransactionAmount,
    bool SupportsRefunds,
    bool SupportsRecurring,
    bool SupportsMobileWallet,
    bool SupportsCardPayments,
    bool SupportsBankTransfer,
    bool IsTestMode,
    int TimeoutSeconds,
    int RetryAttempts,
    DateTimeOffset? LastSuccessfulConnection,
    DateTimeOffset CreatedOn);
