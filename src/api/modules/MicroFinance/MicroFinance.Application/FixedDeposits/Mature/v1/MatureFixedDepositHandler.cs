using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Mature.v1;

/// <summary>
/// Handler for maturing fixed deposit.
/// </summary>
public sealed class MatureFixedDepositHandler(
    [FromKeyedServices("microfinance:fixeddeposits")] IRepository<FixedDeposit> repository,
    ILogger<MatureFixedDepositHandler> logger)
    : IRequestHandler<MatureFixedDepositCommand, MatureFixedDepositResponse>
{
    public async Task<MatureFixedDepositResponse> Handle(MatureFixedDepositCommand request, CancellationToken cancellationToken)
    {
        var deposit = await repository.GetByIdAsync(request.DepositId, cancellationToken)
            ?? throw new NotFoundException($"Fixed deposit with ID {request.DepositId} not found.");

        deposit.Mature();

        await repository.UpdateAsync(deposit, cancellationToken);
        logger.LogInformation("Matured fixed deposit {DepositId}", request.DepositId);

        return new MatureFixedDepositResponse(
            deposit.Id,
            deposit.Status,
            deposit.ClosedDate,
            "Fixed deposit matured successfully.");
    }
}
