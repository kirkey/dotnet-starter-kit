using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>
/// Domain events for Investment/Wealth Management entities.
/// </summary>
/// 
// InvestmentProduct Events
public sealed record InvestmentProductCreated(InvestmentProduct Product) : DomainEvent;
public sealed record InvestmentProductUpdated(InvestmentProduct Product) : DomainEvent;
public sealed record InvestmentProductNavUpdated(Guid ProductId, decimal Nav, DateOnly NavDate) : DomainEvent;

// InvestmentAccount Events
public sealed record InvestmentAccountCreated(InvestmentAccount Account) : DomainEvent;
public sealed record InvestmentAccountUpdated(InvestmentAccount Account) : DomainEvent;
public sealed record InvestmentMade(Guid AccountId, decimal Amount) : DomainEvent;
public sealed record InvestmentRedeemed(Guid AccountId, decimal Amount, decimal GainLoss) : DomainEvent;
public sealed record InvestmentAccountValuationUpdated(Guid AccountId, decimal CurrentValue, decimal GainLossPercent) : DomainEvent;
public sealed record SipSetup(Guid AccountId, decimal Amount, string Frequency) : DomainEvent;

// InvestmentTransaction Events
public sealed record InvestmentTransactionCreated(InvestmentTransaction Transaction) : DomainEvent;
public sealed record InvestmentTransactionUpdated(InvestmentTransaction Transaction) : DomainEvent;
public sealed record InvestmentTransactionCompleted(Guid TransactionId, string TransactionType, decimal Amount) : DomainEvent;
public sealed record InvestmentTransactionFailed(Guid TransactionId, string Reason) : DomainEvent;
