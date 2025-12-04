using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a payment gateway integration configuration.
/// </summary>
public sealed class PaymentGateway : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int NameMaxLength = 128;
    public const int ProviderMaxLength = 64;
    public const int StatusMaxLength = 32;
    public const int ApiKeyMaxLength = 256;
    public const int MerchantIdMaxLength = 128;
    public const int WebhookUrlMaxLength = 512;
    
    // Gateway Status
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusMaintenance = "Maintenance";
    public const string StatusDisabled = "Disabled";
    
    // Provider Types
    public const string ProviderStripe = "Stripe";
    public const string ProviderPaystack = "Paystack";
    public const string ProviderFlutterwave = "Flutterwave";
    public const string ProviderMpesa = "M-Pesa";
    public const string ProviderMtn = "MTN Mobile Money";
    public const string ProviderAirtel = "Airtel Money";

    public string Provider { get; private set; } = default!;
    public string Status { get; private set; } = StatusInactive;
    public string? MerchantId { get; private set; }
    public string? ApiKeyEncrypted { get; private set; }
    public string? SecretKeyEncrypted { get; private set; }
    public string? WebhookUrl { get; private set; }
    public string? WebhookSecret { get; private set; }
    public decimal TransactionFeePercent { get; private set; }
    public decimal TransactionFeeFixed { get; private set; }
    public decimal MinTransactionAmount { get; private set; }
    public decimal MaxTransactionAmount { get; private set; }
    public bool SupportsRefunds { get; private set; }
    public bool SupportsRecurring { get; private set; }
    public bool SupportsMobileWallet { get; private set; }
    public bool SupportsCardPayments { get; private set; }
    public bool SupportsBankTransfer { get; private set; }
    public bool IsTestMode { get; private set; }
    public string? CallbackUrl { get; private set; }
    public string? IpWhitelist { get; private set; }
    public int TimeoutSeconds { get; private set; } = 30;
    public int RetryAttempts { get; private set; } = 3;
    public DateTimeOffset? LastSuccessfulConnection { get; private set; }

    private PaymentGateway() { }

    public static PaymentGateway Create(
        string name,
        string provider,
        decimal transactionFeePercent,
        decimal transactionFeeFixed,
        decimal minTransactionAmount,
        decimal maxTransactionAmount)
    {
        var gateway = new PaymentGateway
        {
            Provider = provider,
            TransactionFeePercent = transactionFeePercent,
            TransactionFeeFixed = transactionFeeFixed,
            MinTransactionAmount = minTransactionAmount,
            MaxTransactionAmount = maxTransactionAmount,
            Status = StatusInactive,
            IsTestMode = true
        };
        gateway.Name = name;

        gateway.QueueDomainEvent(new PaymentGatewayCreated(gateway));
        return gateway;
    }

    public PaymentGateway Configure(
        string merchantId,
        string apiKeyEncrypted,
        string secretKeyEncrypted,
        string? webhookUrl = null,
        string? webhookSecret = null)
    {
        MerchantId = merchantId;
        ApiKeyEncrypted = apiKeyEncrypted;
        SecretKeyEncrypted = secretKeyEncrypted;
        WebhookUrl = webhookUrl;
        WebhookSecret = webhookSecret;
        QueueDomainEvent(new PaymentGatewayConfigured(Id, Provider));
        return this;
    }

    public PaymentGateway Activate()
    {
        Status = StatusActive;
        QueueDomainEvent(new PaymentGatewayActivated(Id, Name));
        return this;
    }

    public PaymentGateway Deactivate()
    {
        Status = StatusInactive;
        return this;
    }

    public PaymentGateway SetTestMode(bool isTestMode)
    {
        IsTestMode = isTestMode;
        return this;
    }

    public PaymentGateway RecordSuccessfulConnection()
    {
        LastSuccessfulConnection = DateTimeOffset.UtcNow;
        return this;
    }

    public PaymentGateway SetMaintenanceMode()
    {
        Status = StatusMaintenance;
        return this;
    }

    public PaymentGateway Update(
        string? name = null,
        decimal? transactionFeePercent = null,
        decimal? transactionFeeFixed = null,
        decimal? minTransactionAmount = null,
        decimal? maxTransactionAmount = null,
        bool? supportsRefunds = null,
        bool? supportsRecurring = null,
        bool? supportsMobileWallet = null,
        bool? supportsCardPayments = null,
        bool? supportsBankTransfer = null,
        int? timeoutSeconds = null,
        int? retryAttempts = null)
    {
        if (name is not null) Name = name;
        if (transactionFeePercent.HasValue) TransactionFeePercent = transactionFeePercent.Value;
        if (transactionFeeFixed.HasValue) TransactionFeeFixed = transactionFeeFixed.Value;
        if (minTransactionAmount.HasValue) MinTransactionAmount = minTransactionAmount.Value;
        if (maxTransactionAmount.HasValue) MaxTransactionAmount = maxTransactionAmount.Value;
        if (supportsRefunds.HasValue) SupportsRefunds = supportsRefunds.Value;
        if (supportsRecurring.HasValue) SupportsRecurring = supportsRecurring.Value;
        if (supportsMobileWallet.HasValue) SupportsMobileWallet = supportsMobileWallet.Value;
        if (supportsCardPayments.HasValue) SupportsCardPayments = supportsCardPayments.Value;
        if (supportsBankTransfer.HasValue) SupportsBankTransfer = supportsBankTransfer.Value;
        if (timeoutSeconds.HasValue) TimeoutSeconds = timeoutSeconds.Value;
        if (retryAttempts.HasValue) RetryAttempts = retryAttempts.Value;

        QueueDomainEvent(new PaymentGatewayUpdated(this));
        return this;
    }
}
