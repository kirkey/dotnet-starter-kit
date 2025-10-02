namespace Accounting.Application.BankReconciliations.Create.v1;

public class CreateBankReconciliationRequestValidator : AbstractValidator<CreateBankReconciliationCommand>
{
    public CreateBankReconciliationRequestValidator()
    {
        RuleFor(x => x.BankAccountId)
            .NotEmpty()
            .WithMessage("Bank account is required");

        RuleFor(x => x.ReconciliationDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Reconciliation date cannot be in the future");

        RuleFor(x => x.StatementBalance)
            .NotEmpty()
            .WithMessage("Statement balance is required");

        RuleFor(x => x.BookBalance)
            .NotEmpty()
            .WithMessage("Book balance is required");
    }
}
