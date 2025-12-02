namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Create.v1;

/// <summary>
/// Validator for CreateBankAccountCommand with security and format checks.
/// </summary>
public class CreateBankAccountValidator : AbstractValidator<CreateBankAccountCommand>
{
    public CreateBankAccountValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("Account number is required.")
            .MinimumLength(8).WithMessage("Account number must be at least 8 characters.")
            .MaximumLength(32).WithMessage("Account number must not exceed 20 characters.")
            .Matches(@"^\d+$").WithMessage("Account number must contain only digits.");

        RuleFor(x => x.RoutingNumber)
            .NotEmpty().WithMessage("Routing number is required.")
            .Length(9).WithMessage("Routing number must be exactly 9 digits.")
            .Matches(@"^\d{9}$").WithMessage("Routing number must be exactly 9 digits.");

        RuleFor(x => x.BankName)
            .NotEmpty().WithMessage("Bank name is required.")
            .MaximumLength(128).WithMessage("Bank name must not exceed 100 characters.");

        RuleFor(x => x.AccountType)
            .NotEmpty().WithMessage("Account type is required.")
            .Must(t => new[] { "Checking", "Savings", "MoneyMarket", "Other" }.Contains(t))
            .WithMessage("Account type must be: Checking, Savings, MoneyMarket, or Other.");

        RuleFor(x => x.AccountHolderName)
            .NotEmpty().WithMessage("Account holder name is required.")
            .MaximumLength(128).WithMessage("Account holder name must not exceed 100 characters.");

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

