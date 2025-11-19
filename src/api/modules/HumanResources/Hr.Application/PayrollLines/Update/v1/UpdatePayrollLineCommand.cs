namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Update.v1;

/// <summary>
/// Command to update payroll line calculations.
/// </summary>
public sealed record UpdatePayrollLineCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] decimal? RegularHours = null,
    [property: DefaultValue(null)] decimal? OvertimeHours = null,
    [property: DefaultValue(null)] decimal? RegularPay = null,
    [property: DefaultValue(null)] decimal? OvertimePay = null,
    [property: DefaultValue(null)] decimal? BonusPay = null,
    [property: DefaultValue(null)] decimal? OtherEarnings = null,
    [property: DefaultValue(null)] decimal? IncomeTax = null,
    [property: DefaultValue(null)] decimal? SocialSecurityTax = null,
    [property: DefaultValue(null)] decimal? MedicareTax = null,
    [property: DefaultValue(null)] decimal? OtherTaxes = null,
    [property: DefaultValue(null)] decimal? HealthInsurance = null,
    [property: DefaultValue(null)] decimal? RetirementContribution = null,
    [property: DefaultValue(null)] decimal? OtherDeductions = null,
    [property: DefaultValue(null)] string? PaymentMethod = null,
    [property: DefaultValue(null)] string? BankAccountLast4 = null,
    [property: DefaultValue(null)] string? CheckNumber = null) : IRequest<UpdatePayrollLineResponse>;

