namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Create.v1;

public class CreateWholesaleContractCommandValidator : AbstractValidator<CreateWholesaleContractCommand>
{
    public CreateWholesaleContractCommandValidator([FromKeyedServices("store:wholesale-contracts")] IReadRepository<WholesaleContract> readRepository)
    {
        RuleFor(x => x.ContractNumber)
            .NotEmpty().WithMessage("Contract number is required")
            .MaximumLength(100).WithMessage("Contract number must not exceed 100 characters");

        // async uniqueness check
        RuleFor(x => x.ContractNumber).MustAsync(async (contractNumber, ct) =>
        {
            if (string.IsNullOrWhiteSpace(contractNumber)) return true;
            var existing = await readRepository.FirstOrDefaultAsync(new Specs.WholesaleContractByContractNumberSpec(contractNumber), ct).ConfigureAwait(false);
            return existing is null;
        }).WithMessage("A wholesale contract with the same ContractNumber already exists.");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("StartDate is required");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("EndDate is required")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("EndDate must be equal to or after StartDate");

        RuleFor(x => x.MinimumOrderValue)
            .GreaterThanOrEqualTo(0m).WithMessage("MinimumOrderValue must be zero or greater");

        RuleFor(x => x.VolumeDiscountPercentage)
            .InclusiveBetween(0m, 100m).WithMessage("VolumeDiscountPercentage must be between 0 and 100");

        RuleFor(x => x.PaymentTermsDays)
            .GreaterThanOrEqualTo(0).WithMessage("PaymentTermsDays must be zero or greater")
            .LessThanOrEqualTo(365).WithMessage("PaymentTermsDays must not exceed 365 days");

        RuleFor(x => x.CreditLimit)
            .GreaterThanOrEqualTo(0m).WithMessage("CreditLimit must be zero or greater");

        RuleFor(x => x.DeliveryTerms)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.DeliveryTerms)).WithMessage("DeliveryTerms must not exceed 500 characters");

        RuleFor(x => x.ContractTerms)
            .MaximumLength(5000).When(x => !string.IsNullOrEmpty(x.ContractTerms)).WithMessage("ContractTerms must not exceed 5000 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(2048).When(x => !string.IsNullOrEmpty(x.Notes)).WithMessage("Notes must not exceed 2048 characters");
    }
}

