using Accounting.Application.PaymentAllocations.Commands;

namespace Accounting.Application.PaymentAllocations.Validators;

public class CreatePaymentAllocationCommandValidator : AbstractValidator<CreatePaymentAllocationCommand>
{
    public CreatePaymentAllocationCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.InvoiceId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Notes).MaximumLength(2048).When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

