namespace Accounting.Application.SecurityDeposits.Commands;

/// <summary>
/// Validator for CreateSecurityDepositCommand.
/// Enforces validation rules for security deposit creation.
/// </summary>
public sealed class CreateSecurityDepositCommandValidator : AbstractValidator<CreateSecurityDepositCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSecurityDepositCommandValidator class.
    /// </summary>
    public CreateSecurityDepositCommandValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty()
            .WithMessage("Member ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Deposit amount must be greater than zero.")
            .LessThanOrEqualTo(999999.99m)
            .WithMessage("Deposit amount cannot exceed 999,999.99.");

        RuleFor(x => x.DepositDate)
            .NotEmpty()
            .WithMessage("Deposit date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Deposit date cannot be in the future.");

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Notes cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

