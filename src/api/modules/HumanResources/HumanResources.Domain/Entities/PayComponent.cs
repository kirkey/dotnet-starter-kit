namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a pay component (earnings type or deduction type).
/// Fully database-driven configuration for payroll calculations.
/// Supports Philippine labor law compliance through configurable rates and formulas.
/// </summary>
/// <remarks>
/// Examples:
/// - BasicPay: CalculationMethod=Manual, no formula
/// - SSS: CalculationMethod=Bracket, uses PayComponentRates table
/// - OvertimeRegular: CalculationMethod=Formula, formula="HourlyRate * OvertimeHours * 1.25"
/// - NightDifferential: CalculationMethod=Percentage, rate=0.10
/// </remarks>
public class PayComponent : AuditableEntity, IAggregateRoot
{
    private PayComponent() { }

    private PayComponent(
        DefaultIdType id,
        string code,
        string componentName,
        string componentType,
        string calculationMethod,
        string glAccountCode = "")
    {
        Id = id;
        Code = code;
        ComponentName = componentName;
        ComponentType = componentType;
        CalculationMethod = calculationMethod;
        GlAccountCode = glAccountCode;
        IsActive = true;
        IsCalculated = false;
        IsMandatory = false;
        IsSubjectToTax = true;
        IsTaxExempt = false;
        DisplayOrder = 0;
    }

    /// <summary>
    /// Unique code for this component (e.g., "BASIC_PAY", "SSS_EE", "OT_REGULAR").
    /// </summary>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Display name of the pay component.
    /// </summary>
    public string ComponentName { get; private set; } = default!;

    /// <summary>
    /// Type of component: Earnings, Deduction, Tax, EmployerContribution.
    /// </summary>
    public string ComponentType { get; private set; } = default!;

    /// <summary>
    /// Calculation method: Manual, Formula, Percentage, Bracket, Fixed.
    /// </summary>
    public string CalculationMethod { get; private set; } = default!;

    /// <summary>
    /// Formula for calculation (if CalculationMethod = Formula).
    /// Example: "HourlyRate * OvertimeHours * 1.25"
    /// Supports variables: BasicSalary, HourlyRate, DailyRate, OvertimeHours, etc.
    /// </summary>
    public string? CalculationFormula { get; private set; }

    /// <summary>
    /// Rate or percentage (if CalculationMethod = Percentage or Fixed).
    /// Example: 0.10 for 10% night differential, 1.25 for 125% overtime.
    /// </summary>
    public decimal? Rate { get; private set; }

    /// <summary>
    /// Fixed amount (if CalculationMethod = Fixed).
    /// </summary>
    public decimal? FixedAmount { get; private set; }

    /// <summary>
    /// Minimum value for this component.
    /// </summary>
    public decimal? MinValue { get; private set; }

    /// <summary>
    /// Maximum value for this component.
    /// </summary>
    public decimal? MaxValue { get; private set; }

    /// <summary>
    /// GL account code for posting to accounting.
    /// </summary>
    public string GlAccountCode { get; private set; } = default!;

    /// <summary>
    /// Whether the component is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Whether the component is auto-calculated by system.
    /// </summary>
    public bool IsCalculated { get; private set; }

    /// <summary>
    /// Whether this is a mandatory component per Philippine law.
    /// </summary>
    public bool IsMandatory { get; private set; }

    /// <summary>
    /// Whether this component is subject to income tax.
    /// </summary>
    public bool IsSubjectToTax { get; private set; }

    /// <summary>
    /// Whether this component is tax-exempt (de minimis, 13th month up to ₱90K, etc).
    /// </summary>
    public bool IsTaxExempt { get; private set; }

    /// <summary>
    /// Philippine labor law reference (e.g., "Labor Code Art 87", "PD 851", "SSS Law").
    /// </summary>
    public string? LaborLawReference { get; private set; }

    /// <summary>
    /// Description of the component.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Display order in payslip.
    /// </summary>
    public int DisplayOrder { get; private set; }

    /// <summary>
    /// Whether this component affects gross pay calculation.
    /// </summary>
    public bool AffectsGrossPay { get; private set; }

    /// <summary>
    /// Whether this component affects net pay calculation.
    /// </summary>
    public bool AffectsNetPay { get; private set; } = true;

    /// <summary>
    /// Collection of rates/brackets for this component (for SSS, PhilHealth, Pag-IBIG, tax brackets).
    /// </summary>
    public ICollection<PayComponentRate> Rates { get; private set; } = new List<PayComponentRate>();

    /// <summary>
    /// Creates a new pay component.
    /// </summary>
    public static PayComponent Create(
        string code,
        string componentName,
        string componentType,
        string calculationMethod,
        string glAccountCode = "")
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Component code is required.", nameof(code));

        if (string.IsNullOrWhiteSpace(componentName))
            throw new ArgumentException("Component name is required.", nameof(componentName));

        var component = new PayComponent(
            DefaultIdType.NewGuid(),
            code,
            componentName,
            componentType,
            calculationMethod,
            glAccountCode);

        return component;
    }

    /// <summary>
    /// Updates the pay component information.
    /// </summary>
    public PayComponent Update(
        string? componentName = null,
        string? calculationMethod = null,
        string? calculationFormula = null,
        decimal? rate = null,
        decimal? fixedAmount = null,
        string? glAccountCode = null,
        string? description = null,
        int? displayOrder = null)
    {
        if (!string.IsNullOrWhiteSpace(componentName))
            ComponentName = componentName;

        if (!string.IsNullOrWhiteSpace(calculationMethod))
            CalculationMethod = calculationMethod;

        if (calculationFormula != null)
            CalculationFormula = calculationFormula;

        if (rate.HasValue)
            Rate = rate.Value;

        if (fixedAmount.HasValue)
            FixedAmount = fixedAmount.Value;

        if (!string.IsNullOrWhiteSpace(glAccountCode))
            GlAccountCode = glAccountCode;

        if (description != null)
            Description = description;

        if (displayOrder.HasValue)
            DisplayOrder = displayOrder.Value;

        return this;
    }

    /// <summary>
    /// Sets calculation formula.
    /// </summary>
    public PayComponent SetFormula(string formula)
    {
        CalculationFormula = formula;
        CalculationMethod = "Formula";
        return this;
    }

    /// <summary>
    /// Sets rate/percentage.
    /// </summary>
    public PayComponent SetRate(decimal rate)
    {
        Rate = rate;
        CalculationMethod = "Percentage";
        return this;
    }

    /// <summary>
    /// Sets fixed amount.
    /// </summary>
    public PayComponent SetFixedAmount(decimal amount)
    {
        FixedAmount = amount;
        CalculationMethod = "Fixed";
        return this;
    }

    /// <summary>
    /// Sets min/max values.
    /// </summary>
    public PayComponent SetLimits(decimal? minValue, decimal? maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        return this;
    }

    /// <summary>
    /// Marks as mandatory component per Philippine law.
    /// </summary>
    public PayComponent SetMandatory(string? laborLawReference = null)
    {
        IsMandatory = true;
        LaborLawReference = laborLawReference;
        return this;
    }

    /// <summary>
    /// Sets tax treatment.
    /// </summary>
    public PayComponent SetTaxTreatment(bool isSubjectToTax, bool isTaxExempt = false)
    {
        IsSubjectToTax = isSubjectToTax;
        IsTaxExempt = isTaxExempt;
        return this;
    }

    /// <summary>
    /// Sets whether component affects gross/net pay.
    /// </summary>
    public PayComponent SetPayImpact(bool affectsGrossPay, bool affectsNetPay)
    {
        AffectsGrossPay = affectsGrossPay;
        AffectsNetPay = affectsNetPay;
        return this;
    }

    /// <summary>
    /// Marks as auto-calculated.
    /// </summary>
    public PayComponent SetAutoCalculated(bool isCalculated = true)
    {
        IsCalculated = isCalculated;
        return this;
    }

    /// <summary>
    /// Deactivates the pay component.
    /// </summary>
    public PayComponent Deactivate()
    {
        IsActive = false;
        return this;
    }

    /// <summary>
    /// Activates the pay component.
    /// </summary>
    public PayComponent Activate()
    {
        IsActive = true;
        return this;
    }

    // =====================================================
    // Temporal Rate Query Methods (Master-Detail Pattern)
    // =====================================================

    /// <summary>
    /// Gets all rates effective on a specific date.
    /// Example: Get SSS contribution table for payroll date December 15, 2025.
    /// </summary>
    /// <param name="effectiveDate">The date to check (typically payroll date).</param>
    /// <returns>All rate brackets effective on that date.</returns>
    public IEnumerable<PayComponentRate> GetRatesEffectiveOn(DateTime effectiveDate)
    {
        return Rates.Where(r => r.IsEffectiveOn(effectiveDate)).OrderBy(r => r.MinAmount);
    }

    /// <summary>
    /// Gets the rate for a specific amount on a specific date.
    /// Example: Get SSS bracket for salary ₱25,000 on December 15, 2025.
    /// </summary>
    /// <param name="amount">The salary/income amount.</param>
    /// <param name="effectiveDate">The date to check.</param>
    /// <returns>The applicable rate bracket, or null if none found.</returns>
    public PayComponentRate? GetApplicableRate(decimal amount, DateTime effectiveDate)
    {
        return Rates
            .Where(r => r.IsEffectiveOn(effectiveDate) && r.IsInBracket(amount))
            .OrderBy(r => r.MinAmount) // In case of overlaps, take first match
            .FirstOrDefault();
    }

    /// <summary>
    /// Gets all rates for a specific year (for reporting/historical data).
    /// </summary>
    /// <param name="year">The year to filter by.</param>
    /// <returns>All rates where EffectiveStartDate is in the specified year.</returns>
    public IEnumerable<PayComponentRate> GetRatesForYear(int year)
    {
        return Rates.Where(r => r.Year == year).OrderBy(r => r.MinAmount);
    }

    /// <summary>
    /// Adds a new rate and automatically terminates overlapping rates.
    /// Use this when implementing new government contribution tables.
    /// Example: SSS announces new rates effective January 1, 2026 - 
    /// this will terminate existing 2025 rates on December 31, 2025.
    /// </summary>
    /// <param name="newRate">The new rate to add.</param>
    public PayComponent AddRateAndSupersedePrevious(PayComponentRate newRate)
    {
        // Find any overlapping rates
        var overlappingRates = Rates
            .Where(r => r.IsActive && 
                       r.OverlapsWith(newRate.EffectiveStartDate, newRate.EffectiveEndDate))
            .ToList();

        // Terminate overlapping rates
        foreach (var existingRate in overlappingRates)
        {
            // Set end date to one day before new rate starts
            existingRate.Terminate(newRate.EffectiveStartDate.AddDays(-1));
        }

        // Add new rate
        Rates.Add(newRate);
        
        return this;
    }

    /// <summary>
    /// Validates that no rates overlap for the same time period.
    /// Used for data integrity checks.
    /// </summary>
    /// <returns>True if all rates have non-overlapping date ranges.</returns>
    public bool HasNonOverlappingRates()
    {
        var activeRates = Rates.Where(r => r.IsActive).ToList();
        
        for (int i = 0; i < activeRates.Count; i++)
        {
            for (int j = i + 1; j < activeRates.Count; j++)
            {
                if (activeRates[i].OverlapsWith(
                    activeRates[j].EffectiveStartDate, 
                    activeRates[j].EffectiveEndDate))
                {
                    // Check if they're in different brackets (that's OK)
                    if (activeRates[i].IsInBracket(activeRates[j].MinAmount) ||
                        activeRates[i].IsInBracket(activeRates[j].MaxAmount))
                    {
                        return false; // Overlapping date AND bracket = bad
                    }
                }
            }
        }
        
        return true;
    }
}

