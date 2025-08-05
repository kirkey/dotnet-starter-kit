using FluentValidation;

namespace Accounting.Application.Currencies.Update;

public class UpdateCurrencyRequestValidator : AbstractValidator<UpdateCurrencyRequest>
{
    public UpdateCurrencyRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.CurrencyCode)
            .Length(3)
            .Matches("^[A-Z]{3}$")
            .WithMessage("Currency code must be a 3-letter ISO code (e.g., USD, EUR)")
            .When(x => !string.IsNullOrEmpty(x.CurrencyCode));

        RuleFor(x => x.Name)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Symbol)
            .MaximumLength(10)
            .When(x => !string.IsNullOrEmpty(x.Symbol));

        RuleFor(x => x.DecimalPlaces)
            .InclusiveBetween(0, 4)
            .When(x => x.DecimalPlaces.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
