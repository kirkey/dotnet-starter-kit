using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Reconcile.v1;

public sealed class ReconcileCashVaultHandler(
    ILogger<ReconcileCashVaultHandler> logger,
    [FromKeyedServices("microfinance:cashvaults")] IRepository<CashVault> repository)
    : IRequestHandler<ReconcileCashVaultCommand, ReconcileCashVaultResponse>
{
    public async Task<ReconcileCashVaultResponse> Handle(ReconcileCashVaultCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var vault = await repository.FirstOrDefaultAsync(
            new CashVaultByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (vault is null)
            throw new NotFoundException($"Cash vault with ID {request.Id} not found.");

        var expectedBalance = vault.CurrentBalance;
        vault.Reconcile(request.PhysicalCount, request.DenominationBreakdown);

        await repository.UpdateAsync(vault, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var variance = request.PhysicalCount - expectedBalance;
        logger.LogInformation("Cash vault {VaultId} reconciled. Expected: {Expected}, Actual: {Actual}, Variance: {Variance}",
            vault.Id, expectedBalance, request.PhysicalCount, variance);

        return new ReconcileCashVaultResponse(
            vault.Id,
            expectedBalance,
            request.PhysicalCount,
            variance,
            vault.LastReconciliationDate!.Value);
    }
}
