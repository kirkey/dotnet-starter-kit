using Accounting.Domain.Events.PatronageCapital;

namespace Accounting.Domain;

/// <summary>
/// Represents a cooperative's patronage capital allocation and retirement for a member in a specific fiscal year.
/// </summary>
/// <remarks>
/// Use cases:
/// - Allocate patronage capital to cooperative members based on their annual patronage (purchases/usage).
/// - Track capital credits accumulated by members over multiple years for equity accounting.
/// - Manage patronage capital retirements when the cooperative returns capital to members.
/// - Support cooperative financial reporting and member equity calculations.
/// - Enable regulatory compliance for cooperative capital structure requirements.
/// - Track partial retirements over multiple years based on board retirement policies.
/// - Generate member capital credit statements and retirement notices.
/// - Support estate settlements and membership transfers with capital credit tracking.
/// 
/// Default values:
/// - MemberId: required reference to member receiving the allocation
/// - FiscalYear: required year for the allocation (example: 2025)
/// - AmountAllocated: required positive amount allocated (example: 1250.00 based on annual patronage)
/// - AmountRetired: 0.00 (starts with no retirements)
/// - Status: "Allocated" (new allocations start as allocated)
/// - AllocationDate: date when allocation is recorded (typically end of fiscal year)
/// - RetirementDate: null (set when retirement begins)
/// - RetirementMethod: null (set when retirement policy is applied)
/// 
/// Business rules:
/// - AmountAllocated must be positive
/// - AmountRetired cannot exceed AmountAllocated
/// - FiscalYear must be valid (not future year)
/// - Cannot retire capital before board approval
/// - Retirement follows FIFO (first allocated, first retired) or board policy
/// - Member must be active to receive new allocations
/// - Estate transfers require proper documentation
/// - Tax reporting required for allocations and retirements
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.PatronageCapital.PatronageCapitalAllocated"/>
/// <seealso cref="Accounting.Domain.Events.PatronageCapital.PatronageCapitalRetired"/>
/// <seealso cref="Accounting.Domain.Events.PatronageCapital.PatronageCapitalPartiallyRetired"/>
/// <seealso cref="Accounting.Domain.Events.PatronageCapital.PatronageCapitalTransferred"/>
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

        QueueDomainEvent(new PatronageCapitalAllocated(Id, memberId, fiscalYear, amountAllocated, DateTime.UtcNow));
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

        var retirementDate = DateTime.UtcNow;
        if (Status == "Retired")
        {
            QueueDomainEvent(new PatronageCapitalRetired(Id, MemberId, FiscalYear, amount, retirementDate, null));
        }
        else
        {
            var remaining = AmountAllocated - AmountRetired;
            QueueDomainEvent(new PatronageCapitalPartiallyRetired(Id, MemberId, FiscalYear, amount, remaining, retirementDate, null));
        }

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
