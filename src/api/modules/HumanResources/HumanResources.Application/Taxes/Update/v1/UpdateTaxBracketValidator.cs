namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Update.v1;

/// <summary>
/// Validator for UpdateTaxBracketCommand.
/// </summary>
public class UpdateTaxBracketValidator : AbstractValidator<UpdateTaxBracketCommand>
{
    public UpdateTaxBracketValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Tax bracket ID is required.");

        RuleFor(x => x.MinIncome)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum income cannot be negative.")
            .When(x => x.MinIncome.HasValue);

        RuleFor(x => x.MaxIncome)
            .GreaterThan(x => x.MinIncome ?? 0).WithMessage("Maximum income must be greater than minimum income.")
            .When(x => x.MaxIncome.HasValue);

        RuleFor(x => x.Rate)
            .GreaterThanOrEqualTo(0).WithMessage("Tax rate cannot be negative.")
            .LessThanOrEqualTo(1).WithMessage("Tax rate cannot exceed 100%.")
            .When(x => x.Rate.HasValue);

        RuleFor(x => x.FilingStatus)
            .MaximumLength(50).WithMessage("Filing status must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.FilingStatus));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

