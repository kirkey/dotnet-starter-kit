namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Update.v1;

/// <summary>
/// Validator for updating a deduction.
/// </summary>
public class UpdateDeductionValidator : AbstractValidator<UpdateDeductionCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateDeductionValidator"/> class.
    /// </summary>
    public UpdateDeductionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Deduction ID is required");

        RuleFor(x => x.ComponentName)
            .MaximumLength(100)
            .WithMessage("Component name cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ComponentName));

        RuleFor(x => x.GLAccountCode)
            .MaximumLength(20)
            .WithMessage("GL account code cannot exceed 20 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.GLAccountCode));

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

