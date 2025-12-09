using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Get.v1;

/// <summary>
/// Handler for getting a fee waiver by ID.
/// </summary>
public sealed class GetFeeWaiverHandler(
    [FromKeyedServices("microfinance:feewaivers")] IReadRepository<FeeWaiver> repository,
    ILogger<GetFeeWaiverHandler> logger)
    : IRequestHandler<GetFeeWaiverRequest, FeeWaiverResponse>
{
    public async Task<FeeWaiverResponse> Handle(GetFeeWaiverRequest request, CancellationToken cancellationToken)
    {
        var spec = new FeeWaiverByIdSpec(request.Id);
        var feeWaiver = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (feeWaiver is null)
        {
            throw new NotFoundException($"Fee waiver with ID {request.Id} not found.");
        }

        logger.LogInformation("Retrieved fee waiver {FeeWaiverId}", request.Id);

        return feeWaiver;
    }
}
