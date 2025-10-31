namespace Accounting.Domain.Events.RetainedEarnings;

public record RetainedEarningsCreated(DefaultIdType Id, int FiscalYear, decimal OpeningBalance, DateTime FiscalYearStartDate, DateTime FiscalYearEndDate, string? Description, string? Notes) : DomainEvent;

public record RetainedEarningsUpdated(DefaultIdType Id, int FiscalYear, decimal ClosingBalance, string? Description, string? Notes) : DomainEvent;

public record RetainedEarningsDeleted(DefaultIdType Id) : DomainEvent;

public record RetainedEarningsNetIncomeUpdated(DefaultIdType Id, int FiscalYear, decimal NetIncome, decimal NewClosingBalance) : DomainEvent;

public record RetainedEarningsDistributionRecorded(DefaultIdType Id, int FiscalYear, decimal Amount, string DistributionType, DateTime DistributionDate, decimal NewClosingBalance) : DomainEvent;

public record RetainedEarningsCapitalContributionRecorded(DefaultIdType Id, int FiscalYear, decimal Amount, string ContributionType, DateTime ContributionDate, decimal NewClosingBalance) : DomainEvent;

public record RetainedEarningsEquityChangeRecorded(DefaultIdType Id, int FiscalYear, decimal Amount, string ChangeType, string Reason, decimal NewClosingBalance) : DomainEvent;

public record RetainedEarningsAppropriated(DefaultIdType Id, int FiscalYear, decimal Amount, string Purpose, decimal TotalAppropriated) : DomainEvent;

public record RetainedEarningsAppropriationReleased(DefaultIdType Id, int FiscalYear, decimal Amount, string Reason, decimal TotalAppropriated) : DomainEvent;

public record RetainedEarningsClosed(DefaultIdType Id, int FiscalYear, decimal ClosingBalance, string ClosedBy, DateTime ClosedDate) : DomainEvent;

public record RetainedEarningsReopened(DefaultIdType Id, int FiscalYear, string Reason) : DomainEvent;

