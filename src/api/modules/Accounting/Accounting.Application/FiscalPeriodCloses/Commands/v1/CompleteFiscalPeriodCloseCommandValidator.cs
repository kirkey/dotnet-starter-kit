namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Validator for CompleteFiscalPeriodCloseCommand.
/// The completer is automatically determined from the current user session.
/// </summary>
public sealed class CompleteFiscalPeriodCloseCommandValidator : AbstractValidator<CompleteFiscalPeriodCloseCommand>
{
    public CompleteFiscalPeriodCloseCommandValidator()
    {
        RuleFor(x => x.FiscalPeriodCloseId)
            .NotEmpty()
            .WithMessage("Fiscal period close ID is required.");
    }
}

