using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Create.v1;

public sealed class CreateFeeDefinitionCommandValidator : AbstractValidator<CreateFeeDefinitionCommand>
{
    public CreateFeeDefinitionCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(FeeDefinition.CodeMaxLength)
            .WithMessage($"Code must not exceed {FeeDefinition.CodeMaxLength} characters.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(FeeDefinition.NameMinLength)
            .MaximumLength(FeeDefinition.NameMaxLength)
            .WithMessage($"Name must be between {FeeDefinition.NameMinLength} and {FeeDefinition.NameMaxLength} characters.");

        RuleFor(x => x.Description)
            .MaximumLength(FeeDefinition.DescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description must not exceed {FeeDefinition.DescriptionMaxLength} characters.");

        RuleFor(x => x.FeeType)
            .NotEmpty()
            .MaximumLength(FeeDefinition.FeeTypeMaxLength)
            .WithMessage($"Fee type must not exceed {FeeDefinition.FeeTypeMaxLength} characters.");

        RuleFor(x => x.CalculationType)
            .NotEmpty()
            .MaximumLength(FeeDefinition.CalculationTypeMaxLength)
            .WithMessage($"Calculation type must not exceed {FeeDefinition.CalculationTypeMaxLength} characters.");

        RuleFor(x => x.AppliesTo)
            .NotEmpty()
            .MaximumLength(FeeDefinition.AppliesToMaxLength)
            .WithMessage($"Applies to must not exceed {FeeDefinition.AppliesToMaxLength} characters.");

        RuleFor(x => x.ChargeFrequency)
            .NotEmpty()
            .MaximumLength(FeeDefinition.ChargeFrequencyMaxLength)
            .WithMessage($"Charge frequency must not exceed {FeeDefinition.ChargeFrequencyMaxLength} characters.");

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0)
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
            .When(x => x.IsTaxable && x.TaxRate.HasValue)
            .WithMessage("Tax rate must be between 0 and 100.");
    }
}
