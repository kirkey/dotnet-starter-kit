namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Create.v1;

public class CreatePayComponentValidator : AbstractValidator<CreatePayComponentCommand>
{
    public CreatePayComponentValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(64).WithMessage("Code must not exceed 50 characters.");

        RuleFor(x => x.ComponentName)
            .NotEmpty().WithMessage("Component name is required.")
            .MaximumLength(256).WithMessage("Component name must not exceed 200 characters.");

        RuleFor(x => x.ComponentType)
            .NotEmpty().WithMessage("Component type is required.")
            .Must(type => new[] { "Earnings", "Deduction", "Tax", "EmployerContribution" }.Contains(type))
            .WithMessage("Component type must be one of: Earnings, Deduction, Tax, EmployerContribution.");

        RuleFor(x => x.CalculationMethod)
            .NotEmpty().WithMessage("Calculation method is required.")
            .Must(method => new[] { "Manual", "Formula", "Percentage", "Bracket", "Fixed" }.Contains(method))
            .WithMessage("Calculation method must be one of: Manual, Formula, Percentage, Bracket, Fixed.");

        RuleFor(x => x.GlAccountCode)
            .NotEmpty().WithMessage("GL account code is required.")
            .MaximumLength(64).WithMessage("GL account code must not exceed 50 characters.");

        RuleFor(x => x.CalculationFormula)
            .MaximumLength(512).WithMessage("Calculation formula must not exceed 500 characters.");

        RuleFor(x => x.Rate)
            .GreaterThanOrEqualTo(0).WithMessage("Rate must be greater than or equal to 0.")
            .When(x => x.Rate.HasValue);

        RuleFor(x => x.FixedAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Fixed amount must be greater than or equal to 0.")
            .When(x => x.FixedAmount.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(1024).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.LaborLawReference)
            .MaximumLength(256).WithMessage("Labor law reference must not exceed 200 characters.");
    }
}

