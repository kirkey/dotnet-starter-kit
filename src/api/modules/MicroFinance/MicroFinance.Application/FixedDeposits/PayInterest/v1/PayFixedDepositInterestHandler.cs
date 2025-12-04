using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.PayInterest.v1;

/// <summary>
/// Handler for paying interest from fixed deposit.
/// </summary>
public sealed class PayFixedDepositInterestHandler(
    IRepository<FixedDeposit> repository,
    ILogger<PayFixedDepositInterestHandler> logger)
    : IRequestHandler<PayFixedDepositInterestCommand, PayFixedDepositInterestResponse>
{
    public async Task<PayFixedDepositInterestResponse> Handle(PayFixedDepositInterestCommand request, CancellationToken cancellationToken)
    {
        var deposit = await repository.GetByIdAsync(request.DepositId, cancellationToken)
            ?? throw new Exception($"Fixed deposit with ID {request.DepositId} not found.");

        deposit.PayInterest(request.Amount);

        await repository.UpdateAsync(deposit, cancellationToken);
        logger.LogInformation("Paid interest {Amount} from fixed deposit {DepositId}", request.Amount, request.DepositId);

        return new PayFixedDepositInterestResponse(
            deposit.Id,
            request.Amount,
            deposit.InterestPaid,
            "Interest paid successfully.");
    }
}
