namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Create.v1;

/// <summary>
/// Validator for CreatePayComponentCommand with Philippines payroll compliance.
/// </summary>
public class CreatePayComponentValidator : AbstractValidator<CreatePayComponentCommand>
{
    public CreatePayComponentValidator()
    {
        RuleFor(x => x.ComponentName)
            .NotEmpty().WithMessage("Component name is required.")
            .MaximumLength(100).WithMessage("Component name must not exceed 100 characters.");

        RuleFor(x => x.ComponentType)
            .NotEmpty().WithMessage("Component type is required.")
            .Must(t => new[] { "Earnings", "Tax", "Deduction", "Benefit" }.Contains(t))
            .WithMessage("Component type must be: Earnings, Tax, Deduction, or Benefit.");

        RuleFor(x => x.GlAccountCode)
            .MaximumLength(50).WithMessage("GL account code must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.GlAccountCode));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

