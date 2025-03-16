using FluentValidation;
using FSH.Framework.Core.Extensions;

namespace Accounting.Application.Accounts.Update.v1;
public class AccountUpdateRequestValidator : BaseRequestValidator<AccountUpdateRequest>
{
    public AccountUpdateRequestValidator()
    {
        RuleFor(a => a.AccountCategory.ToString())
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(16);
        
        RuleFor(a => a.ParentCode)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(16);
        
        RuleFor(a => a.Code)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(16);
        
        RuleFor(a => a.Balance)
            .GreaterThanOrEqualTo(0);
    }
}
