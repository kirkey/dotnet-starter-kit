using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.RecordPayment.v1;

/// <summary>
/// Validator for RecordFeePaymentCommand.
/// </summary>
public class RecordFeePaymentCommandValidator : AbstractValidator<RecordFeePaymentCommand>
{
    public RecordFeePaymentCommandValidator()
    {
        RuleFor(x => x.FeeChargeId)
            .NotEmpty().WithMessage("Fee charge ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Payment amount must be greater than zero.");
    }
}
