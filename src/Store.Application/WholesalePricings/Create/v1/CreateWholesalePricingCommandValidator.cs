namespace FSH.Starter.WebApi.Store.Application.WholesalePricings.Create.v1;

public class CreateWholesalePricingCommandValidator : AbstractValidator<CreateWholesalePricingCommand>
{
    public CreateWholesalePricingCommandValidator(
        [FromKeyedServices("store:wholesale-contracts")] IReadRepository<WholesaleContract> contractRepository,
        [FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> groceryRepository,
        [FromKeyedServices("store:wholesale-pricings")] IReadRepository<WholesalePricing> pricingRepository)
    {
        RuleFor(x => x.WholesaleContractId)
            .NotEmpty().WithMessage("WholesaleContractId is required")
            .MustAsync(async (id, ct) => await contractRepository.GetByIdAsync(id, ct) != null).WithMessage("WholesaleContract not found");

        RuleFor(x => x.GroceryItemId)
            .NotEmpty().WithMessage("GroceryItemId is required")
            .MustAsync(async (id, ct) => await groceryRepository.GetByIdAsync(id, ct) != null).WithMessage("GroceryItem not found");

        RuleFor(x => x.MinimumQuantity)
            .GreaterThan(0).WithMessage("MinimumQuantity must be greater than zero");

        RuleFor(x => x.MaximumQuantity)
            .GreaterThanOrEqualTo(x => x.MinimumQuantity)
            .When(x => x.MaximumQuantity.HasValue)
            .WithMessage("MaximumQuantity must be greater than or equal to MinimumQuantity");

        RuleFor(x => x.TierPrice)
            .GreaterThanOrEqualTo(0m).WithMessage("TierPrice must be zero or greater");

        RuleFor(x => x.DiscountPercentage)
            .InclusiveBetween(0m, 100m).WithMessage("DiscountPercentage must be between 0 and 100");

        RuleFor(x => x.EffectiveDate)
            .NotEmpty().WithMessage("EffectiveDate is required");

        RuleFor(x => x.ExpiryDate)
            .GreaterThanOrEqualTo(x => x.EffectiveDate)
            .When(x => x.ExpiryDate.HasValue)
            .WithMessage("ExpiryDate cannot be earlier than EffectiveDate");

        RuleFor(x => x.Notes)
            .MaximumLength(2048).When(x => !string.IsNullOrEmpty(x.Notes)).WithMessage("Notes must not exceed 2048 characters");

        // Async uniqueness/overlap check
        RuleFor(x => x).MustAsync(async (cmd, ct) =>
        {
            var existing = await pricingRepository.FirstOrDefaultAsync(new Specs.OverlappingWholesalePricingSpec(
                cmd.WholesaleContractId,
                cmd.GroceryItemId,
                cmd.MinimumQuantity,
                cmd.MaximumQuantity,
                cmd.EffectiveDate,
                cmd.ExpiryDate), ct).ConfigureAwait(false);
            return existing is null;
        }).WithMessage("An overlapping wholesale pricing already exists for the same contract and item.");
    }
}

