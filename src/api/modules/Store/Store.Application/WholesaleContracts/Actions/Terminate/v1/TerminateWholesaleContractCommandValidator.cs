namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Actions.Terminate.v1;

public class TerminateWholesaleContractCommandValidator : AbstractValidator<TerminateWholesaleContractCommand>
{
    public TerminateWholesaleContractCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("WholesaleContract ID is required");
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Termination reason is required")
            .MaximumLength(1000).WithMessage("Termination reason must not exceed 1000 characters");
    }
}

