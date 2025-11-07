namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Validator for CompleteFiscalPeriodCloseCommand.
/// </summary>
public sealed class CompleteFiscalPeriodCloseCommandValidator : AbstractValidator<CompleteFiscalPeriodCloseCommand>
{
    public CompleteFiscalPeriodCloseCommandValidator()
    {
        RuleFor(x => x.FiscalPeriodCloseId)
            .NotEmpty()
            .WithMessage("Fiscal period close ID is required.");

        RuleFor(x => x.CompletedBy)
            .NotEmpty()
            .WithMessage("Completer information is required.")
            .MaximumLength(200)
            .WithMessage("Completer information must not exceed 200 characters.");
    }
}

