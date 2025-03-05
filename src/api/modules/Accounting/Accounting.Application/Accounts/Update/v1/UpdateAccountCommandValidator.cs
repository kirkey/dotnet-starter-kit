using FluentValidation;

namespace Accounting.Application.Accounts.Update.v1;
public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(a => a.Category.ToString())
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(16);
        
        RuleFor(a => a.TransactionType.ToString())
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(8);
        
        RuleFor(a => a.ParentCode)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(16);
        
        RuleFor(a => a.Code)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(16);
        
        RuleFor(a => a.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(1024);
        
        RuleFor(a => a.Balance)
            .GreaterThanOrEqualTo(0);
    }
}
