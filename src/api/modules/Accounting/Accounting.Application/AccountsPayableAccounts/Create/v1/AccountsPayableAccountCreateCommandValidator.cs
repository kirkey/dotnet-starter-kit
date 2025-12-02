namespace Accounting.Application.AccountsPayableAccounts.Create.v1;

/// <summary>
/// Validator for accounts payable account creation command.
/// </summary>
public class AccountsPayableAccountCreateCommandValidator : AbstractValidator<AccountsPayableAccountCreateCommand>
{
    public AccountsPayableAccountCreateCommandValidator()
    {
        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("Account number is required")
            .MaximumLength(64).WithMessage("Account number cannot exceed 50 characters");

        RuleFor(x => x.AccountName)
            .NotEmpty().WithMessage("Account name is required")
            .MaximumLength(256).WithMessage("Account name cannot exceed 256 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2048).WithMessage("Description cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048).WithMessage("Notes cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

