namespace Accounting.Application.Checks.Create.v1;

/// <summary>
/// Validator for check bundle creation command with range validation rules.
/// Validates check number ranges and ensures proper sequential ordering.
/// </summary>
public class CheckCreateCommandValidator : AbstractValidator<CheckCreateCommand>
{
    public CheckCreateCommandValidator()
    {
        RuleFor(x => x.StartCheckNumber)
            .NotEmpty().WithMessage("Start check number is required.")
            .MaximumLength(64).WithMessage("Start check number cannot exceed 64 characters.")
            .Matches(@"^[a-zA-Z0-9\-_]+$").WithMessage("Start check number can only contain alphanumeric characters, hyphens, and underscores.");

        RuleFor(x => x.EndCheckNumber)
            .NotEmpty().WithMessage("End check number is required.")
            .MaximumLength(64).WithMessage("End check number cannot exceed 64 characters.")
            .Matches(@"^[a-zA-Z0-9\-_]+$").WithMessage("End check number can only contain alphanumeric characters, hyphens, and underscores.");

        RuleFor(x => x)
            .Custom((command, context) =>
            {
                // Validate that both check numbers are numeric (for range comparison)
                if (!long.TryParse(command.StartCheckNumber, out var startNum) ||
                    !long.TryParse(command.EndCheckNumber, out var endNum))
                {
                    context.AddFailure("Check numbers must be numeric for range validation.");
                    return;
                }

                // Validate that end number is greater than or equal to start number
                if (endNum < startNum)
                {
                    context.AddFailure("End check number must be greater than or equal to start check number.");
                    return;
                }

                // Validate range is reasonable (max 10,000 checks per bundle to prevent abuse)
                long rangeSize = endNum - startNum + 1;
                if (rangeSize > 10000)
                {
                    context.AddFailure("Check range cannot exceed 10,000 checks per bundle.");
                    return;
                }

                if (rangeSize < 1)
                {
                    context.AddFailure("Check range must contain at least 1 check.");
                }
            });

        RuleFor(x => x.BankAccountCode)
            .NotEmpty().WithMessage("Bank account code is required.")
            .MaximumLength(64).WithMessage("Bank account code cannot exceed 64 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1024).WithMessage("Description cannot exceed 1024 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1024).WithMessage("Notes cannot exceed 1024 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

