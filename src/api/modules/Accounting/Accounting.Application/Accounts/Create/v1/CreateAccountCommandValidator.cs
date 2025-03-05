using FluentValidation;

namespace Accounting.Application.Accounts.Create.v1;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(a => a.Category.ToString())
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(16);
        
        RuleFor(a => a.TransactionType.ToString())
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(8);
        
        RuleFor(a => a.Code)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(16);
        
        RuleFor(a => a.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(1024);
        
        RuleFor(a => a.Balance)
            .GreaterThanOrEqualTo(0);
    }
}
