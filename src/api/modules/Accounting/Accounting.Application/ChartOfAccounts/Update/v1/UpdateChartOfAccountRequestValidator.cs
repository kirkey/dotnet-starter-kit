using FSH.Framework.Core.Extensions;

namespace Accounting.Application.ChartOfAccounts.Update.v1;
public class UpdateChartOfAccountRequestValidator : BaseRequestValidator<UpdateChartOfAccountRequest>
{
    private static readonly HashSet<string> AllowedAccountTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "Asset",
        "Liability",
        "Equity",
        "Revenue",
        "Expense",
    };

    private static readonly HashSet<string> AllowedUsoaCategories = new(StringComparer.OrdinalIgnoreCase)
    {
        "Production",
        "Transmission",
        "Distribution",
        "Customer Accounts",
        "Customer Service",
        "Sales",
        "Administrative",
        "General",
        "Maintenance",
        "Operation",
    };

    public UpdateChartOfAccountRequestValidator()
    {
            RuleFor(a => a.AccountName)
                .MaximumLength(1024)
                .When(x => !string.IsNullOrEmpty(x.AccountName));

            RuleFor(a => a.AccountType)
                .MaximumLength(32)
                .Must(at => string.IsNullOrEmpty(at) || AllowedAccountTypes.Contains(at!.Trim()))
                .WithMessage(a => $"AccountType must be one of: {string.Join(", ", AllowedAccountTypes)}")
                .When(x => !string.IsNullOrEmpty(x.AccountType));

            RuleFor(a => a.UsoaCategory)
                .MaximumLength(16)
                .Must(uc => string.IsNullOrEmpty(uc) || AllowedUsoaCategories.Contains(uc!.Trim()))
                .WithMessage(a => $"UsoaCategory must be one of: {string.Join(", ", AllowedUsoaCategories)}")
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
