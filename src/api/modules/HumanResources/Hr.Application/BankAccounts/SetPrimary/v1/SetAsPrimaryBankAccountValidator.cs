namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.SetPrimary.v1;

/// <summary>
/// Validator for SetAsPrimaryBankAccountCommand.
/// </summary>
public class SetAsPrimaryBankAccountValidator : AbstractValidator<SetAsPrimaryBankAccountCommand>
{
    public SetAsPrimaryBankAccountValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Bank account ID is required.");
    }
}

