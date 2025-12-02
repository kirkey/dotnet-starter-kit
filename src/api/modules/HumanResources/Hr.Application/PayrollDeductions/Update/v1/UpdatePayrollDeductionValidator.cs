namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Update.v1;

/// <summary>
/// Validator for UpdatePayrollDeductionCommand.
/// </summary>
public class UpdatePayrollDeductionValidator : AbstractValidator<UpdatePayrollDeductionCommand>
{
    public UpdatePayrollDeductionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Payroll deduction ID is required.");

        RuleFor(x => x.DeductionAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Deduction amount cannot be negative.")
            .When(x => x.DeductionAmount.HasValue);

        RuleFor(x => x.DeductionPercentage)
            .GreaterThanOrEqualTo(0).WithMessage("Deduction percentage cannot be negative.")
            .LessThanOrEqualTo(100).WithMessage("Deduction percentage cannot exceed 100%.")
            .When(x => x.DeductionPercentage.HasValue);

        RuleFor(x => x.EndDate)
            .GreaterThan(DateTime.Today).WithMessage("End date must be in the future.")
            .When(x => x.EndDate.HasValue);

        RuleFor(x => x.MaxDeductionLimit)
            .GreaterThanOrEqualTo(0).WithMessage("Max deduction limit cannot be negative.")
            .When(x => x.MaxDeductionLimit.HasValue);

        RuleFor(x => x.Remarks)
            .MaximumLength(512).WithMessage("Remarks must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks));
    }
}

