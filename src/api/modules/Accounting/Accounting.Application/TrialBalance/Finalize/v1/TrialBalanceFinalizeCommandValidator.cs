namespace Accounting.Application.TrialBalance.Finalize.v1;

/// <summary>
/// Validator for FinalizeTrialBalanceCommand.
/// The finalizer is automatically determined from the current user session.
/// </summary>
public sealed class FinalizeTrialBalanceCommandValidator : AbstractValidator<FinalizeTrialBalanceCommand>
{
    public FinalizeTrialBalanceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Trial balance ID is required.");
    }
}

