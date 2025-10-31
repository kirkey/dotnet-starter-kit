namespace Accounting.Domain.Events.TrialBalance;

public record TrialBalanceCreated(DefaultIdType Id, string TrialBalanceNumber, DefaultIdType PeriodId, DateTime PeriodStartDate, DateTime PeriodEndDate) : DomainEvent;

public record TrialBalanceRecalculated(DefaultIdType Id, string TrialBalanceNumber, decimal TotalDebits, decimal TotalCredits, bool IsBalanced) : DomainEvent;

public record TrialBalanceFinalized(DefaultIdType Id, string TrialBalanceNumber, string FinalizedBy, DateTime FinalizedDate, decimal TotalDebits, decimal TotalCredits) : DomainEvent;

public record TrialBalanceReopened(DefaultIdType Id, string TrialBalanceNumber, string Reason) : DomainEvent;

public record TrialBalanceDeleted(DefaultIdType Id) : DomainEvent;

