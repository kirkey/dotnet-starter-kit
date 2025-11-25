namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Deactivate.v1;

/// <summary>
/// Validator for DeactivateBankAccountCommand.
/// </summary>
public class DeactivateBankAccountValidator : AbstractValidator<DeactivateBankAccountCommand>
{
    public DeactivateBankAccountValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Bank account ID is required.");

        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}

