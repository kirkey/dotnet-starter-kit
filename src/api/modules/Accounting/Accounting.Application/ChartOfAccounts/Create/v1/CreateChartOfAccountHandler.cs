using Accounting.Application.ChartOfAccounts.Exceptions;
using Accounting.Application.ChartOfAccounts.Specs;

namespace Accounting.Application.ChartOfAccounts.Create.v1;
public sealed class CreateChartOfAccountHandler(
    ILogger<CreateChartOfAccountHandler> logger,
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> repository)
    : IRequestHandler<CreateChartOfAccountCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateChartOfAccountCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var accountCode = command.AccountCode?.Trim() ?? string.Empty;
        var accountName = command.AccountName?.Trim() ?? string.Empty;
        var accountType = command.AccountType?.Trim() ?? string.Empty;
        var usoaCategory = command.UsoaCategory?.Trim() ?? string.Empty;
        var parentCode = command.ParentCode?.Trim() ?? string.Empty;
        var normalBalance = command.NormalBalance?.Trim() ?? "Debit";

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
            parentAccountId: command.ParentAccountId,
            parentCode: parentCode,
            balance: command.Balance,
            isControlAccount: command.IsControlAccount,
            normalBalance: normalBalance,
            isUsoaCompliant: command.IsUsoaCompliant,
            regulatoryClassification: command.RegulatoryClassification?.Trim(),
            description: command.Description?.Trim(),
            notes: command.Notes?.Trim());

        await repository.AddAsync(account, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Account created {AccountId}", account.Id);

        return account.Id;
    }
}
