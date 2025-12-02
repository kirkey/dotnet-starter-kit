namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Validator for CompleteFiscalPeriodTaskCommand.
/// </summary>
public sealed class CompleteFiscalPeriodTaskCommandValidator : AbstractValidator<CompleteFiscalPeriodTaskCommand>
{
    public CompleteFiscalPeriodTaskCommandValidator()
    {
        RuleFor(x => x.FiscalPeriodCloseId)
            .NotEmpty()
            .WithMessage("Fiscal period close ID is required.");

        RuleFor(x => x.TaskName)
            .NotEmpty()
            .WithMessage("Task name is required.")
            .MaximumLength(256)
            .WithMessage("Task name must not exceed 200 characters.");
    }
}
