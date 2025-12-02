namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Validator for AddFiscalPeriodCloseValidationIssueCommand.
/// </summary>
public sealed class AddFiscalPeriodCloseValidationIssueCommandValidator : AbstractValidator<AddFiscalPeriodCloseValidationIssueCommand>
{
    public AddFiscalPeriodCloseValidationIssueCommandValidator()
    {
        RuleFor(x => x.FiscalPeriodCloseId)
            .NotEmpty()
            .WithMessage("Fiscal period close ID is required.");

        RuleFor(x => x.IssueDescription)
            .NotEmpty()
            .WithMessage("Issue description is required.")
            .MaximumLength(512)
            .WithMessage("Issue description must not exceed 500 characters.");

        RuleFor(x => x.Severity)
            .NotEmpty()
            .WithMessage("Severity is required.")
            .Must(s => new[] { "Critical", "High", "Medium", "Low" }.Contains(s))
            .WithMessage("Severity must be one of: Critical, High, Medium, Low.");

        RuleFor(x => x.Resolution)
            .MaximumLength(512)
            .WithMessage("Resolution must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Resolution));
    }
}

