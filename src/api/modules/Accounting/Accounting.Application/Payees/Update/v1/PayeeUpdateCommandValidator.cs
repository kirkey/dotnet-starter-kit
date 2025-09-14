namespace Accounting.Application.Payees.Update.v1;
public class PayeeUpdateCommandValidator : AbstractValidator<PayeeUpdateCommand>
{
    public PayeeUpdateCommandValidator()
    {
        RuleFor(p => p.PayeeCode)
            .NotEmpty()
            .MaximumLength(32);
        
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(1024);
    }
}
