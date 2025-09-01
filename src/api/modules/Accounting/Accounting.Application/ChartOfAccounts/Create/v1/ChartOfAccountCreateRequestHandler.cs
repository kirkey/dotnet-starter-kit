using Accounting.Application.ChartOfAccounts.Exceptions;
using Accounting.Application.ChartOfAccounts.Queries;
using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.ChartOfAccounts.Create.v1;
public sealed class ChartOfAccountCreateRequestHandler(
    ILogger<ChartOfAccountCreateRequestHandler> logger,
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> repository)
    : IRequestHandler<ChartOfAccountCreateRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ChartOfAccountCreateRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate account code
        var existingByCode = await repository.FirstOrDefaultAsync(
            new ChartOfAccountByCodeSpec(request.AccountCode), cancellationToken);
        if (existingByCode != null)
        {
            throw new ChartOfAccountForbiddenException($"Account code {request.AccountCode} already exists");
        }

        // Check for duplicate account name
        var existingByName = await repository.FirstOrDefaultAsync(
            new ChartOfAccountByNameSpec(request.AccountName), cancellationToken);
        if (existingByName != null)
        {
            throw new ChartOfAccountForbiddenException($"Account name {request.AccountName} already exists");
        }

        var account = ChartOfAccount.Create(
            accountId: request.AccountCode,
            accountName: request.AccountName,
            accountType: request.AccountType,
            usoaCategory: request.UsoaCategory,
            subAccountOf: request.SubAccountOf,
            parentCode: request.ParentCode,
            balance: request.Balance,
            isControlAccount: request.IsControlAccount,
            normalBalance: request.NormalBalance,
            isUsoaCompliant: request.IsUsoaCompliant,
            regulatoryClassification: request.RegulatoryClassification,
            description: request.Description,
            notes: request.Notes);

        await repository.AddAsync(account, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Account created {AccountId}", account.Id);

        return account.Id;
    }
}
