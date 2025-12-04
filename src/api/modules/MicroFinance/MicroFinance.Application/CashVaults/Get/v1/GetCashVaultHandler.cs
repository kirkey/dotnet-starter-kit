using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Get.v1;

public sealed class GetCashVaultHandler(
    [FromKeyedServices("microfinance:cashvaults")] IReadRepository<CashVault> repository)
    : IRequestHandler<GetCashVaultRequest, CashVaultResponse>
{
    public async Task<CashVaultResponse> Handle(GetCashVaultRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var vault = await repository.FirstOrDefaultAsync(
            new CashVaultByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (vault is null)
            throw new NotFoundException($"Cash vault with ID {request.Id} not found.");

        return new CashVaultResponse(
            vault.Id,
            vault.BranchId,
            vault.Code,
            vault.Name,
            vault.VaultType,
            vault.Status,
            vault.CurrentBalance,
            vault.OpeningBalance,
            vault.MinimumBalance,
            vault.MaximumBalance,
            vault.Location,
            vault.CustodianName,
            vault.CustodianUserId,
            vault.LastReconciliationDate,
            vault.LastReconciledBalance,
            vault.Notes);
    }
}
