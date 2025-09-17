using Accounting.Domain.Events.AccountingPeriod;

namespace Accounting.Domain;

public class AccountingPeriod : AuditableEntity, IAggregateRoot
{
    private const int MaxNameLength = 1024; // aligns with AuditableEntity.Name VARCHAR(1024)
    private const int MaxDescriptionLength = 2048; // aligns with AuditableEntity.Description
    private const int MaxNotesLength = 2048;
    private const int MaxPeriodTypeLength = 16;
    private static readonly HashSet<string> AllowedPeriodTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "Monthly",
        "Quarterly",
        "Yearly",
        "Annual",
    };

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsClosed { get; private set; }
    public bool IsAdjustmentPeriod { get; private set; }
    public int FiscalYear { get; private set; }
    public string PeriodType { get; private set; } = string.Empty; // Monthly, Quarterly, Yearly

    // Parameterless constructor for EF Core
    private AccountingPeriod()
    {
    }

    private AccountingPeriod(string periodName, DateTime startDate, DateTime endDate,
        int fiscalYear, string periodType, bool isAdjustmentPeriod = false, string? description = null, string? notes = null)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(periodName))
            throw new AccountingPeriodInvalidNameException("Accounting period name is required.");

        if (periodName.Trim().Length > MaxNameLength)
            throw new AccountingPeriodInvalidNameException($"Accounting period name cannot exceed {MaxNameLength} characters.");

        if (startDate >= endDate)
            throw new InvalidAccountingPeriodDateRangeException();

        if (fiscalYear < 1900 || fiscalYear > 2100)
            throw new AccountingPeriodInvalidFiscalYearException(fiscalYear);

        if (string.IsNullOrWhiteSpace(periodType))
            throw new AccountingPeriodInvalidPeriodTypeException(periodType);

        if (periodType.Trim().Length > MaxPeriodTypeLength)
            throw new AccountingPeriodInvalidPeriodTypeException(periodType);

        if (!AllowedPeriodTypes.Contains(periodType))
            throw new AccountingPeriodInvalidPeriodTypeException(periodType);

        Name = periodName.Trim();
        StartDate = startDate;
        EndDate = endDate;
        IsClosed = false;
        IsAdjustmentPeriod = isAdjustmentPeriod;
        FiscalYear = fiscalYear;
        PeriodType = periodType.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();

        if (Description?.Length > MaxDescriptionLength)
            Description = Description.Substring(0, MaxDescriptionLength);
        if (Notes?.Length > MaxNotesLength)
            Notes = Notes.Substring(0, MaxNotesLength);

        QueueDomainEvent(new AccountingPeriodCreated(Id, Name, StartDate, EndDate, FiscalYear, Description, Notes));
    }

    public static AccountingPeriod Create(string periodName, DateTime startDate, DateTime endDate,
        int fiscalYear, string periodType, bool isAdjustmentPeriod = false, string? description = null, string? notes = null)
    {
        // Domain-level validation occurs in the private constructor
        return new AccountingPeriod(periodName, startDate, endDate, fiscalYear, periodType, isAdjustmentPeriod, description, notes);
    }

    public AccountingPeriod Update(string? periodName, DateTime? startDate, DateTime? endDate,
        int? fiscalYear, string? periodType, bool isAdjustmentPeriod = false, string? description = null, string? notes = null)
    {
        bool isUpdated = false;

        if (IsClosed)
            throw new AccountingPeriodCannotBeModifiedException(Id);

        // Validate proposed date changes
        DateTime newStart = startDate ?? StartDate;
        DateTime newEnd = endDate ?? EndDate;
        if (newStart >= newEnd)
            throw new InvalidAccountingPeriodDateRangeException();

        if (!string.IsNullOrWhiteSpace(periodName) && Name != periodName)
        {
            if (periodName.Trim().Length > MaxNameLength)
                throw new AccountingPeriodInvalidNameException($"Accounting period name cannot exceed {MaxNameLength} characters.");

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
            if (fiscalYear.Value < 1900 || fiscalYear.Value > 2100)
                throw new AccountingPeriodInvalidFiscalYearException(fiscalYear.Value);

            FiscalYear = fiscalYear.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(periodType) && PeriodType != periodType)
        {
            if (periodType.Trim().Length > MaxPeriodTypeLength)
                throw new AccountingPeriodInvalidPeriodTypeException(periodType);
            if (!AllowedPeriodTypes.Contains(periodType))
                throw new AccountingPeriodInvalidPeriodTypeException(periodType);

            PeriodType = periodType.Trim();
            isUpdated = true;
        }

        if (IsAdjustmentPeriod != isAdjustmentPeriod)
        {
            IsAdjustmentPeriod = isAdjustmentPeriod;
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            if (Description?.Length > MaxDescriptionLength)
                Description = Description!.Substring(0, MaxDescriptionLength);
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            if (Notes?.Length > MaxNotesLength)
                Notes = Notes!.Substring(0, MaxNotesLength);
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
