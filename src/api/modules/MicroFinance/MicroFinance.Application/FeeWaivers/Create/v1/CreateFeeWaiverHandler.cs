using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Create.v1;

/// <summary>
/// Handler for creating a new fee waiver request.
/// </summary>
public sealed class CreateFeeWaiverHandler(
    [FromKeyedServices("microfinance:feewaivers")] IRepository<FeeWaiver> repository,
    ILogger<CreateFeeWaiverHandler> logger)
    : IRequestHandler<CreateFeeWaiverCommand, CreateFeeWaiverResponse>
{
    public async Task<CreateFeeWaiverResponse> Handle(CreateFeeWaiverCommand request, CancellationToken cancellationToken)
    {
        var feeWaiver = FeeWaiver.Create(
            request.FeeChargeId,
            request.Reference,
            request.OriginalAmount,
            request.WaivedAmount,
            request.WaiverReason,
            request.RequestDate,
            request.Notes);

        await repository.AddAsync(feeWaiver, cancellationToken);

        logger.LogInformation("Fee waiver {Reference} created for fee charge {FeeChargeId}, waived amount: {WaivedAmount}",
            request.Reference, request.FeeChargeId, request.WaivedAmount);

        return new CreateFeeWaiverResponse(
            feeWaiver.Id,
            feeWaiver.Reference,
            feeWaiver.WaiverType,
            feeWaiver.WaivedAmount,
            feeWaiver.Status);
    }
}
