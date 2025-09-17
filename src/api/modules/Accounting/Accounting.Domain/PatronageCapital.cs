namespace Accounting.Domain;

/// <summary>
/// Represents a cooperative's patronage capital allocation and retirement for a member in a fiscal year.
/// </summary>
/// <remarks>
/// Tracks amounts allocated and retired, and status transitions (Allocated, PartiallyRetired, Retired).
/// Defaults: <see cref="AmountRetired"/> starts at 0; <see cref="Status"/> starts as "Allocated".
/// </remarks>
public class PatronageCapital : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// The member who receives the allocation.
    /// </summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>
    /// The fiscal year of the allocation.
    /// </summary>
    public int FiscalYear { get; private set; }

    /// <summary>
    /// Total capital amount allocated for the year.
    /// </summary>
    public decimal AmountAllocated { get; private set; }

    /// <summary>
    /// Cumulative amount retired from the allocation.
    /// </summary>
    public decimal AmountRetired { get; private set; }

    /// <summary>
    /// Status of the allocation lifecycle: Allocated, Retired, or PartiallyRetired.
    /// </summary>
    public string Status { get; private set; } // Allocated, Retired, PartiallyRetired

    private PatronageCapital()
    {
        // EF Core parameterless constructor - initialize non-nullable properties to safe defaults
        MemberId = DefaultIdType.Empty;
        FiscalYear = 0;
        AmountAllocated = 0m;
        AmountRetired = 0m;
        Status = string.Empty;
    }

    private PatronageCapital(DefaultIdType memberId, int fiscalYear, decimal amountAllocated, string? description = null, string? notes = null)
    {
        MemberId = memberId;
        FiscalYear = fiscalYear;
        AmountAllocated = amountAllocated;
        AmountRetired = 0m;
        Status = "Allocated";
        Description = description?.Trim();
        Notes = notes?.Trim();
    }

    /// <summary>
    /// Create a new patronage capital allocation with validation for fiscal year and positive amount.
    /// </summary>
    public static PatronageCapital Create(DefaultIdType memberId, int fiscalYear, decimal amountAllocated, string? description = null, string? notes = null)
    {
        if (fiscalYear <= 1900)
            throw new ArgumentException("Invalid fiscal year.");
        if (amountAllocated <= 0)
            throw new ArgumentException("Allocated amount must be positive.");

        return new PatronageCapital(memberId, fiscalYear, amountAllocated, description, notes);
    }

    /// <summary>
    /// Retire a portion of the allocation; updates status to PartiallyRetired or Retired.
    /// </summary>
    public PatronageCapital Retire(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Retirement amount must be positive.");
        if (amount > (AmountAllocated - AmountRetired))
            throw new InvalidOperationException("Cannot retire more than the remaining allocated amount.");

        AmountRetired += amount;
        Status = AmountRetired == AmountAllocated ? "Retired" : "PartiallyRetired";
        return this;
    }

    /// <summary>
    /// Adjust the allocated amount; must not fall below the already retired amount.
    /// </summary>
    public PatronageCapital UpdateAllocatedAmount(decimal newAmount)
    {
        if (newAmount <= 0)
            throw new ArgumentException("Allocated amount must be positive.");
        if (newAmount < AmountRetired)
            throw new InvalidOperationException("Allocated amount cannot be less than already retired amount.");

        AmountAllocated = newAmount;
        Status = AmountRetired == AmountAllocated ? "Retired" : "Allocated";
        return this;
    }
}
