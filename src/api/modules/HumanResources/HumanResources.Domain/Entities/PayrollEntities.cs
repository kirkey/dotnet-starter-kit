namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents pay calculation for one employee in a payroll period.
/// </summary>
public class PayrollLine : AuditableEntity, IAggregateRoot
{
    private PayrollLine() { }

    private PayrollLine(
        DefaultIdType id,
        DefaultIdType payrollId,
        DefaultIdType employeeId,
        decimal regularHours = 0,
        decimal overtimeHours = 0)
    {
        Id = id;
        PayrollId = payrollId;
        EmployeeId = employeeId;
        RegularHours = regularHours;
        OvertimeHours = overtimeHours;
        GrossPay = 0;
        TotalTaxes = 0;
        TotalDeductions = 0;
        NetPay = 0;
    }

    public DefaultIdType PayrollId { get; private set; }
    public Payroll Payroll { get; private set; } = default!;

    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    public decimal RegularHours { get; private set; }
    public decimal OvertimeHours { get; private set; }

    public decimal RegularPay { get; private set; }
    public decimal OvertimePay { get; private set; }
    public decimal BonusPay { get; private set; }
    public decimal OtherEarnings { get; private set; }

    public decimal GrossPay { get; private set; }
    public decimal IncomeTax { get; private set; }
    public decimal SocialSecurityTax { get; private set; }
    public decimal MedicareTax { get; private set; }
    public decimal OtherTaxes { get; private set; }
    public decimal TotalTaxes { get; private set; }

    public decimal HealthInsurance { get; private set; }
    public decimal RetirementContribution { get; private set; }
    public decimal OtherDeductions { get; private set; }
    public decimal TotalDeductions { get; private set; }

    public decimal NetPay { get; private set; }

    public string? PaymentMethod { get; private set; }
    public string? BankAccountLast4 { get; private set; }
    public string? CheckNumber { get; private set; }

    public static PayrollLine Create(
        DefaultIdType payrollId,
        DefaultIdType employeeId,
        decimal regularHours = 0,
        decimal overtimeHours = 0)
    {
        var line = new PayrollLine(
            DefaultIdType.NewGuid(),
            payrollId,
            employeeId,
            regularHours,
            overtimeHours);

        return line;
    }

    public PayrollLine SetHours(decimal regularHours, decimal overtimeHours)
    {
        if (regularHours < 0 || overtimeHours < 0)
            throw new ArgumentException("Hours cannot be negative.");

        RegularHours = regularHours;
        OvertimeHours = overtimeHours;
        return this;
    }

    public PayrollLine SetEarnings(
        decimal regularPay = 0,
        decimal overtimePay = 0,
        decimal bonusPay = 0,
        decimal otherEarnings = 0)
    {
        RegularPay = regularPay;
        OvertimePay = overtimePay;
        BonusPay = bonusPay;
        OtherEarnings = otherEarnings;
        return this;
    }

    public PayrollLine SetTaxes(
        decimal incomeTax = 0,
        decimal socialSecurityTax = 0,
        decimal medicareTax = 0,
        decimal otherTaxes = 0)
    {
        IncomeTax = incomeTax;
        SocialSecurityTax = socialSecurityTax;
        MedicareTax = medicareTax;
        OtherTaxes = otherTaxes;
        return this;
    }

    public PayrollLine SetDeductions(
        decimal healthInsurance = 0,
        decimal retirementContribution = 0,
        decimal otherDeductions = 0)
    {
        HealthInsurance = healthInsurance;
        RetirementContribution = retirementContribution;
        OtherDeductions = otherDeductions;
        return this;
    }

    public PayrollLine SetPaymentMethod(string method, string? bankAccountLast4 = null, string? checkNumber = null)
    {
        PaymentMethod = method;
        BankAccountLast4 = bankAccountLast4;
        CheckNumber = checkNumber;
        return this;
    }

    public PayrollLine RecalculateTotals()
    {
        GrossPay = RegularPay + OvertimePay + BonusPay + OtherEarnings;
        TotalTaxes = IncomeTax + SocialSecurityTax + MedicareTax + OtherTaxes;
        TotalDeductions = HealthInsurance + RetirementContribution + OtherDeductions;
        NetPay = GrossPay - TotalTaxes - TotalDeductions;

        if (NetPay < 0)
            throw new InvalidOperationException("Net pay cannot be negative.");

        return this;
    }
}

/// <summary>
/// Represents a pay component (earnings type or deduction type).
/// Used for configuration of what goes into payroll calculations.
/// </summary>
public class PayComponent : AuditableEntity, IAggregateRoot
{
    private PayComponent() { }

    private PayComponent(
        DefaultIdType id,
        string componentName,
        string componentType,
        string glAccountCode = "")
    {
        Id = id;
        ComponentName = componentName;
        ComponentType = componentType;
        GLAccountCode = glAccountCode;
        IsActive = true;
        IsCalculated = false;
    }

    public string ComponentName { get; private set; } = default!;
    public string ComponentType { get; private set; } = default!; // Earnings, Tax, Deduction
    public string GLAccountCode { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public bool IsCalculated { get; private set; }
    public string? Description { get; private set; }

    public static PayComponent Create(
        string componentName,
        string componentType,
        string glAccountCode = "")
    {
        if (string.IsNullOrWhiteSpace(componentName))
            throw new ArgumentException("Component name is required.", nameof(componentName));

        var component = new PayComponent(
            DefaultIdType.NewGuid(),
            componentName,
            componentType,
            glAccountCode);

        return component;
    }

    public PayComponent Update(string? componentName = null, string? glAccountCode = null, string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(componentName))
            ComponentName = componentName;

        if (!string.IsNullOrWhiteSpace(glAccountCode))
            GLAccountCode = glAccountCode;

        if (description != null)
            Description = description;

        return this;
    }

    public PayComponent Deactivate()
    {
        IsActive = false;
        return this;
    }

    public PayComponent Activate()
    {
        IsActive = true;
        return this;
    }
}

/// <summary>
/// Represents tax brackets for tax calculation.
/// </summary>
public class TaxBracket : AuditableEntity, IAggregateRoot
{
    private TaxBracket() { }

    private TaxBracket(
        DefaultIdType id,
        string taxType,
        int year,
        decimal minIncome,
        decimal maxIncome,
        decimal rate)
    {
        Id = id;
        TaxType = taxType;
        Year = year;
        MinIncome = minIncome;
        MaxIncome = maxIncome;
        Rate = rate;
    }

    public string TaxType { get; private set; } = default!; // Federal, State, FICA, etc
    public int Year { get; private set; }
    public decimal MinIncome { get; private set; }
    public decimal MaxIncome { get; private set; }
    public decimal Rate { get; private set; }
    public string? FilingStatus { get; private set; } // Single, Married, etc
    public string? Description { get; private set; }

    public static TaxBracket Create(
        string taxType,
        int year,
        decimal minIncome,
        decimal maxIncome,
        decimal rate)
    {
        if (maxIncome <= minIncome)
            throw new ArgumentException("Max income must be greater than min income.", nameof(maxIncome));

        if (rate < 0 || rate > 1)
            throw new ArgumentException("Tax rate must be between 0 and 1.", nameof(rate));

        var bracket = new TaxBracket(
            DefaultIdType.NewGuid(),
            taxType,
            year,
            minIncome,
            maxIncome,
            rate);

        return bracket;
    }

    public TaxBracket Update(string? filingStatus = null, string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(filingStatus))
            FilingStatus = filingStatus;

        if (description != null)
            Description = description;

        return this;
    }
}

/// <summary>
/// Component type constants.
/// </summary>
public static class PayComponentType
{
    public const string Earnings = "Earnings";
    public const string Tax = "Tax";
    public const string Deduction = "Deduction";
}

/// <summary>
/// Tax type constants.
/// </summary>
public static class TaxType
{
    public const string IncomeTax = "IncomeTax";
    public const string SocialSecurity = "SocialSecurity";
    public const string Medicare = "Medicare";
}

