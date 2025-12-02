namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Update.v1;

/// <summary>
/// Validator for UpdateBankAccountCommand.
/// </summary>
public class UpdateBankAccountValidator : AbstractValidator<UpdateBankAccountCommand>
{
    public UpdateBankAccountValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Bank account ID is required.");

        RuleFor(x => x.BankName)
            .MaximumLength(128).WithMessage("Bank name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.BankName));

        RuleFor(x => x.AccountHolderName)
            .MaximumLength(128).WithMessage("Account holder name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.AccountHolderName));

        RuleFor(x => x.SwiftCode)
            .MaximumLength(16).WithMessage("SWIFT code must not exceed 16 characters.")
            .Matches(@"^[A-Z0-9]+$").WithMessage("SWIFT code must contain only uppercase letters and digits.")
            .When(x => !string.IsNullOrWhiteSpace(x.SwiftCode));

        RuleFor(x => x.Iban)
            .MaximumLength(64).WithMessage("IBAN must not exceed 64 characters.")
            .Matches(@"^[A-Z0-9]+$").WithMessage("IBAN must contain only uppercase letters and digits.")
            .When(x => !string.IsNullOrWhiteSpace(x.Iban));

        RuleFor(x => x.Notes)
            .MaximumLength(512).WithMessage("Notes must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

