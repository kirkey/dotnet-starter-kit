namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Update.v1;

/// <summary>
/// Validator for updating a bank account.
/// </summary>
public class UpdateBankAccountValidator : AbstractValidator<UpdateBankAccountCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateBankAccountValidator"/> class.
    /// </summary>
    public UpdateBankAccountValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Bank account ID is required");

        RuleFor(x => x.BankName)
            .MaximumLength(100)
            .WithMessage("Bank name cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.BankName));

        RuleFor(x => x.AccountHolderName)
            .MaximumLength(100)
            .WithMessage("Account holder name cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.AccountHolderName));

        RuleFor(x => x.SwiftCode)
            .MaximumLength(11)
            .WithMessage("SWIFT code cannot exceed 11 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.SwiftCode));

        RuleFor(x => x.Iban)
            .MaximumLength(34)
            .WithMessage("IBAN cannot exceed 34 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Iban));

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

