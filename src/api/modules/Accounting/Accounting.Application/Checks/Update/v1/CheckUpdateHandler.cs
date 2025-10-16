using Accounting.Application.Banks.Queries;
using Accounting.Application.ChartOfAccounts.Specs;
using Accounting.Application.Checks.Exceptions;
using Accounting.Application.Checks.Queries;
using Accounting.Domain.Entities;

namespace Accounting.Application.Checks.Update.v1;

/// <summary>
/// Handler for updating an existing check.
/// Only available checks can be updated.
/// </summary>
public sealed class CheckUpdateHandler(
    ILogger<CheckUpdateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Check> checkRepository,
    [FromKeyedServices("accounting")] IRepository<Bank> bankRepository,
    [FromKeyedServices("accounting")] IRepository<ChartOfAccount> chartOfAccountRepository)
    : IRequestHandler<CheckUpdateCommand, CheckUpdateResponse>
{
    public async Task<CheckUpdateResponse> Handle(CheckUpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Get the check to update
        var check = await checkRepository.GetByIdAsync(request.CheckId, cancellationToken)
            ?? throw new CheckNotFoundException(request.CheckId);

        // Fetch bank account name from ChartOfAccount
        string? bankAccountName = null;
        if (!string.IsNullOrWhiteSpace(request.BankAccountCode))
        {
            var chartOfAccount = await chartOfAccountRepository.FirstOrDefaultAsync(
                new ChartOfAccountByCodeSpec(request.BankAccountCode),
                cancellationToken);

            if (chartOfAccount != null)
            {
                bankAccountName = chartOfAccount.AccountName;
            }
        }

        // Fetch bank name from Bank entity
        string? bankName = null;
        if (request.BankId.HasValue && request.BankId.Value != DefaultIdType.Empty)
        {
            var bank = await bankRepository.FirstOrDefaultAsync(
                new BankByIdSpec(request.BankId.Value),
                cancellationToken);

            if (bank != null)
            {
                bankName = bank.Name;
            }
        }

        // Update the check
        check.Update(
            request.BankAccountCode,
            bankAccountName,
            request.BankId,
            bankName,
            request.Description,
            request.Notes);

        await checkRepository.UpdateAsync(check, cancellationToken);
        await checkRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Check updated: {CheckId} - {CheckNumber}", check.Id, check.CheckNumber);
        return new CheckUpdateResponse(check.Id);
    }
}
