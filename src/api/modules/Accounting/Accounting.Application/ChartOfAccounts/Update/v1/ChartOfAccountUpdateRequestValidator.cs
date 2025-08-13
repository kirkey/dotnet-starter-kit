using FluentValidation;
using FSH.Framework.Core.Extensions;

namespace Accounting.Application.ChartOfAccounts.Update.v1;
public class ChartOfAccountUpdateRequestValidator : BaseRequestValidator<ChartOfAccountUpdateRequest>
{
    public ChartOfAccountUpdateRequestValidator()
    {
            RuleFor(a => a.AccountName)
                .MaximumLength(1024)
                .When(x => !string.IsNullOrEmpty(x.AccountName));

            RuleFor(a => a.AccountType)
                .MaximumLength(32)
                .When(x => !string.IsNullOrEmpty(x.AccountType));

            RuleFor(a => a.UsoaCategory)
                .MaximumLength(16)
                .When(x => !string.IsNullOrEmpty(x.UsoaCategory));

            RuleFor(a => a.ParentCode)
                .MaximumLength(16)
                .When(x => !string.IsNullOrEmpty(x.ParentCode));

            RuleFor(a => a.AccountCode)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(16);

            RuleFor(a => a.Balance)
                .GreaterThanOrEqualTo(0);
    }
}
