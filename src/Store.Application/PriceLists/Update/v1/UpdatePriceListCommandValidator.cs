namespace FSH.Starter.WebApi.Store.Application.PriceLists.Update.v1;

public class UpdatePriceListCommandValidator : AbstractValidator<UpdatePriceListCommand>
{
    public UpdatePriceListCommandValidator([FromKeyedServices("store:price-lists")] IReadRepository<PriceList> readRepository)
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("PriceList ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(256).WithMessage("Name must not exceed 256 characters");

        RuleFor(x => x.PriceListName)
            .NotEmpty().WithMessage("PriceList name is required")
            .MaximumLength(64).WithMessage("PriceList name must not exceed 64 characters");

        // Async uniqueness check for update (exclude current)
        RuleFor(x => x.PriceListName).MustAsync(async (cmd, priceListName, ct) =>
        {
            if (string.IsNullOrWhiteSpace(priceListName)) return true;
            var existing = await readRepository.FirstOrDefaultAsync(new Specs.PriceListByNameSpec(priceListName), ct).ConfigureAwait(false);
            return existing is null || existing.Id == cmd.Id;
        }).WithMessage("Another price list with the same PriceListName already exists.");

        RuleFor(x => x.PriceListType)
            .NotEmpty().WithMessage("Price list type is required");

        RuleFor(x => x.EffectiveDate).NotEmpty().WithMessage("Effective date is required");

        RuleFor(x => x.ExpiryDate)
            .GreaterThanOrEqualTo(x => x.EffectiveDate)
            .When(x => x.ExpiryDate.HasValue)
            .WithMessage("Expiry date must be equal to or after Effective date");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .MaximumLength(8).WithMessage("Currency code must not exceed 8 characters");

        RuleFor(x => x.MinimumOrderValue)
            .GreaterThanOrEqualTo(0).When(x => x.MinimumOrderValue.HasValue)
            .WithMessage("Minimum order value must be non-negative");
    }
}

