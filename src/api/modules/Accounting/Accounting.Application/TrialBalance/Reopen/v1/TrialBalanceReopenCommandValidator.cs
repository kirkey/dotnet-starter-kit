namespace Accounting.Application.TrialBalance.Reopen.v1;

/// <summary>
/// Validator for TrialBalanceReopenCommand.
/// </summary>
public sealed class TrialBalanceReopenCommandValidator : AbstractValidator<TrialBalanceReopenCommand>
{
    public TrialBalanceReopenCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Trial balance ID is required.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Reason is required to reopen a trial balance.")
            .MinimumLength(10)
            .WithMessage("Reason must be at least 10 characters.")
            .MaximumLength(512)
            .WithMessage("Reason must not exceed 500 characters.");
    }
}

