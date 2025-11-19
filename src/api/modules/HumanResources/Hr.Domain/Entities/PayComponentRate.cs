namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

using Events;

/// <summary>
/// Represents rates/brackets for pay components (SSS, PhilHealth, Pag-IBIG, tax brackets).
/// Allows database-driven configuration of Philippine labor law rates.
/// </summary>
/// <remarks>
/// Examples:
/// - SSS bracket: MinAmount=4000, MaxAmount=4250, EmployeeRate=0.045, EmployerRate=0.095
/// - Tax bracket: MinAmount=250000, MaxAmount=400000, Rate=0.15, BaseAmount=0
/// - PhilHealth: MinAmount=10000, MaxAmount=100000, EmployeeRate=0.02, EmployerRate=0.02
/// </remarks>
public class PayComponentRate : AuditableEntity, IAggregateRoot
{
    private PayComponentRate() { }

    private PayComponentRate(
        DefaultIdType id,
        DefaultIdType payComponentId,
        decimal minAmount,
        decimal maxAmount,
        DateTime effectiveStartDate,
        DateTime? effectiveEndDate = null)
    {
        Id = id;
        PayComponentId = payComponentId;
        MinAmount = minAmount;
        MaxAmount = maxAmount;
        EffectiveStartDate = effectiveStartDate;
        EffectiveEndDate = effectiveEndDate;
        Year = effectiveStartDate.Year; // Derived from start date
        IsActive = true;
    }

    /// <summary>
    /// The pay component this rate belongs to.
    /// </summary>
    public DefaultIdType PayComponentId { get; private set; }
    public PayComponent PayComponent { get; private set; } = default!;

    /// <summary>
    /// Minimum amount for this bracket/rate.
    /// </summary>
    public decimal MinAmount { get; private set; }

    /// <summary>
    /// Maximum amount for this bracket/rate.
    /// </summary>
    public decimal MaxAmount { get; private set; }

    /// <summary>
    /// Employee contribution rate (for SSS, PhilHealth, Pag-IBIG).
    /// Example: 0.045 = 4.5%
    /// </summary>
    public decimal? EmployeeRate { get; private set; }

    /// <summary>
    /// Employer contribution rate (for SSS, PhilHealth, Pag-IBIG).
    /// Example: 0.095 = 9.5%
    /// </summary>
    public decimal? EmployerRate { get; private set; }

    /// <summary>
    /// Additional employer contribution (for SSS EC).
    /// Example: 0.01 = 1%
    /// </summary>
    public decimal? AdditionalEmployerRate { get; private set; }

    /// <summary>
    /// Fixed employee contribution amount.
    /// </summary>
    public decimal? EmployeeAmount { get; private set; }

    /// <summary>
    /// Fixed employer contribution amount.
    /// </summary>
    public decimal? EmployerAmount { get; private set; }

    /// <summary>
    /// Tax rate for this bracket (for income tax).
    /// Example: 0.15 = 15%
    /// </summary>
    public decimal? TaxRate { get; private set; }

    /// <summary>
    /// Base tax amount for this bracket (for graduated tax).
    /// Example: ₱22,500 for the ₱400K-₱800K bracket.
    /// </summary>
    public decimal? BaseAmount { get; private set; }

    /// <summary>
    /// Rate applied to excess over minimum (for graduated tax).
    /// Example: 0.20 = 20% of excess over ₱400K.
    /// </summary>
    public decimal? ExcessRate { get; private set; }

    /// <summary>
    /// Effective start date for this rate (REQUIRED).
    /// Rate becomes valid from this date forward.
    /// Example: January 1, 2025 for new SSS contribution table.
    /// </summary>
    public DateTime EffectiveStartDate { get; private set; }

    /// <summary>
    /// Effective end date for this rate (OPTIONAL).
    /// If null, rate is valid indefinitely until superseded.
    /// When a new rate is created, the previous rate's EndDate should be set.
    /// Example: December 31, 2025 when new 2026 rates take effect.
    /// </summary>
    public DateTime? EffectiveEndDate { get; private set; }

    /// <summary>
    /// Year this rate is effective (DERIVED from EffectiveStartDate).
    /// Used for quick year-based filtering.
    /// </summary>
    public int Year { get; private set; }

    /// <summary>
    /// Whether this rate is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Description of this rate/bracket.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Creates a new pay component rate with temporal validity.
    /// </summary>
    /// <param name="payComponentId">The pay component this rate belongs to.</param>
    /// <param name="minAmount">Minimum bracket amount.</param>
    /// <param name="maxAmount">Maximum bracket amount.</param>
    /// <param name="effectiveStartDate">Date when this rate becomes effective (REQUIRED).</param>
    /// <param name="effectiveEndDate">Date when this rate expires (optional - null means indefinite).</param>
    public static PayComponentRate Create(
        DefaultIdType payComponentId,
        decimal minAmount,
        decimal maxAmount,
        DateTime effectiveStartDate,
        DateTime? effectiveEndDate = null)
    {
        if (effectiveEndDate.HasValue && effectiveEndDate.Value <= effectiveStartDate)
            throw new ArgumentException("Effective end date must be after start date.", nameof(effectiveEndDate));

        var rate = new PayComponentRate(
            DefaultIdType.NewGuid(),
            payComponentId,
            minAmount,
            maxAmount,
            effectiveStartDate,
            effectiveEndDate);

        return rate;
    }

    /// <summary>
    /// Creates a new pay component rate for SSS/PhilHealth/Pag-IBIG contributions with temporal validity.
    /// </summary>
    /// <param name="payComponentId">The pay component (SSS, PhilHealth, or Pag-IBIG).</param>
    /// <param name="minAmount">Minimum salary bracket.</param>
    /// <param name="maxAmount">Maximum salary bracket.</param>
    /// <param name="employeeRate">Employee contribution rate (e.g., 0.045 for 4.5%).</param>
    /// <param name="employerRate">Employer contribution rate (e.g., 0.095 for 9.5%).</param>
    /// <param name="effectiveStartDate">Date when this contribution rate becomes effective.</param>
    /// <param name="effectiveEndDate">Date when this rate expires (optional).</param>
    /// <param name="additionalEmployerRate">Additional employer rate like SSS EC (optional).</param>
    public static PayComponentRate CreateContributionRate(
        DefaultIdType payComponentId,
        decimal minAmount,
        decimal maxAmount,
        decimal employeeRate,
        decimal employerRate,
        DateTime effectiveStartDate,
        DateTime? effectiveEndDate = null,
        decimal? additionalEmployerRate = null)
    {
        var rate = Create(payComponentId, minAmount, maxAmount, effectiveStartDate, effectiveEndDate);

        rate.EmployeeRate = employeeRate;
        rate.EmployerRate = employerRate;
        rate.AdditionalEmployerRate = additionalEmployerRate;

        return rate;
    }

    /// <summary>
    /// Creates a new pay component rate for graduated income tax (BIR tax tables) with temporal validity.
    /// </summary>
    /// <param name="payComponentId">The tax component.</param>
    /// <param name="minAmount">Minimum taxable income for this bracket.</param>
    /// <param name="maxAmount">Maximum taxable income for this bracket.</param>
    /// <param name="baseAmount">Base tax for this bracket.</param>
    /// <param name="excessRate">Tax rate on excess over minimum.</param>
    /// <param name="effectiveStartDate">Date when this tax bracket becomes effective.</param>
    /// <param name="effectiveEndDate">Date when this bracket expires (optional).</param>
    public static PayComponentRate CreateTaxBracket(
        DefaultIdType payComponentId,
        decimal minAmount,
        decimal maxAmount,
        decimal baseAmount,
        decimal excessRate,
        DateTime effectiveStartDate,
        DateTime? effectiveEndDate = null)
    {
        var rate = Create(payComponentId, minAmount, maxAmount, effectiveStartDate, effectiveEndDate);

        rate.BaseAmount = baseAmount;
        rate.ExcessRate = excessRate;
        rate.TaxRate = excessRate; // For compatibility

        return rate;
    }

    /// <summary>
    /// Creates a new pay component rate with fixed contribution amounts (for flat-rate contributions).
    /// </summary>
    /// <param name="payComponentId">The pay component.</param>
    /// <param name="minAmount">Minimum salary bracket.</param>
    /// <param name="maxAmount">Maximum salary bracket.</param>
    /// <param name="employeeAmount">Fixed employee contribution amount.</param>
    /// <param name="employerAmount">Fixed employer contribution amount.</param>
    /// <param name="effectiveStartDate">Date when this rate becomes effective.</param>
    /// <param name="effectiveEndDate">Date when this rate expires (optional).</param>
    public static PayComponentRate CreateFixedRate(
        DefaultIdType payComponentId,
        decimal minAmount,
        decimal maxAmount,
        decimal? employeeAmount,
        decimal? employerAmount,
        DateTime effectiveStartDate,
        DateTime? effectiveEndDate = null)
    {
        var rate = Create(payComponentId, minAmount, maxAmount, effectiveStartDate, effectiveEndDate);

        rate.EmployeeAmount = employeeAmount;
        rate.EmployerAmount = employerAmount;

        return rate;
    }

    /// <summary>
    /// Updates the rate information.
    /// </summary>
    public PayComponentRate Update(
        decimal? employeeRate = null,
        decimal? employerRate = null,
        decimal? additionalEmployerRate = null,
        decimal? employeeAmount = null,
        decimal? employerAmount = null,
        decimal? taxRate = null,
        decimal? baseAmount = null,
        decimal? excessRate = null,
        string? description = null)
    {
        if (employeeRate.HasValue)
            EmployeeRate = employeeRate.Value;

        if (employerRate.HasValue)
            EmployerRate = employerRate.Value;

        if (additionalEmployerRate.HasValue)
            AdditionalEmployerRate = additionalEmployerRate.Value;

        if (employeeAmount.HasValue)
            EmployeeAmount = employeeAmount.Value;

        if (employerAmount.HasValue)
            EmployerAmount = employerAmount.Value;

        if (taxRate.HasValue)
            TaxRate = taxRate.Value;

        if (baseAmount.HasValue)
            BaseAmount = baseAmount.Value;

        if (excessRate.HasValue)
            ExcessRate = excessRate.Value;

        if (description != null)
            Description = description;

        return this;
    }

    /// <summary>
    /// Sets effective date range and updates derived year.
    /// </summary>
    public PayComponentRate SetEffectiveDates(DateTime startDate, DateTime? endDate = null)
    {
        if (endDate.HasValue && endDate.Value <= startDate)
            throw new ArgumentException("End date must be after start date.", nameof(endDate));

        EffectiveStartDate = startDate;
        EffectiveEndDate = endDate;
        Year = startDate.Year; // Update derived year
        
        return this;
    }

    /// <summary>
    /// Terminates this rate by setting the end date.
    /// Used when a new rate supersedes this one.
    /// </summary>
    /// <param name="endDate">The date this rate becomes invalid.</param>
    public PayComponentRate Terminate(DateTime endDate)
    {
        if (endDate <= EffectiveStartDate)
            throw new ArgumentException("End date must be after start date.", nameof(endDate));

        EffectiveEndDate = endDate;
        IsActive = false;
        
        return this;
    }

    /// <summary>
    /// Checks if this rate is valid/effective for a specific date.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns>True if the rate is effective on the given date.</returns>
    public bool IsEffectiveOn(DateTime date)
    {
        if (!IsActive)
            return false;

        // Date must be on or after start date
        if (date < EffectiveStartDate.Date)
            return false;

        // If end date is set, date must be before or on end date
        if (EffectiveEndDate.HasValue && date > EffectiveEndDate.Value.Date)
            return false;

        return true;
    }

    /// <summary>
    /// Checks if this rate's date range overlaps with another date range.
    /// Used for validation to prevent overlapping rates for the same component/bracket.
    /// </summary>
    public bool OverlapsWith(DateTime otherStart, DateTime? otherEnd)
    {
        // If this rate has no end date, it's valid indefinitely
        var thisEnd = EffectiveEndDate ?? DateTime.MaxValue;
        var compareEnd = otherEnd ?? DateTime.MaxValue;

        // Check for overlap
        return EffectiveStartDate <= compareEnd && otherStart <= thisEnd;
    }

    /// <summary>
    /// Checks if an amount falls within this rate's bracket.
    /// </summary>
    /// <param name="amount">The amount to check.</param>
    /// <returns>True if amount is within MinAmount and MaxAmount.</returns>
    public bool IsInBracket(decimal amount)
    {
        return amount >= MinAmount && amount <= MaxAmount;
    }

    /// <summary>
    /// Gets the applicable rate for a specific date and amount.
    /// Returns null if this rate is not applicable.
    /// </summary>
    public PayComponentRate? GetApplicableRate(DateTime date, decimal amount)
    {
        if (IsEffectiveOn(date) && IsInBracket(amount))
            return this;
        
        return null;
    }

    /// <summary>
    /// Deactivates the rate.
    /// </summary>
    public PayComponentRate Deactivate()
    {
        IsActive = false;
        return this;
    }

    /// <summary>
    /// Activates the rate.
    /// </summary>
    public PayComponentRate Activate()
    {
        IsActive = true;
        return this;
    }
}

