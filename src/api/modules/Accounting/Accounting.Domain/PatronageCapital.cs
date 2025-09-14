namespace Accounting.Domain;

public class PatronageCapital : AuditableEntity, IAggregateRoot
{
    public DefaultIdType MemberId { get; private set; }
    public int FiscalYear { get; private set; }
    public decimal AmountAllocated { get; private set; }
    public decimal AmountRetired { get; private set; }
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

    public static PatronageCapital Create(DefaultIdType memberId, int fiscalYear, decimal amountAllocated, string? description = null, string? notes = null)
    {
        if (fiscalYear <= 1900)
            throw new ArgumentException("Invalid fiscal year.");
        if (amountAllocated <= 0)
            throw new ArgumentException("Allocated amount must be positive.");

        return new PatronageCapital(memberId, fiscalYear, amountAllocated, description, notes);
    }

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
