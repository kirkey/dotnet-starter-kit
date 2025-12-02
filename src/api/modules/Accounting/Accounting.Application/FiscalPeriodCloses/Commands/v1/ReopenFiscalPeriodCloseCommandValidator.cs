namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Validator for ReopenFiscalPeriodCloseCommand.
/// </summary>
public sealed class ReopenFiscalPeriodCloseCommandValidator : AbstractValidator<ReopenFiscalPeriodCloseCommand>
{
    public ReopenFiscalPeriodCloseCommandValidator()
    {
        RuleFor(x => x.FiscalPeriodCloseId)
            .NotEmpty()
            .WithMessage("Fiscal period close ID is required.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Reason is required to reopen a fiscal period close.")
            .MinimumLength(10)
            .WithMessage("Reason must be at least 10 characters.")
            .MaximumLength(512)
            .WithMessage("Reason must not exceed 500 characters.");

        RuleFor(x => x.ReopenedBy)
            .NotEmpty()
            .WithMessage("Reopener information is required.")
            .MaximumLength(256)
            .WithMessage("Reopener information must not exceed 200 characters.");
    }
}
