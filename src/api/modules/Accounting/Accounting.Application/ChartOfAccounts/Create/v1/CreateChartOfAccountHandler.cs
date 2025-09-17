using Accounting.Application.ChartOfAccounts.Exceptions;
using Accounting.Application.ChartOfAccounts.Queries;

namespace Accounting.Application.ChartOfAccounts.Create.v1;
public sealed class CreateChartOfAccountHandler(
    ILogger<CreateChartOfAccountHandler> logger,
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> repository)
    : IRequestHandler<CreateChartOfAccountRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateChartOfAccountRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var accountCode = request.AccountCode?.Trim() ?? string.Empty;
        var accountName = request.AccountName?.Trim() ?? string.Empty;
        var accountType = request.AccountType?.Trim() ?? string.Empty;
        var usoaCategory = request.UsoaCategory?.Trim() ?? string.Empty;
        var parentCode = request.ParentCode?.Trim() ?? string.Empty;
        var normalBalance = request.NormalBalance?.Trim() ?? "Debit";

        // Check for duplicate account code
        var existingByCode = await repository.FirstOrDefaultAsync(
            new ChartOfAccountByCodeSpec(accountCode), cancellationToken);
        if (existingByCode != null)
        {
            throw new ChartOfAccountForbiddenException($"Account code {accountCode} already exists");
        }

        // Check for duplicate account name
        var existingByName = await repository.FirstOrDefaultAsync(
            new ChartOfAccountByNameSpec(accountName), cancellationToken);
        if (existingByName != null)
        {
            throw new ChartOfAccountForbiddenException($"Account name {accountName} already exists");
        }

        var account = ChartOfAccount.Create(
            accountId: accountCode,
            accountName: accountName,
            accountType: accountType,
            usoaCategory: usoaCategory,
            subAccountOf: request.SubAccountOf,
            parentCode: parentCode,
            balance: request.Balance,
            isControlAccount: request.IsControlAccount,
            normalBalance: normalBalance,
            isUsoaCompliant: request.IsUsoaCompliant,
            regulatoryClassification: request.RegulatoryClassification?.Trim(),
            description: request.Description?.Trim(),
            notes: request.Notes?.Trim());

        await repository.AddAsync(account, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Account created {AccountId}", account.Id);

        return account.Id;
    }
}
