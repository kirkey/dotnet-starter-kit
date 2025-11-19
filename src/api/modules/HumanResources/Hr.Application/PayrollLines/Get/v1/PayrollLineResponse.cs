namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Get.v1;

/// <summary>
/// Response object for PayrollLine entity details.
/// </summary>
public sealed record PayrollLineResponse
{
    /// <summary>
    /// Gets the unique identifier of the payroll line.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets the payroll period identifier.
    /// </summary>
    public DefaultIdType PayrollId { get; init; }

    /// <summary>
    /// Gets the employee identifier.
    /// </summary>
    public DefaultIdType EmployeeId { get; init; }

    /// <summary>
    /// Gets the regular hours worked.
    /// </summary>
    public decimal RegularHours { get; init; }

    /// <summary>
    /// Gets the overtime hours worked.
    /// </summary>
    public decimal OvertimeHours { get; init; }

    /// <summary>
    /// Gets the regular pay amount.
    /// </summary>
    public decimal RegularPay { get; init; }

    /// <summary>
    /// Gets the overtime pay amount.
    /// </summary>
    public decimal OvertimePay { get; init; }

    /// <summary>
    /// Gets the bonus pay amount.
    /// </summary>
    public decimal BonusPay { get; init; }

    /// <summary>
    /// Gets other earnings.
    /// </summary>
    public decimal OtherEarnings { get; init; }

    /// <summary>
    /// Gets the gross pay (sum of all earnings).
    /// </summary>
    public decimal GrossPay { get; init; }

    /// <summary>
    /// Gets the income tax withheld.
    /// </summary>
    public decimal IncomeTax { get; init; }

    /// <summary>
    /// Gets the Social Security tax withheld.
    /// </summary>
    public decimal SocialSecurityTax { get; init; }

    /// <summary>
    /// Gets the Medicare tax withheld.
    /// </summary>
    public decimal MedicareTax { get; init; }

    /// <summary>
    /// Gets other taxes withheld.
    /// </summary>
    public decimal OtherTaxes { get; init; }

    /// <summary>
    /// Gets the total taxes withheld.
    /// </summary>
    public decimal TotalTaxes { get; init; }

    /// <summary>
    /// Gets the health insurance deduction.
    /// </summary>
    public decimal HealthInsurance { get; init; }

    /// <summary>
    /// Gets the retirement contribution deduction.
    /// </summary>
    public decimal RetirementContribution { get; init; }

    /// <summary>
    /// Gets other deductions.
    /// </summary>
    public decimal OtherDeductions { get; init; }

    /// <summary>
    /// Gets the total deductions.
    /// </summary>
    public decimal TotalDeductions { get; init; }

    /// <summary>
    /// Gets the net pay (gross - taxes - deductions).
    /// </summary>
    public decimal NetPay { get; init; }

    /// <summary>
    /// Gets the payment method (Direct Deposit, Check, etc).
    /// </summary>
    public string? PaymentMethod { get; init; }

    /// <summary>
    /// Gets the last 4 digits of bank account (if applicable).
    /// </summary>
    public string? BankAccountLast4 { get; init; }

    /// <summary>
    /// Gets the check number (if applicable).
    /// </summary>
    public string? CheckNumber { get; init; }
}

