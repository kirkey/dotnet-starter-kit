using Accounting.Domain.Events.AccountingPeriod;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class AccountingPeriod : AuditableEntity, IAggregateRoot
{
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsClosed { get; private set; }
    public bool IsAdjustmentPeriod { get; private set; }
    public int FiscalYear { get; private set; }
    public string PeriodType { get; private set; } // Monthly, Quarterly, Yearly

    // Parameterless constructor for EF Core
    private AccountingPeriod()
    {
    }

    private AccountingPeriod(string periodName, DateTime startDate, DateTime endDate,
        int fiscalYear, string periodType, bool isAdjustmentPeriod = false, string? description = null, string? notes = null)
    {
        Name = periodName.Trim();
        StartDate = startDate;
        EndDate = endDate;
        IsClosed = false;
        IsAdjustmentPeriod = isAdjustmentPeriod;
        FiscalYear = fiscalYear;
        PeriodType = periodType.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new AccountingPeriodCreated(Id, Name, StartDate, EndDate, FiscalYear, Description, Notes));
    }

    public static AccountingPeriod Create(string periodName, DateTime startDate, DateTime endDate,
        int fiscalYear, string periodType, bool isAdjustmentPeriod = false, string? description = null, string? notes = null)
    {
        if (startDate >= endDate)
            throw new InvalidAccountingPeriodDateRangeException();

        return new AccountingPeriod(periodName, startDate, endDate, fiscalYear, periodType, isAdjustmentPeriod, description, notes);
    }

    public AccountingPeriod Update(string? periodName, DateTime? startDate, DateTime? endDate,
        int? fiscalYear, string? periodType, bool? isAdjustmentPeriod, string? description, string? notes)
    {
        bool isUpdated = false;

        if (IsClosed)
            throw new AccountingPeriodCannotBeModifiedException(Id);

        if (!string.IsNullOrWhiteSpace(periodName) && Name != periodName)
        {
            Name = periodName.Trim();
            isUpdated = true;
        }

        if (startDate.HasValue && StartDate != startDate.Value)
        {
            StartDate = startDate.Value;
            isUpdated = true;
        }

        if (endDate.HasValue && EndDate != endDate.Value)
        {
            EndDate = endDate.Value;
            isUpdated = true;
        }

        if (fiscalYear.HasValue && FiscalYear != fiscalYear.Value)
        {
            FiscalYear = fiscalYear.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(periodType) && PeriodType != periodType)
        {
            PeriodType = periodType.Trim();
            isUpdated = true;
        }

        if (isAdjustmentPeriod.HasValue && IsAdjustmentPeriod != isAdjustmentPeriod.Value)
        {
            IsAdjustmentPeriod = isAdjustmentPeriod.Value;
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new AccountingPeriodUpdated(this));
        }

        return this;
    }

    public AccountingPeriod Close()
    {
        if (IsClosed)
            throw new AccountingPeriodAlreadyClosedException(Id);

        IsClosed = true;
        QueueDomainEvent(new AccountingPeriodClosed(Id, Name, EndDate));
        return this;
    }

    public AccountingPeriod Reopen()
    {
        if (!IsClosed)
            throw new AccountingPeriodNotClosedException(Id);

        IsClosed = false;
        QueueDomainEvent(new AccountingPeriodReopened(Id, Name));
        return this;
    }

    public bool IsDateInPeriod(DateTime date)
    {
        return date >= StartDate && date <= EndDate;
    }
}
