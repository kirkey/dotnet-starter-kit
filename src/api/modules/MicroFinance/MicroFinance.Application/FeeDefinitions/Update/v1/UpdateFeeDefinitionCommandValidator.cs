using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Update.v1;

public sealed class UpdateFeeDefinitionCommandValidator : AbstractValidator<UpdateFeeDefinitionCommand>
{
    public UpdateFeeDefinitionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Fee definition ID is required.");

        RuleFor(x => x.Name)
            .MinimumLength(FeeDefinition.NameMinLength)
            .MaximumLength(FeeDefinition.NameMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Name))
            .WithMessage($"Name must be between {FeeDefinition.NameMinLength} and {FeeDefinition.NameMaxLength} characters.");

        RuleFor(x => x.Description)
            .MaximumLength(FeeDefinition.DescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description must not exceed {FeeDefinition.DescriptionMaxLength} characters.");

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Amount.HasValue)
            .WithMessage("Amount must be 0 or greater.");

        RuleFor(x => x.MinAmount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinAmount.HasValue)
            .WithMessage("Minimum amount must be 0 or greater.");

        RuleFor(x => x.MaxAmount)
            .GreaterThan(x => x.MinAmount ?? 0)
            .When(x => x.MaxAmount.HasValue)
            .WithMessage("Maximum amount must be greater than minimum amount.");

        RuleFor(x => x.TaxRate)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100)
            .When(x => x.TaxRate.HasValue)
            .WithMessage("Tax rate must be between 0 and 100.");
    }
}
