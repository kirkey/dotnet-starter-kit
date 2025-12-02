namespace FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Create.v1;

public class CreateTaxBracketValidator : AbstractValidator<CreateTaxBracketCommand>
{
    public CreateTaxBracketValidator()
    {
        RuleFor(cmd => cmd.TaxType)
            .NotEmpty().WithMessage("Tax type is required.")
            .MaximumLength(64).WithMessage("Tax type must not exceed 50 characters.");

        RuleFor(cmd => cmd.Year)
            .GreaterThanOrEqualTo(2000).WithMessage("Year must be 2000 or later.")
            .LessThanOrEqualTo(2100).WithMessage("Year must be 2100 or earlier.");

        RuleFor(cmd => cmd.MinIncome)
            .GreaterThanOrEqualTo(0m).WithMessage("Minimum income cannot be negative.");

        RuleFor(cmd => cmd.MaxIncome)
            .GreaterThan(cmd => cmd.MinIncome).WithMessage("Maximum income must be greater than minimum income.");

        RuleFor(cmd => cmd.Rate)
            .InclusiveBetween(0m, 1m).WithMessage("Tax rate must be between 0 and 1 (0-100%).");

        RuleFor(cmd => cmd.FilingStatus)
            .MaximumLength(64).WithMessage("Filing status must not exceed 50 characters.");

        RuleFor(cmd => cmd.Description)
            .MaximumLength(512).WithMessage("Description must not exceed 500 characters.");
    }
}
