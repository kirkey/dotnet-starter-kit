namespace Accounting.Application.RetainedEarnings.Close.v1;

/// <summary>
/// Validator for CloseRetainedEarningsCommand.
/// The user who closes is automatically determined from the current user session.
/// </summary>
public sealed class CloseRetainedEarningsCommandValidator : AbstractValidator<CloseRetainedEarningsCommand>
{
    public CloseRetainedEarningsCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Retained earnings ID is required.");
    }
}

