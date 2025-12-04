using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Create.v1;

public sealed class CreateLoanRepaymentCommandValidator : AbstractValidator<CreateLoanRepaymentCommand>
{
    public CreateLoanRepaymentCommandValidator()
    {
        RuleFor(x => x.LoanId)
            .NotEmpty()
            .WithMessage("Loan ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Payment amount must be greater than 0.");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .Must(BeValidPaymentMethod)
            .WithMessage("Payment method must be 'CASH', 'BANK_TRANSFER', 'MOBILE_MONEY', 'CHEQUE', or 'DEBIT_CARD'.");
    }

    private static bool BeValidPaymentMethod(string method) =>
        method is "CASH" or "BANK_TRANSFER" or "MOBILE_MONEY" or "CHEQUE" or "DEBIT_CARD";
}
