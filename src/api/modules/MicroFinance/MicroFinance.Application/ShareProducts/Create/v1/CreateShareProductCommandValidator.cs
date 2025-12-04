using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Create.v1;

public sealed class CreateShareProductCommandValidator : AbstractValidator<CreateShareProductCommand>
{
    public CreateShareProductCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(ShareProduct.CodeMaxLength)
            .WithMessage($"Code must not exceed {ShareProduct.CodeMaxLength} characters.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(ShareProduct.NameMinLength)
            .MaximumLength(ShareProduct.NameMaxLength)
            .WithMessage($"Name must be between {ShareProduct.NameMinLength} and {ShareProduct.NameMaxLength} characters.");

        RuleFor(x => x.Description)
            .MaximumLength(ShareProduct.DescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description must not exceed {ShareProduct.DescriptionMaxLength} characters.");

        RuleFor(x => x.NominalValue)
            .GreaterThan(0)
            .WithMessage("Nominal value must be greater than 0.");

        RuleFor(x => x.CurrentPrice)
            .GreaterThan(0)
            .WithMessage("Current price must be greater than 0.");

        RuleFor(x => x.MinSharesForMembership)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum shares for membership must be 0 or greater.");

        RuleFor(x => x.MaxSharesPerMember)
            .GreaterThan(0)
            .When(x => x.MaxSharesPerMember.HasValue)
            .WithMessage("Maximum shares per member must be greater than 0.");

        RuleFor(x => x.MinHoldingPeriodMonths)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinHoldingPeriodMonths.HasValue)
            .WithMessage("Minimum holding period must be 0 or greater.");
    }
}
