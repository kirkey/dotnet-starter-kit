namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Validator for CompleteTaskCommand.
/// </summary>
public sealed class CompleteTaskCommandValidator : AbstractValidator<CompleteTaskCommand>
{
    public CompleteTaskCommandValidator()
    {
        RuleFor(x => x.FiscalPeriodCloseId)
            .NotEmpty()
            .WithMessage("Fiscal period close ID is required.");

        RuleFor(x => x.TaskName)
            .NotEmpty()
            .WithMessage("Task name is required.")
            .MaximumLength(200)
            .WithMessage("Task name must not exceed 200 characters.");
    }
}
