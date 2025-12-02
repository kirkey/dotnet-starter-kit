namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Create.v1;

/// <summary>
/// Validator for CreateDeductionCommand.
/// </summary>
public sealed class CreateDeductionValidator : AbstractValidator<CreateDeductionCommand>
{
    public CreateDeductionValidator()
    {
        RuleFor(x => x.DeductionName)
            .NotEmpty().WithMessage("Deduction name is required.")
            .MaximumLength(128).WithMessage("Deduction name must not exceed 100 characters.");

        RuleFor(x => x.DeductionType)
            .NotEmpty().WithMessage("Deduction type is required.")
            .Must(BeValidDeductionType).WithMessage("Deduction type must be: Loan, CashAdvance, Uniform, Equipment, Damages, or Others.");

        RuleFor(x => x.RecoveryMethod)
            .NotEmpty().WithMessage("Recovery method is required.")
            .Must(BeValidRecoveryMethod).WithMessage("Recovery method must be: Manual, FixedAmount, Percentage, or Installment.");

        RuleFor(x => x.RecoveryFixedAmount)
            .GreaterThan(0).WithMessage("Recovery fixed amount must be greater than 0.")
            .When(x => x.RecoveryMethod == "FixedAmount");

        RuleFor(x => x.RecoveryPercentage)
            .InclusiveBetween(0.01m, 100m).WithMessage("Recovery percentage must be between 0.01 and 100.")
            .When(x => x.RecoveryMethod == "Percentage");

        RuleFor(x => x.InstallmentCount)
            .GreaterThan(0).WithMessage("Installment count must be greater than 0.")
            .When(x => x.RecoveryMethod == "Installment");

        RuleFor(x => x.MaxRecoveryPercentage)
            .InclusiveBetween(1m, 100m).WithMessage("Max recovery percentage must be between 1 and 100.");

        RuleFor(x => x.GlAccountCode)
            .MaximumLength(32).WithMessage("GL account code must not exceed 20 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.GlAccountCode));

        RuleFor(x => x.Description)
            .MaximumLength(512).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }

    private static bool BeValidDeductionType(string type)
    {
        var validTypes = new[] { "Loan", "CashAdvance", "Uniform", "Equipment", "Damages", "Others" };
        return validTypes.Contains(type, StringComparer.OrdinalIgnoreCase);
    }

    private static bool BeValidRecoveryMethod(string method)
    {
        var validMethods = new[] { "Manual", "FixedAmount", "Percentage", "Installment" };
        return validMethods.Contains(method, StringComparer.OrdinalIgnoreCase);
    }
}

