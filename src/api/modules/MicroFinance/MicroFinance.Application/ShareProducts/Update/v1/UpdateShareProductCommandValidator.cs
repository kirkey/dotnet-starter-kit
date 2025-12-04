using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Update.v1;

public sealed class UpdateShareProductCommandValidator : AbstractValidator<UpdateShareProductCommand>
{
    public UpdateShareProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Share product ID is required.");

        RuleFor(x => x.Name)
            .MinimumLength(ShareProduct.NameMinLength)
            .MaximumLength(ShareProduct.NameMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Name))
            .WithMessage($"Name must be between {ShareProduct.NameMinLength} and {ShareProduct.NameMaxLength} characters.");

        RuleFor(x => x.Description)
            .MaximumLength(ShareProduct.DescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description must not exceed {ShareProduct.DescriptionMaxLength} characters.");

        RuleFor(x => x.CurrentPrice)
            .GreaterThan(0)
            .When(x => x.CurrentPrice.HasValue)
            .WithMessage("Current price must be greater than 0.");

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
