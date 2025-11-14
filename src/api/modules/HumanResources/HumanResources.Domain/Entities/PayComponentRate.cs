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
        int year)
    {
        Id = id;
        PayComponentId = payComponentId;
        MinAmount = minAmount;
        MaxAmount = maxAmount;
        Year = year;
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
    /// Year this rate is effective (e.g., 2025).
    /// </summary>
    public int Year { get; private set; }

    /// <summary>
    /// Effective start date for this rate.
    /// </summary>
    public DateTime? EffectiveStartDate { get; private set; }

    /// <summary>
    /// Effective end date for this rate.
    /// </summary>
    public DateTime? EffectiveEndDate { get; private set; }

    /// <summary>
    /// Whether this rate is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Description of this rate/bracket.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Creates a new pay component rate for SSS/PhilHealth/Pag-IBIG.
    /// </summary>
    public static PayComponentRate CreateContributionRate(
        DefaultIdType payComponentId,
        decimal minAmount,
        decimal maxAmount,
        decimal employeeRate,
        decimal employerRate,
        int year,
        decimal? additionalEmployerRate = null)
    {
        var rate = new PayComponentRate(
            DefaultIdType.NewGuid(),
            payComponentId,
            minAmount,
            maxAmount,
            year);

        rate.EmployeeRate = employeeRate;
        rate.EmployerRate = employerRate;
        rate.AdditionalEmployerRate = additionalEmployerRate;

        return rate;
    }

    /// <summary>
    /// Creates a new pay component rate for graduated income tax.
    /// </summary>
    public static PayComponentRate CreateTaxBracket(
        DefaultIdType payComponentId,
        decimal minAmount,
        decimal maxAmount,
        decimal baseAmount,
        decimal excessRate,
        int year)
    {
        var rate = new PayComponentRate(
            DefaultIdType.NewGuid(),
            payComponentId,
            minAmount,
            maxAmount,
            year);

        rate.BaseAmount = baseAmount;
        rate.ExcessRate = excessRate;
        rate.TaxRate = excessRate; // For compatibility

        return rate;
    }

    /// <summary>
    /// Creates a new pay component rate with fixed amounts.
    /// </summary>
    public static PayComponentRate CreateFixedRate(
        DefaultIdType payComponentId,
        decimal minAmount,
        decimal maxAmount,
        decimal? employeeAmount,
        decimal? employerAmount,
        int year)
    {
        var rate = new PayComponentRate(
            DefaultIdType.NewGuid(),
            payComponentId,
            minAmount,
            maxAmount,
            year);

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
    /// Sets effective date range.
    /// </summary>
    public PayComponentRate SetEffectiveDates(DateTime startDate, DateTime? endDate = null)
    {
        EffectiveStartDate = startDate;
        EffectiveEndDate = endDate;
        return this;
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

