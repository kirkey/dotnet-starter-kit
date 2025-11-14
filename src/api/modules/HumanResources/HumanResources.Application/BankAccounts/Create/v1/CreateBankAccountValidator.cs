namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Create.v1;

/// <summary>
/// Validator for creating a bank account.
/// </summary>
public class CreateBankAccountValidator : AbstractValidator<CreateBankAccountCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBankAccountValidator"/> class.
    /// </summary>
    public CreateBankAccountValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee ID is required");

        RuleFor(x => x.AccountNumber)
            .NotEmpty()
            .WithMessage("Account number is required")
            .MinimumLength(8)
            .WithMessage("Account number must be at least 8 characters");

        RuleFor(x => x.RoutingNumber)
            .NotEmpty()
            .WithMessage("Routing number is required")
            .Length(9)
            .WithMessage("Routing number must be exactly 9 digits");

        RuleFor(x => x.BankName)
            .NotEmpty()
            .WithMessage("Bank name is required")
            .MaximumLength(100)
            .WithMessage("Bank name cannot exceed 100 characters");

        RuleFor(x => x.AccountType)
            .NotEmpty()
            .WithMessage("Account type is required")
            .MaximumLength(50)
            .WithMessage("Account type cannot exceed 50 characters");

        RuleFor(x => x.AccountHolderName)
            .NotEmpty()
            .WithMessage("Account holder name is required")
            .MaximumLength(100)
            .WithMessage("Account holder name cannot exceed 100 characters");

        RuleFor(x => x.SwiftCode)
            .MaximumLength(11)
            .WithMessage("SWIFT code cannot exceed 11 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.SwiftCode));

        RuleFor(x => x.Iban)
            .MaximumLength(34)
            .WithMessage("IBAN cannot exceed 34 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Iban));

        RuleFor(x => x.CurrencyCode)
            .Length(3)
            .WithMessage("Currency code must be exactly 3 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.CurrencyCode));
    }
}

