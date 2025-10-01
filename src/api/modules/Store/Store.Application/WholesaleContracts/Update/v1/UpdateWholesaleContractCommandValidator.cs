namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Update.v1;

public class UpdateWholesaleContractCommandValidator : AbstractValidator<UpdateWholesaleContractCommand>
{
    public UpdateWholesaleContractCommandValidator([FromKeyedServices("store:wholesale-contracts")] IReadRepository<WholesaleContract> readRepository)
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("WholesaleContract ID is required");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.EndDate.HasValue && x.StartDate.HasValue)
            .WithMessage("EndDate must be equal to or after StartDate");

        RuleFor(x => x.MinimumOrderValue)
            .GreaterThanOrEqualTo(0m).When(x => x.MinimumOrderValue.HasValue)
            .WithMessage("MinimumOrderValue must be zero or greater");

        RuleFor(x => x.VolumeDiscountPercentage)
            .InclusiveBetween(0m, 100m).When(x => x.VolumeDiscountPercentage.HasValue)
            .WithMessage("VolumeDiscountPercentage must be between 0 and 100");

        RuleFor(x => x.PaymentTermsDays)
            .GreaterThanOrEqualTo(0).When(x => x.PaymentTermsDays.HasValue)
            .LessThanOrEqualTo(365).When(x => x.PaymentTermsDays.HasValue)
            .WithMessage("PaymentTermsDays must be between 0 and 365 days");

        RuleFor(x => x.CreditLimit)
            .GreaterThanOrEqualTo(0m).When(x => x.CreditLimit.HasValue)
            .WithMessage("CreditLimit must be zero or greater");

        RuleFor(x => x.DeliveryTerms)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.DeliveryTerms)).WithMessage("DeliveryTerms must not exceed 500 characters");

        RuleFor(x => x.ContractTerms)
            .MaximumLength(5000).When(x => !string.IsNullOrEmpty(x.ContractTerms)).WithMessage("ContractTerms must not exceed 5000 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(2048).When(x => !string.IsNullOrEmpty(x.Notes)).WithMessage("Notes must not exceed 2048 characters");

        RuleFor(x => x.Status)
            .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Status)).WithMessage("Status must not exceed 50 characters");
    }
}

