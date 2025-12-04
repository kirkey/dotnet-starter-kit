using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>
/// Domain events for Mobile Money and Digital Payment entities.
/// </summary>
/// 
// MobileWallet Events
public sealed record MobileWalletCreated(MobileWallet Wallet) : DomainEvent;
public sealed record MobileWalletUpdated(MobileWallet Wallet) : DomainEvent;
public sealed record MobileWalletActivated(Guid WalletId, string PhoneNumber) : DomainEvent;
public sealed record MobileWalletCredited(Guid WalletId, decimal Amount, string TransactionRef) : DomainEvent;
public sealed record MobileWalletDebited(Guid WalletId, decimal Amount, string TransactionRef) : DomainEvent;
public sealed record MobileWalletSuspended(Guid WalletId, string Reason) : DomainEvent;
public sealed record MobileWalletLinkedToAccount(Guid WalletId, Guid SavingsAccountId) : DomainEvent;
public sealed record MobileWalletTierUpgraded(Guid WalletId, string NewTier) : DomainEvent;

// MobileTransaction Events
public sealed record MobileTransactionCreated(MobileTransaction Transaction) : DomainEvent;
public sealed record MobileTransactionUpdated(MobileTransaction Transaction) : DomainEvent;
public sealed record MobileTransactionProcessing(Guid TransactionId, string ProviderReference) : DomainEvent;
public sealed record MobileTransactionCompleted(Guid TransactionId, decimal Amount, string TransactionRef) : DomainEvent;
public sealed record MobileTransactionFailed(Guid TransactionId, string FailureReason) : DomainEvent;
public sealed record MobileTransactionReversed(Guid TransactionId, Guid ReversalTransactionId) : DomainEvent;

// UssdSession Events
public sealed record UssdSessionStarted(UssdSession Session) : DomainEvent;
public sealed record UssdSessionNavigated(Guid SessionId, string CurrentMenu, int StepCount) : DomainEvent;
public sealed record UssdSessionAuthenticated(Guid SessionId, string PhoneNumber) : DomainEvent;
public sealed record UssdSessionCompleted(Guid SessionId, int TotalSteps) : DomainEvent;
public sealed record UssdSessionTimedOut(Guid SessionId) : DomainEvent;

// AgentBanking Events
public sealed record AgentBankingCreated(AgentBanking Agent) : DomainEvent;
public sealed record AgentBankingUpdated(AgentBanking Agent) : DomainEvent;
public sealed record AgentBankingApproved(Guid AgentId, string AgentCode) : DomainEvent;
public sealed record AgentBankingSuspended(Guid AgentId, string Reason) : DomainEvent;
public sealed record AgentFloatCredited(Guid AgentId, decimal Amount, decimal NewBalance) : DomainEvent;
public sealed record AgentFloatDebited(Guid AgentId, decimal Amount, decimal NewBalance) : DomainEvent;
public sealed record AgentTierUpgraded(Guid AgentId, string NewTier) : DomainEvent;

// PaymentGateway Events
public sealed record PaymentGatewayCreated(PaymentGateway Gateway) : DomainEvent;
public sealed record PaymentGatewayUpdated(PaymentGateway Gateway) : DomainEvent;
public sealed record PaymentGatewayConfigured(Guid GatewayId, string Provider) : DomainEvent;
public sealed record PaymentGatewayActivated(Guid GatewayId, string Name) : DomainEvent;

// QrPayment Events
public sealed record QrPaymentCreated(QrPayment Qr) : DomainEvent;
public sealed record QrPaymentUsed(Guid QrId, Guid TransactionId) : DomainEvent;
