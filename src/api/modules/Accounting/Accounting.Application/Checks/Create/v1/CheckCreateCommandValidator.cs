namespace Accounting.Application.Checks.Create.v1;

/// <summary>
/// Validator for check creation command with stricter validation rules.
/// </summary>
public class CheckCreateCommandValidator : AbstractValidator<CheckCreateCommand>
{
    public CheckCreateCommandValidator()
    {
        RuleFor(x => x.CheckNumber)
            .NotEmpty().WithMessage("Check number is required.")
            .MaximumLength(64).WithMessage("Check number cannot exceed 64 characters.")
            .Matches(@"^[a-zA-Z0-9\-_]+$").WithMessage("Check number can only contain alphanumeric characters, hyphens, and underscores.");

        RuleFor(x => x.BankAccountCode)
            .NotEmpty().WithMessage("Bank account code is required.")
            .MaximumLength(64).WithMessage("Bank account code cannot exceed 64 characters.");

        RuleFor(x => x.BankAccountName)
            .MaximumLength(256).WithMessage("Bank account name cannot exceed 256 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.BankAccountName));

        RuleFor(x => x.Description)
            .MaximumLength(1024).WithMessage("Description cannot exceed 1024 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1024).WithMessage("Notes cannot exceed 1024 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

