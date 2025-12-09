using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Update.v1;

/// <summary>
/// Validator for the UpdateFeePaymentCommand.
/// </summary>
public class UpdateFeePaymentCommandValidator : AbstractValidator<UpdateFeePaymentCommand>
{
    public UpdateFeePaymentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Fee payment ID is required.");

        RuleFor(x => x.Reference)
            .MaximumLength(FeePayment.ReferenceMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Reference));

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .When(x => x.Amount.HasValue);

        RuleFor(x => x.PaymentMethod)
            .MaximumLength(FeePayment.PaymentMethodMaxLength)
            .When(x => !string.IsNullOrEmpty(x.PaymentMethod));

        RuleFor(x => x.PaymentSource)
            .MaximumLength(FeePayment.PaymentSourceMaxLength)
            .When(x => !string.IsNullOrEmpty(x.PaymentSource));

        RuleFor(x => x.Notes)
            .MaximumLength(FeePayment.NotesMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
