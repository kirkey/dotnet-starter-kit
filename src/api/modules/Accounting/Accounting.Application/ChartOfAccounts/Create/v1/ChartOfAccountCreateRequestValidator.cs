using FSH.Framework.Core.Extensions;

namespace Accounting.Application.ChartOfAccounts.Create.v1;

public class ChartOfAccountCreateRequestValidator : BaseRequestValidator<ChartOfAccountCreateRequest>
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
                .MaximumLength(32)
                .Must(at => AllowedAccountTypes.Contains(at.Trim()))
                .WithMessage(a => $"AccountType must be one of: {string.Join(", ", AllowedAccountTypes)}");

            RuleFor(a => a.UsoaCategory)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(16)
                .Must(uc => AllowedUsoaCategories.Contains(uc.Trim()))
                .WithMessage(a => $"UsoaCategory must be one of: {string.Join(", ", AllowedUsoaCategories)}");

            RuleFor(a => a.ParentCode)
                .MaximumLength(16);

            RuleFor(a => a.Balance)
                .GreaterThanOrEqualTo(0);
    }
}
