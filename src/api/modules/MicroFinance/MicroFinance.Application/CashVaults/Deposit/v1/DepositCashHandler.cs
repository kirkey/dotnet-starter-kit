using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Deposit.v1;

public sealed class DepositCashHandler(
    ILogger<DepositCashHandler> logger,
    [FromKeyedServices("microfinance:cashvaults")] IRepository<CashVault> repository)
    : IRequestHandler<DepositCashCommand, DepositCashResponse>
{
    public async Task<DepositCashResponse> Handle(DepositCashCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var vault = await repository.FirstOrDefaultAsync(
            new CashVaultByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (vault is null)
            throw new NotFoundException($"Cash vault with ID {request.Id} not found.");

        var previousBalance = vault.CurrentBalance;
        vault.Deposit(request.Amount, request.DenominationBreakdown);

        await repository.UpdateAsync(vault, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Cash deposited to vault {VaultId}: {Amount}, New Balance: {NewBalance}",
            vault.Id, request.Amount, vault.CurrentBalance);

        return new DepositCashResponse(vault.Id, previousBalance, vault.CurrentBalance, request.Amount);
    }
}
