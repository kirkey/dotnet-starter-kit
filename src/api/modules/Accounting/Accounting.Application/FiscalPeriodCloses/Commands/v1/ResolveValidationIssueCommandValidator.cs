namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Validator for ResolveValidationIssueCommand.
/// </summary>
public sealed class ResolveValidationIssueCommandValidator : AbstractValidator<ResolveValidationIssueCommand>
{
    public ResolveValidationIssueCommandValidator()
    {
        RuleFor(x => x.FiscalPeriodCloseId)
            .NotEmpty()
            .WithMessage("Fiscal period close ID is required.");

        RuleFor(x => x.IssueDescription)
            .NotEmpty()
            .WithMessage("Issue description is required.")
            .MaximumLength(500)
            .WithMessage("Issue description must not exceed 500 characters.");

        RuleFor(x => x.Resolution)
            .NotEmpty()
            .WithMessage("Resolution is required.")
            .MinimumLength(10)
            .WithMessage("Resolution must be at least 10 characters.")
            .MaximumLength(500)
            .WithMessage("Resolution must not exceed 500 characters.");
    }
}

