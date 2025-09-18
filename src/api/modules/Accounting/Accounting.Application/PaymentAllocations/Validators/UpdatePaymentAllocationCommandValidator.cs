using Accounting.Application.PaymentAllocations.Commands;

namespace Accounting.Application.PaymentAllocations.Validators;

public class UpdatePaymentAllocationCommandValidator : AbstractValidator<UpdatePaymentAllocationCommand>
{
    public UpdatePaymentAllocationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).When(x => x.Amount.HasValue);
        RuleFor(x => x.Notes).MaximumLength(2048).When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

