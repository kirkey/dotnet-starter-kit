using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Waive.v1;

/// <summary>
/// Handler for waiving fee charge.
/// </summary>
public sealed class WaiveFeeChargeHandler(
    [FromKeyedServices("microfinance:feecharges")] IRepository<FeeCharge> repository,
    ILogger<WaiveFeeChargeHandler> logger)
    : IRequestHandler<WaiveFeeChargeCommand, WaiveFeeChargeResponse>
{
    public async Task<WaiveFeeChargeResponse> Handle(WaiveFeeChargeCommand request, CancellationToken cancellationToken)
    {
        var feeCharge = await repository.GetByIdAsync(request.FeeChargeId, cancellationToken)
            ?? throw new NotFoundException($"Fee charge with ID {request.FeeChargeId} not found.");

        feeCharge.Waive(request.Reason);

        await repository.UpdateAsync(feeCharge, cancellationToken);
        logger.LogInformation("Waived fee charge {FeeChargeId}", request.FeeChargeId);

        return new WaiveFeeChargeResponse(feeCharge.Id, feeCharge.Status, "Fee charge waived successfully.");
    }
}
