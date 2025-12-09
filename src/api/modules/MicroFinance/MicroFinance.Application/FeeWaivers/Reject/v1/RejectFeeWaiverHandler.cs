using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Reject.v1;

/// <summary>
/// Handler for rejecting a fee waiver.
/// </summary>
public sealed class RejectFeeWaiverHandler(
    [FromKeyedServices("microfinance:feewaivers")] IRepository<FeeWaiver> repository,
    ILogger<RejectFeeWaiverHandler> logger)
    : IRequestHandler<RejectFeeWaiverCommand, RejectFeeWaiverResponse>
{
    public async Task<RejectFeeWaiverResponse> Handle(RejectFeeWaiverCommand request, CancellationToken cancellationToken)
    {
        var feeWaiver = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (feeWaiver is null)
        {
            throw new NotFoundException($"Fee waiver with ID {request.Id} not found.");
        }

        feeWaiver.Reject(request.RejectedByUserId, request.RejectorName, request.Reason);

        await repository.UpdateAsync(feeWaiver, cancellationToken);

        logger.LogInformation("Fee waiver {FeeWaiverId} rejected by {RejectorName}. Reason: {Reason}",
            request.Id, request.RejectorName, request.Reason);

        return new RejectFeeWaiverResponse(feeWaiver.Id, feeWaiver.Status);
    }
}
