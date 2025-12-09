using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Get.v1;

/// <summary>
/// Handler for getting an interest rate change by ID.
/// </summary>
public sealed class GetInterestRateChangeHandler(
    [FromKeyedServices("microfinance:interestratechanges")] IReadRepository<InterestRateChange> repository,
    ILogger<GetInterestRateChangeHandler> logger)
    : IRequestHandler<GetInterestRateChangeRequest, InterestRateChangeResponse>
{
    public async Task<InterestRateChangeResponse> Handle(GetInterestRateChangeRequest request, CancellationToken cancellationToken)
    {
        var spec = new InterestRateChangeByIdSpec(request.Id);
        var rateChange = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (rateChange is null)
        {
            throw new NotFoundException($"Interest rate change with ID {request.Id} not found.");
        }

        logger.LogInformation("Retrieved interest rate change {InterestRateChangeId}", request.Id);

        return rateChange;
    }
}
