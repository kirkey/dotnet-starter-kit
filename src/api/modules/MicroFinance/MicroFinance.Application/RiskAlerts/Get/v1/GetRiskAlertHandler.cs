using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Get.v1;

/// <summary>
/// Handler for getting a risk alert by ID.
/// </summary>
public sealed class GetRiskAlertHandler(
    [FromKeyedServices("microfinance:riskalerts")] IReadRepository<RiskAlert> repository,
    ILogger<GetRiskAlertHandler> logger)
    : IRequestHandler<GetRiskAlertRequest, RiskAlertResponse>
{
    public async Task<RiskAlertResponse> Handle(GetRiskAlertRequest request, CancellationToken cancellationToken)
    {
        var spec = new RiskAlertByIdSpec(request.Id);
        var riskAlert = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (riskAlert is null)
        {
            throw new NotFoundException($"Risk alert with ID {request.Id} not found.");
        }

        logger.LogInformation("Retrieved risk alert {RiskAlertId}", request.Id);

        return riskAlert;
    }
}
