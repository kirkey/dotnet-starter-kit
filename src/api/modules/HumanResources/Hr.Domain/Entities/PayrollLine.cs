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


