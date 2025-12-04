using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.OpenDay.v1;

public sealed class OpenDayHandler(
    ILogger<OpenDayHandler> logger,
    [FromKeyedServices("microfinance:cashvaults")] IRepository<CashVault> repository)
    : IRequestHandler<OpenDayCommand, OpenDayResponse>
{
    public async Task<OpenDayResponse> Handle(OpenDayCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var vault = await repository.FirstOrDefaultAsync(
            new CashVaultByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (vault is null)
            throw new NotFoundException($"Cash vault with ID {request.Id} not found.");

        vault.OpenDay(request.VerifiedBalance);

        await repository.UpdateAsync(vault, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Cash vault {VaultId} day opened with balance: {Balance}", vault.Id, request.VerifiedBalance);

        return new OpenDayResponse(vault.Id, vault.OpeningBalance);
    }
}
