namespace Accounting.Domain.Events.InterCompanyTransaction;

public record InterCompanyTransactionCreated(DefaultIdType Id, string TransactionNumber, string FromEntityName, string ToEntityName, decimal Amount, DateTime TransactionDate, string? Description, string? Notes) : DomainEvent;

public record InterCompanyTransactionUpdated(DefaultIdType Id, string TransactionNumber, decimal Amount, string? Description, string? Notes) : DomainEvent;

public record InterCompanyTransactionDeleted(DefaultIdType Id) : DomainEvent;

public record InterCompanyTransactionMatched(DefaultIdType Id, string TransactionNumber, string FromEntityName, string ToEntityName, DefaultIdType MatchingTransactionId) : DomainEvent;

public record InterCompanyTransactionReconciled(DefaultIdType Id, string TransactionNumber, string FromEntityName, string ToEntityName, string ReconciledBy, DateTime ReconciliationDate) : DomainEvent;

public record InterCompanyTransactionDisputed(DefaultIdType Id, string TransactionNumber, string FromEntityName, string ToEntityName, string Reason) : DomainEvent;

public record InterCompanyTransactionDisputeResolved(DefaultIdType Id, string TransactionNumber) : DomainEvent;

public record InterCompanyTransactionSettled(DefaultIdType Id, string TransactionNumber, DateTime SettlementDate) : DomainEvent;

public record InterCompanyTransactionEliminated(DefaultIdType Id, string TransactionNumber, DateTime EliminationDate) : DomainEvent;

public record InterCompanyTransactionReversed(DefaultIdType Id, string TransactionNumber, string Reason) : DomainEvent;

public record InterCompanyTransactionClosed(DefaultIdType Id, string TransactionNumber) : DomainEvent;

