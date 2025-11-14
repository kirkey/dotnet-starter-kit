namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Update.v1;

/// <summary>
/// Validator for updating a payroll line.
/// </summary>
public class UpdatePayrollLineValidator : AbstractValidator<UpdatePayrollLineCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePayrollLineValidator"/> class.
    /// </summary>
    public UpdatePayrollLineValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Payroll line ID is required");

        RuleFor(x => x.RegularHours)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Regular hours cannot be negative")
            .LessThanOrEqualTo(260)
            .WithMessage("Regular hours cannot exceed 260 per month")
            .When(x => x.RegularHours.HasValue);

        RuleFor(x => x.OvertimeHours)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Overtime hours cannot be negative")
            .LessThanOrEqualTo(100)
            .WithMessage("Overtime hours cannot exceed 100 per month")
            .When(x => x.OvertimeHours.HasValue);

        RuleFor(x => x.RegularPay)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Regular pay cannot be negative")
            .When(x => x.RegularPay.HasValue);

        RuleFor(x => x.OvertimePay)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Overtime pay cannot be negative")
            .When(x => x.OvertimePay.HasValue);

        RuleFor(x => x.BonusPay)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Bonus pay cannot be negative")
            .When(x => x.BonusPay.HasValue);

        RuleFor(x => x.IncomeTax)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Income tax cannot be negative")
            .When(x => x.IncomeTax.HasValue);

        RuleFor(x => x.PaymentMethod)
            .Must(BeValidPaymentMethod)
            .When(x => !string.IsNullOrWhiteSpace(x.PaymentMethod))
            .WithMessage("Payment method must be 'DirectDeposit' or 'Check'");

        RuleFor(x => x.BankAccountLast4)
            .Length(4)
            .WithMessage("Bank account last 4 must be exactly 4 digits")
            .Matches(@"^\d+$")
            .WithMessage("Bank account last 4 must contain only digits")
            .When(x => !string.IsNullOrWhiteSpace(x.BankAccountLast4));

        RuleFor(x => x.CheckNumber)
            .MaximumLength(20)
            .WithMessage("Check number cannot exceed 20 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.CheckNumber));
    }

    private static bool BeValidPaymentMethod(string? method)
    {
        if (string.IsNullOrWhiteSpace(method))
            return true;

        var validMethods = new[] { "DirectDeposit", "Check" };
        return validMethods.Contains(method);
    }
}

