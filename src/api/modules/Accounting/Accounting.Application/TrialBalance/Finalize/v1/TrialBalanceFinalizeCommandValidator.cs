namespace Accounting.Application.TrialBalance.Finalize.v1;

/// <summary>
/// Validator for TrialBalanceFinalizeCommand.
/// The finalizer is automatically determined from the current user session.
/// </summary>
public sealed class TrialBalanceFinalizeCommandValidator : AbstractValidator<TrialBalanceFinalizeCommand>
{
    public TrialBalanceFinalizeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Trial balance ID is required.");
    }
}

