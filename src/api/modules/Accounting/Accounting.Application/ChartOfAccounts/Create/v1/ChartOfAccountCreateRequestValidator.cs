using FluentValidation;
using FSH.Framework.Core.Extensions;

namespace Accounting.Application.ChartOfAccounts.Create.v1;

public class ChartOfAccountCreateRequestValidator : BaseRequestValidator<ChartOfAccountCreateRequest>
{
    public ChartOfAccountCreateRequestValidator()
    {
        RuleFor(a => a.AccountCategory)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(16);
        
        RuleFor(a => a.AccountType)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(16);
        
        RuleFor(a => a.AccountCode)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(16);
        
        RuleFor(a => a.Balance)
            .GreaterThanOrEqualTo(0);
    }
}
