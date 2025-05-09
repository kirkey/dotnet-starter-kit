using FluentValidation;
using FSH.Framework.Core.Extensions;

namespace Accounting.Application.ChartOfAccounts.Update.v1;
public class ChartOfAccountUpdateRequestValidator : BaseRequestValidator<ChartOfAccountUpdateRequest>
{
    public ChartOfAccountUpdateRequestValidator()
    {
        RuleFor(a => a.AccountCategory)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(16);
        
        RuleFor(a => a.AccountType)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(16);
        
        RuleFor(a => a.ParentCode)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(16);
        
        RuleFor(a => a.AccountCode)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(16);
        
        RuleFor(a => a.Balance)
            .GreaterThanOrEqualTo(0);
    }
}
