namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Actions.Renew.v1;

public class RenewWholesaleContractCommandValidator : AbstractValidator<RenewWholesaleContractCommand>
{
    public RenewWholesaleContractCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("WholesaleContract ID is required");
        RuleFor(x => x.NewEndDate)
            .NotEmpty().WithMessage("New end date is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("New end date must be in the future");
    }
}

