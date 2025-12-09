using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Update.v1;

/// <summary>
/// Handler for updating a pending fee waiver.
/// </summary>
public sealed class UpdateFeeWaiverHandler(
    [FromKeyedServices("microfinance:feewaivers")] IRepository<FeeWaiver> repository,
    ILogger<UpdateFeeWaiverHandler> logger)
    : IRequestHandler<UpdateFeeWaiverCommand, UpdateFeeWaiverResponse>
{
    public async Task<UpdateFeeWaiverResponse> Handle(UpdateFeeWaiverCommand request, CancellationToken cancellationToken)
    {
        var feeWaiver = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (feeWaiver is null)
        {
            throw new NotFoundException($"Fee waiver with ID {request.Id} not found.");
        }

        feeWaiver.Update(
            request.WaivedAmount,
            request.WaiverReason,
            request.Notes);

        await repository.UpdateAsync(feeWaiver, cancellationToken);

        logger.LogInformation("Fee waiver {FeeWaiverId} updated", request.Id);

        return new UpdateFeeWaiverResponse(feeWaiver.Id);
    }
}
