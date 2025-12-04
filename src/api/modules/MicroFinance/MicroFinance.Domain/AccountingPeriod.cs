using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents an accounting period (month/quarter/year).
/// </summary>
public sealed class AccountingPeriod : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int NameMaxLength = 64;
    public const int StatusMaxLength = 32;
    public const int TypeMaxLength = 32;
    
    // Period Status
    public const string StatusOpen = "Open";
    public const string StatusClosed = "Closed";
    public const string StatusLocked = "Locked";
    
    // Period Types
    public const string TypeMonth = "Month";
    public const string TypeQuarter = "Quarter";
    public const string TypeYear = "Year";

    public string Name { get; private set; } = default!;
    public string PeriodType { get; private set; } = TypeMonth;
    public string Status { get; private set; } = StatusOpen;
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public int FiscalYear { get; private set; }
    public int PeriodNumber { get; private set; }
    public bool IsAdjustmentPeriod { get; private set; }
    public Guid? ParentPeriodId { get; private set; }
    public DateTimeOffset? ClosedAt { get; private set; }
    public Guid? ClosedById { get; private set; }
    public DateTimeOffset? LockedAt { get; private set; }
    public Guid? LockedById { get; private set; }
    public decimal? OpeningBalance { get; private set; }
    public decimal? ClosingBalance { get; private set; }
    public int TransactionCount { get; private set; }
    public string? Notes { get; private set; }

    private AccountingPeriod() { }

    public static AccountingPeriod Create(
        string name,
        string periodType,
        DateOnly startDate,
        DateOnly endDate,
        int fiscalYear,
        int periodNumber,
        bool isAdjustmentPeriod = false,
        Guid? parentPeriodId = null)
    {
        var period = new AccountingPeriod
        {
            Name = name,
            PeriodType = periodType,
            StartDate = startDate,
            EndDate = endDate,
            FiscalYear = fiscalYear,
            PeriodNumber = periodNumber,
            IsAdjustmentPeriod = isAdjustmentPeriod,
            ParentPeriodId = parentPeriodId,
            Status = StatusOpen,
            TransactionCount = 0
        };

        period.QueueDomainEvent(new AccountingPeriodCreated(period));
        return period;
    }

    public AccountingPeriod SetOpeningBalance(decimal balance)
    {
        OpeningBalance = balance;
        return this;
    }

    public AccountingPeriod IncrementTransactionCount()
    {
        TransactionCount++;
        return this;
    }

    public AccountingPeriod Close(Guid closedById, decimal closingBalance)
    {
        Status = StatusClosed;
        ClosedById = closedById;
        ClosedAt = DateTimeOffset.UtcNow;
        ClosingBalance = closingBalance;
        QueueDomainEvent(new AccountingPeriodClosed(Id, Name, closingBalance));
        return this;
    }

    public AccountingPeriod Reopen(string reason)
    {
        if (Status == StatusLocked)
            throw new InvalidOperationException("Cannot reopen a locked period.");

        Status = StatusOpen;
        ClosedAt = null;
        ClosedById = null;
        Notes = reason;
        QueueDomainEvent(new AccountingPeriodReopened(Id, Name));
        return this;
    }

    public AccountingPeriod Lock(Guid lockedById)
    {
        if (Status != StatusClosed)
            throw new InvalidOperationException("Period must be closed before locking.");

        Status = StatusLocked;
        LockedById = lockedById;
        LockedAt = DateTimeOffset.UtcNow;
        QueueDomainEvent(new AccountingPeriodLocked(Id, Name));
        return this;
    }

    public AccountingPeriod Update(string? notes = null)
    {
        if (notes is not null) Notes = notes;
        QueueDomainEvent(new AccountingPeriodUpdated(this));
        return this;
    }

    public bool IsDateInPeriod(DateOnly date)
    {
        return date >= StartDate && date <= EndDate;
    }
}
