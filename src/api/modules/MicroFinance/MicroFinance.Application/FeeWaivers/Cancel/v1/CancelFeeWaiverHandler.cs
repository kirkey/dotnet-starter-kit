using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Cancel.v1;

/// <summary>
/// Handler for cancelling a pending fee waiver.
/// </summary>
public sealed class CancelFeeWaiverHandler(
    [FromKeyedServices("microfinance:feewaivers")] IRepository<FeeWaiver> repository,
    ILogger<CancelFeeWaiverHandler> logger)
    : IRequestHandler<CancelFeeWaiverCommand, CancelFeeWaiverResponse>
{
    public async Task<CancelFeeWaiverResponse> Handle(CancelFeeWaiverCommand request, CancellationToken cancellationToken)
    {
        var feeWaiver = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (feeWaiver is null)
        {
            throw new NotFoundException($"Fee waiver with ID {request.Id} not found.");
        }

        feeWaiver.Cancel();

        await repository.UpdateAsync(feeWaiver, cancellationToken);

        logger.LogInformation("Fee waiver {FeeWaiverId} cancelled", request.Id);

        return new CancelFeeWaiverResponse(feeWaiver.Id, feeWaiver.Status);
    }
}
