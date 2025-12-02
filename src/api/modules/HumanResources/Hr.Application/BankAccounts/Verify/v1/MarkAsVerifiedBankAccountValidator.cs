namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Verify.v1;

/// <summary>
/// Validator for MarkAsVerifiedBankAccountCommand.
/// </summary>
public class MarkAsVerifiedBankAccountValidator : AbstractValidator<MarkAsVerifiedBankAccountCommand>
{
    public MarkAsVerifiedBankAccountValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Bank account ID is required.");

        RuleFor(x => x.Notes)
            .MaximumLength(512).WithMessage("Notes must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

