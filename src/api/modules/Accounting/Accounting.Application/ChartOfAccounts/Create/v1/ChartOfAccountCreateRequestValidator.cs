using FSH.Framework.Core.Extensions;

namespace Accounting.Application.ChartOfAccounts.Create.v1;

public class ChartOfAccountCreateRequestValidator : BaseRequestValidator<ChartOfAccountCreateRequest>
{
    public ChartOfAccountCreateRequestValidator()
    {
            RuleFor(a => a.AccountCode)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(16);

            RuleFor(a => a.AccountName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(1024);

            RuleFor(a => a.AccountType)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(32);

            RuleFor(a => a.UsoaCategory)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(16);

            RuleFor(a => a.ParentCode)
                .MaximumLength(16);

            RuleFor(a => a.Balance)
                .GreaterThanOrEqualTo(0);
    }
}
