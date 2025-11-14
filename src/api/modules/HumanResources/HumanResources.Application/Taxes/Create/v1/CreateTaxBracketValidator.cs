namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;

/// <summary>
/// Validator for CreateTaxBracketCommand with Philippines TRAIN Law compliance.
/// </summary>
public class CreateTaxBracketValidator : AbstractValidator<CreateTaxBracketCommand>
{
    public CreateTaxBracketValidator()
    {
        RuleFor(x => x.TaxType)
            .NotEmpty().WithMessage("Tax type is required.")
            .MaximumLength(50).WithMessage("Tax type must not exceed 50 characters.");

        RuleFor(x => x.Year)
            .GreaterThanOrEqualTo(2020).WithMessage("Year must be 2020 or later.")
            .LessThanOrEqualTo(DateTime.Now.Year + 5).WithMessage("Year cannot be more than 5 years in the future.");

        RuleFor(x => x.MinIncome)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum income cannot be negative.");

        RuleFor(x => x.MaxIncome)
            .GreaterThan(x => x.MinIncome).WithMessage("Maximum income must be greater than minimum income.");

        RuleFor(x => x.Rate)
            .GreaterThanOrEqualTo(0).WithMessage("Tax rate cannot be negative.")
            .LessThanOrEqualTo(1).WithMessage("Tax rate cannot exceed 100%.");

        RuleFor(x => x.FilingStatus)
            .MaximumLength(50).WithMessage("Filing status must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.FilingStatus));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

