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
}
