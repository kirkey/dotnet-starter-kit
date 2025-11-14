namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;

/// <summary>
/// Validator for creating a tax bracket.
/// </summary>
public class CreateTaxValidator : AbstractValidator<CreateTaxCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTaxValidator"/> class.
    /// </summary>
    public CreateTaxValidator()
    {
        RuleFor(x => x.TaxType)
            .NotEmpty()
            .WithMessage("Tax type is required")
            .MaximumLength(50)
            .WithMessage("Tax type cannot exceed 50 characters");

        RuleFor(x => x.Year)
            .GreaterThan(1999)
            .WithMessage("Year must be 2000 or later")
            .LessThan(2100)
            .WithMessage("Year must be before 2100");

        RuleFor(x => x.MinIncome)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum income cannot be negative");

        RuleFor(x => x.MaxIncome)
            .GreaterThan(x => x.MinIncome)
            .WithMessage("Maximum income must be greater than minimum income");

        RuleFor(x => x.Rate)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Rate cannot be negative")
            .LessThanOrEqualTo(1)
            .WithMessage("Rate cannot exceed 100%");

        RuleFor(x => x.FilingStatus)
            .MaximumLength(50)
            .WithMessage("Filing status cannot exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.FilingStatus));

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

