using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Reverse.v1;

/// <summary>
/// Handler for reversing fee charge.
/// </summary>
public sealed class ReverseFeeChargeHandler(
    IRepository<FeeCharge> repository,
    ILogger<ReverseFeeChargeHandler> logger)
    : IRequestHandler<ReverseFeeChargeCommand, ReverseFeeChargeResponse>
{
    public async Task<ReverseFeeChargeResponse> Handle(ReverseFeeChargeCommand request, CancellationToken cancellationToken)
    {
        var feeCharge = await repository.GetByIdAsync(request.FeeChargeId, cancellationToken)
            ?? throw new Exception($"Fee charge with ID {request.FeeChargeId} not found.");

        feeCharge.Reverse(request.Reason);

        await repository.UpdateAsync(feeCharge, cancellationToken);
        logger.LogInformation("Reversed fee charge {FeeChargeId}", request.FeeChargeId);

        return new ReverseFeeChargeResponse(feeCharge.Id, feeCharge.Status, "Fee charge reversed successfully.");
    }
}
