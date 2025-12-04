using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Withdraw.v1;

public sealed class WithdrawCashHandler(
    ILogger<WithdrawCashHandler> logger,
    [FromKeyedServices("microfinance:cashvaults")] IRepository<CashVault> repository)
    : IRequestHandler<WithdrawCashCommand, WithdrawCashResponse>
{
    public async Task<WithdrawCashResponse> Handle(WithdrawCashCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var vault = await repository.FirstOrDefaultAsync(
            new CashVaultByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (vault is null)
            throw new NotFoundException($"Cash vault with ID {request.Id} not found.");

        var previousBalance = vault.CurrentBalance;
        vault.Withdraw(request.Amount, request.DenominationBreakdown);

        await repository.UpdateAsync(vault, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Cash withdrawn from vault {VaultId}: {Amount}, New Balance: {NewBalance}",
            vault.Id, request.Amount, vault.CurrentBalance);

        return new WithdrawCashResponse(vault.Id, previousBalance, vault.CurrentBalance, request.Amount);
    }
}
