namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Update.v1;

/// <summary>
/// Validator for UpdateDeductionCommand.
/// </summary>
public sealed class UpdateDeductionValidator : AbstractValidator<UpdateDeductionCommand>
{
    public UpdateDeductionValidator()
    {
        RuleFor(x => x.DeductionName)
            .MaximumLength(128).WithMessage("Deduction name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.DeductionName));

        RuleFor(x => x.DeductionType)
            .Must(BeValidDeductionType).WithMessage("Deduction type must be: Loan, CashAdvance, Uniform, Equipment, Damages, or Others.")
            .When(x => !string.IsNullOrWhiteSpace(x.DeductionType));

        RuleFor(x => x.RecoveryMethod)
            .Must(BeValidRecoveryMethod).WithMessage("Recovery method must be: Manual, FixedAmount, Percentage, or Installment.")
            .When(x => !string.IsNullOrWhiteSpace(x.RecoveryMethod));

        RuleFor(x => x.RecoveryFixedAmount)
            .GreaterThan(0).WithMessage("Recovery fixed amount must be greater than 0.")
            .When(x => x.RecoveryFixedAmount.HasValue);

        RuleFor(x => x.RecoveryPercentage)
            .InclusiveBetween(0.01m, 100m).WithMessage("Recovery percentage must be between 0.01 and 100.")
            .When(x => x.RecoveryPercentage.HasValue);

        RuleFor(x => x.InstallmentCount)
            .GreaterThan(0).WithMessage("Installment count must be greater than 0.")
            .When(x => x.InstallmentCount.HasValue);

        RuleFor(x => x.MaxRecoveryPercentage)
            .InclusiveBetween(1m, 100m).WithMessage("Max recovery percentage must be between 1 and 100.")
            .When(x => x.MaxRecoveryPercentage.HasValue);

        RuleFor(x => x.GlAccountCode)
            .MaximumLength(32).WithMessage("GL account code must not exceed 20 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.GlAccountCode));

        RuleFor(x => x.Description)
            .MaximumLength(512).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }

    private static bool BeValidDeductionType(string? type)
    {
        if (string.IsNullOrWhiteSpace(type)) return true;
        var validTypes = new[] { "Loan", "CashAdvance", "Uniform", "Equipment", "Damages", "Others" };
        return validTypes.Contains(type, StringComparer.OrdinalIgnoreCase);
    }

    private static bool BeValidRecoveryMethod(string? method)
    {
        if (string.IsNullOrWhiteSpace(method)) return true;
        var validMethods = new[] { "Manual", "FixedAmount", "Percentage", "Installment" };
        return validMethods.Contains(method, StringComparer.OrdinalIgnoreCase);
    }
}

