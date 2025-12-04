using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Create.v1;

public sealed class CreateCashVaultHandler(
    ILogger<CreateCashVaultHandler> logger,
    [FromKeyedServices("microfinance:cashvaults")] IRepository<CashVault> repository)
    : IRequestHandler<CreateCashVaultCommand, CreateCashVaultResponse>
{
    public async Task<CreateCashVaultResponse> Handle(CreateCashVaultCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var vault = CashVault.Create(
            branchId: request.BranchId,
            code: request.Code,
            name: request.Name,
            vaultType: request.VaultType,
            minimumBalance: request.MinimumBalance,
            maximumBalance: request.MaximumBalance,
            openingBalance: request.OpeningBalance,
            location: request.Location,
            custodianName: request.CustodianName,
            custodianUserId: request.CustodianUserId);

        await repository.AddAsync(vault, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Cash vault created: {Code} with balance {Balance}", vault.Code, vault.CurrentBalance);

        return new CreateCashVaultResponse(vault.Id, vault.Code, vault.Name, vault.CurrentBalance);
    }
}
