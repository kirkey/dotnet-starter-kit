using FluentValidation;

namespace Accounting.Application.Payees.Create.v1;
public class PayeeCreateCommandValidator : AbstractValidator<PayeeCreateCommand>
{
    public PayeeCreateCommandValidator()
    {
        RuleFor(p => p.PayeeCode)
            .NotEmpty()
            .MaximumLength(32);
        
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(1024);
    }
}
