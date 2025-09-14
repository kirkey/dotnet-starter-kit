namespace FSH.Starter.WebApi.Store.Application.WholesalePricings.UpdatePricing.v1;

public class UpdateWholesalePricingCommandValidator : AbstractValidator<UpdateWholesalePricingCommand>
{
    public UpdateWholesalePricingCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("WholesalePricing ID is required");

        RuleFor(x => x.TierPrice)
            .GreaterThanOrEqualTo(0m).WithMessage("TierPrice must be zero or greater");

        RuleFor(x => x.DiscountPercentage)
            .InclusiveBetween(0m, 100m).WithMessage("DiscountPercentage must be between 0 and 100");
    }
}

