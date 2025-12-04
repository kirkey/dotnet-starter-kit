using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.ClosePremature.v1;

/// <summary>
/// Handler for premature closure.
/// </summary>
public sealed class ClosePrematureFixedDepositHandler(
    IRepository<FixedDeposit> repository,
    ILogger<ClosePrematureFixedDepositHandler> logger)
    : IRequestHandler<ClosePrematureFixedDepositCommand, ClosePrematureFixedDepositResponse>
{
    public async Task<ClosePrematureFixedDepositResponse> Handle(ClosePrematureFixedDepositCommand request, CancellationToken cancellationToken)
    {
        var deposit = await repository.GetByIdAsync(request.DepositId, cancellationToken)
            ?? throw new Exception($"Fixed deposit with ID {request.DepositId} not found.");

        deposit.ClosePremature(request.Reason);

        await repository.UpdateAsync(deposit, cancellationToken);
        logger.LogInformation("Prematurely closed fixed deposit {DepositId}", request.DepositId);

        return new ClosePrematureFixedDepositResponse(
            deposit.Id,
            deposit.Status,
            deposit.ClosedDate,
            "Fixed deposit closed prematurely.");
    }
}
