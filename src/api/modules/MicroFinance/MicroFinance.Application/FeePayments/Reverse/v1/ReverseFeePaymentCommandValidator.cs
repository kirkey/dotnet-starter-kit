using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Reverse.v1;

/// <summary>
/// Validator for the ReverseFeePaymentCommand.
/// </summary>
public class ReverseFeePaymentCommandValidator : AbstractValidator<ReverseFeePaymentCommand>
{
    public ReverseFeePaymentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Fee payment ID is required.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .MaximumLength(FeePayment.ReversalReasonMaxLength)
            .WithMessage($"Reversal reason is required and must not exceed {FeePayment.ReversalReasonMaxLength} characters.");
    }
}
