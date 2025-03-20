using FluentValidation;
using FSH.Framework.Core.Extensions;

namespace Accounting.Application.Accounts.Create.v1;

public class AccountCreateRequestValidator : BaseRequestValidator<AccountCreateRequest>
{
    public AccountCreateRequestValidator()
    {
        RuleFor(a => a.AccountCategory)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(16);
        
        RuleFor(a => a.AccountType)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(16);
        
        RuleFor(a => a.Code)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(16);
        
        RuleFor(a => a.Balance)
            .GreaterThanOrEqualTo(0);
    }
}
