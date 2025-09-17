using Accounting.Domain.Events.AccountingPeriod;

namespace Accounting.Domain;

/// <summary>
/// Defines a fiscal accounting period (monthly, quarterly, yearly) used to group and control financial postings.
/// </summary>
/// <remarks>
/// An <see cref="AccountingPeriod"/> establishes a start and end date for financial activity, tracks whether the
/// period is closed to prevent further modification, and records metadata like fiscal year and period type.
/// Defaults: <see cref="IsClosed"/> is false on creation; <see cref="IsAdjustmentPeriod"/> is false unless specified.
/// </remarks>
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

    /// <summary>
    /// Inclusive start date of the accounting period.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// Inclusive end date of the accounting period. Must be after <see cref="StartDate"/>.
    /// </summary>
    public DateTime EndDate { get; private set; }

    /// <summary>
    /// Whether the period has been closed to postings and updates.
    /// </summary>
    /// <remarks>Defaults to <c>false</c> on creation and becomes <c>true</c> after calling <see cref="Close"/>.</remarks>
    public bool IsClosed { get; private set; }

    /// <summary>
    /// Indicates if this is an adjustment period (e.g., period 13).
    /// </summary>
    /// <remarks>Defaults to <c>false</c> unless specified.</remarks>
    public bool IsAdjustmentPeriod { get; private set; }

    /// <summary>
    /// The fiscal year the period belongs to. Enforced to be within a reasonable range (1900-2100).
    /// </summary>
    public int FiscalYear { get; private set; }

    /// <summary>
    /// The period granularity, e.g. "Monthly", "Quarterly", or "Yearly".
    /// </summary>
    /// <remarks>Trimmed and validated against <see cref="AllowedPeriodTypes"/>. Defaults to empty for EF constructor.</remarks>
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

        if (fiscalYear is < 1900 or > 2100)
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

    /// <summary>
    /// Factory method to create a new accounting period with validation for date range, fiscal year, and period type.
    /// </summary>
    public static AccountingPeriod Create(string periodName, DateTime startDate, DateTime endDate,
        int fiscalYear, string periodType, bool isAdjustmentPeriod = false, string? description = null, string? notes = null)
    {
        // Domain-level validation occurs in the private constructor
        return new AccountingPeriod(periodName, startDate, endDate, fiscalYear, periodType, isAdjustmentPeriod, description, notes);
    }

    /// <summary>
    /// Update mutable fields while enforcing invariants. Throws if the period is already closed.
    /// </summary>
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
            if (fiscalYear.Value is < 1900 or > 2100)
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

    /// <summary>
    /// Close the period to further changes and postings.
    /// </summary>
    public AccountingPeriod Close()
    {
        if (IsClosed)
            throw new AccountingPeriodAlreadyClosedException(Id);

        IsClosed = true;
        QueueDomainEvent(new AccountingPeriodClosed(Id, Name, EndDate));
        return this;
    }

    /// <summary>
    /// Reopen a previously closed period to allow changes.
    /// </summary>
    public AccountingPeriod Reopen()
    {
        if (!IsClosed)
            throw new AccountingPeriodNotClosedException(Id);

        IsClosed = false;
        QueueDomainEvent(new AccountingPeriodReopened(Id, Name));
        return this;
    }

    /// <summary>
    /// Checks whether the provided date falls within the current period (inclusive).
    /// </summary>
    public bool IsDateInPeriod(DateTime date)
    {
        return date >= StartDate && date <= EndDate;
    }
}
