using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.CloseDay.v1;

/// <summary>
/// Handler for closing a cash vault for the day.
/// </summary>
public sealed class CloseDayCashVaultHandler(
    [FromKeyedServices("microfinance:cashvaults")] IRepository<CashVault> repository,
    ILogger<CloseDayCashVaultHandler> logger)
    : IRequestHandler<CloseDayCashVaultCommand, CloseDayCashVaultResponse>
{
    public async Task<CloseDayCashVaultResponse> Handle(CloseDayCashVaultCommand request, CancellationToken cancellationToken)
    {
        var vault = await repository.GetByIdAsync(request.CashVaultId, cancellationToken)
            ?? throw new NotFoundException($"Cash vault with ID {request.CashVaultId} not found.");

        vault.CloseDay(request.VerifiedBalance, request.DenominationBreakdown);

        await repository.UpdateAsync(vault, cancellationToken);
        logger.LogInformation("Closed cash vault {CashVaultId} for the day with verified balance {VerifiedBalance}", 
            request.CashVaultId, request.VerifiedBalance);

        return new CloseDayCashVaultResponse(vault.Id, request.VerifiedBalance, "Cash vault closed for the day successfully.");
    }
}
