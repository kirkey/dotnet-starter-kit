using Accounting.Application.PaymentAllocations.Commands;

namespace Accounting.Application.PaymentAllocations.Validators;

public class DeletePaymentAllocationCommandValidator : AbstractValidator<DeletePaymentAllocationCommand>
{
    public DeletePaymentAllocationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}


