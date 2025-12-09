using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Create.v1;

/// <summary>
/// Validator for the CreateFeePaymentCommand.
/// </summary>
public class CreateFeePaymentCommandValidator : AbstractValidator<CreateFeePaymentCommand>
{
    public CreateFeePaymentCommandValidator()
    {
        RuleFor(x => x.FeeChargeId)
            .NotEmpty()
            .WithMessage("Fee charge ID is required.");

        RuleFor(x => x.Reference)
            .NotEmpty()
            .MaximumLength(FeePayment.ReferenceMaxLength)
            .WithMessage($"Reference is required and must not exceed {FeePayment.ReferenceMaxLength} characters.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .MaximumLength(FeePayment.PaymentMethodMaxLength)
            .WithMessage($"Payment method is required and must not exceed {FeePayment.PaymentMethodMaxLength} characters.");

        RuleFor(x => x.PaymentSource)
            .NotEmpty()
            .MaximumLength(FeePayment.PaymentSourceMaxLength)
            .WithMessage($"Payment source is required and must not exceed {FeePayment.PaymentSourceMaxLength} characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(FeePayment.NotesMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
