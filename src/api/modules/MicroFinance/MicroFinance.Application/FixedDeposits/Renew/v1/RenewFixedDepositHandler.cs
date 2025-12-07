using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Renew.v1;

/// <summary>
/// Handler for renewing fixed deposit.
/// </summary>
public sealed class RenewFixedDepositHandler(
    [FromKeyedServices("microfinance:fixeddeposits")] IRepository<FixedDeposit> repository,
    ILogger<RenewFixedDepositHandler> logger)
    : IRequestHandler<RenewFixedDepositCommand, RenewFixedDepositResponse>
{
    public async Task<RenewFixedDepositResponse> Handle(RenewFixedDepositCommand request, CancellationToken cancellationToken)
    {
        var deposit = await repository.GetByIdAsync(request.DepositId, cancellationToken)
            ?? throw new NotFoundException($"Fixed deposit with ID {request.DepositId} not found.");

        deposit.Renew(request.NewTermMonths, request.NewInterestRate);

        await repository.UpdateAsync(deposit, cancellationToken);
        logger.LogInformation("Renewed fixed deposit {DepositId}", request.DepositId);

        return new RenewFixedDepositResponse(
            deposit.Id,
            deposit.Status,
            deposit.TermMonths,
            deposit.InterestRate,
            deposit.MaturityDate,
            "Fixed deposit renewed successfully.");
    }
}
