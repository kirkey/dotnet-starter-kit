namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Activate.v1;

/// <summary>
/// Validator for ActivateBankAccountCommand.
/// </summary>
public class ActivateBankAccountValidator : AbstractValidator<ActivateBankAccountCommand>
{
    public ActivateBankAccountValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Bank account ID is required.");
    }
}

