using FluentValidation;

namespace Accounting.Application.AccountsPayableAccounts.Update.v1;

/// <summary>
/// Validator for AccountsPayableAccountUpdateCommand.
/// </summary>
public sealed class AccountsPayableAccountUpdateCommandValidator : AbstractValidator<AccountsPayableAccountUpdateCommand>
{
    public AccountsPayableAccountUpdateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(DefaultIdType.Empty)
            .WithMessage("ID is required");

        RuleFor(x => x.AccountNumber)
            .MaximumLength(50)
            .WithMessage("Account number cannot exceed 50 characters");

        RuleFor(x => x.AccountName)
            .MaximumLength(200)
            .WithMessage("Account name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Notes cannot exceed 1000 characters");
    }
}

