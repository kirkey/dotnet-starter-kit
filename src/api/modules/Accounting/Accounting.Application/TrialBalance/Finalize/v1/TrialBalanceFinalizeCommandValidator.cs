namespace Accounting.Application.TrialBalance.Finalize.v1;

/// <summary>
/// Validator for TrialBalanceFinalizeCommand.
/// </summary>
public sealed class TrialBalanceFinalizeCommandValidator : AbstractValidator<TrialBalanceFinalizeCommand>
{
    public TrialBalanceFinalizeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Trial balance ID is required.");

        RuleFor(x => x.FinalizedBy)
            .NotEmpty()
            .WithMessage("Finalizer information is required.")
            .MaximumLength(200)
            .WithMessage("Finalizer information must not exceed 200 characters.");
    }
}

