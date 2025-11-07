namespace Accounting.Application.RetainedEarnings.Close.v1;

public sealed class CloseRetainedEarningsCommandValidator : AbstractValidator<CloseRetainedEarningsCommand>
{
    public CloseRetainedEarningsCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Retained earnings ID is required.");
        RuleFor(x => x.ClosedBy).NotEmpty().WithMessage("Closed by information is required.")
            .MaximumLength(256).WithMessage("Closed by must not exceed 256 characters.");
    }
}

