using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.TransferToVault.v1;

/// <summary>
/// Handler for transferring cash between vaults.
/// </summary>
public sealed class TransferToVaultHandler(
    [FromKeyedServices("microfinance:cashvaults")] IRepository<CashVault> repository,
    ILogger<TransferToVaultHandler> logger)
    : IRequestHandler<TransferToVaultCommand, TransferToVaultResponse>
{
    public async Task<TransferToVaultResponse> Handle(TransferToVaultCommand request, CancellationToken cancellationToken)
    {
        var sourceVault = await repository.GetByIdAsync(request.SourceVaultId, cancellationToken)
            ?? throw new NotFoundException($"Source cash vault with ID {request.SourceVaultId} not found.");

        var targetVault = await repository.GetByIdAsync(request.TargetVaultId, cancellationToken)
            ?? throw new NotFoundException($"Target cash vault with ID {request.TargetVaultId} not found.");

        sourceVault.TransferTo(targetVault, request.Amount, request.DenominationBreakdown);

        await repository.UpdateAsync(sourceVault, cancellationToken);
        await repository.UpdateAsync(targetVault, cancellationToken);
        
        logger.LogInformation("Transferred {Amount} from vault {SourceVaultId} to vault {TargetVaultId}", 
            request.Amount, request.SourceVaultId, request.TargetVaultId);

        return new TransferToVaultResponse(
            sourceVault.Id, 
            targetVault.Id, 
            request.Amount,
            sourceVault.CurrentBalance,
            targetVault.CurrentBalance,
            "Cash transferred between vaults successfully.");
    }
}
