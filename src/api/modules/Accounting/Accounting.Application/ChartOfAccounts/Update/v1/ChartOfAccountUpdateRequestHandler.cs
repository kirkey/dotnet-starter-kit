using Accounting.Application.ChartOfAccounts.Exceptions;
using Accounting.Application.ChartOfAccounts.Queries;
using Accounting.Domain;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.ChartOfAccounts.Update.v1;
public sealed class ChartOfAccountUpdateRequestHandler(
    ILogger<ChartOfAccountUpdateRequestHandler> logger,
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> repository)
    : IRequestHandler<ChartOfAccountUpdateRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ChartOfAccountUpdateRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null) 
            throw new ChartOfAccountNotFoundException(request.Id);

        // Check for duplicate account name (excluding current account)
        if (!string.IsNullOrEmpty(request.AccountName) && request.AccountName != account.AccountName)
        {
            var existingByName = await repository.FirstOrDefaultAsync(
                new ChartOfAccountByNameSpec(request.AccountName), cancellationToken);
            if (existingByName != null)
            {
                throw new ChartOfAccountForbiddenException($"Account name {request.AccountName} already exists");
            }
        }

        var updatedAccount = account.Update(
            accountName: request.AccountName,
            accountType: request.AccountType,
            usoaCategory: request.UsoaCategory,
            subAccountOf: request.SubAccountOf,
            parentCode: request.ParentCode,
            isControlAccount: request.IsControlAccount,
            normalBalance: request.NormalBalance,
            isUsoaCompliant: request.IsUsoaCompliant,
            regulatoryClassification: request.RegulatoryClassification,
            description: request.Description,
            notes: request.Notes);

        await repository.UpdateAsync(updatedAccount, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Account with id: {AccountId} updated.", account.Id);

        return account.Id;
    }
}
