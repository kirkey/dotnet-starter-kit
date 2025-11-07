namespace Accounting.Application.RetainedEarnings.Reopen.v1;

public sealed class ReopenRetainedEarningsCommandValidator : AbstractValidator<ReopenRetainedEarningsCommand>
{
    public ReopenRetainedEarningsCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Retained earnings ID is required.");
        RuleFor(x => x.Reason).NotEmpty().WithMessage("Reason is required to reopen.")
            .MinimumLength(10).WithMessage("Reason must be at least 10 characters.")
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.");
    }
}

