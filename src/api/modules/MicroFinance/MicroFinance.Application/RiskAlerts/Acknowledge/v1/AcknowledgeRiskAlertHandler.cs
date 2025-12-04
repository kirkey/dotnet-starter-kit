using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Acknowledge.v1;

/// <summary>
/// Handler for acknowledging a risk alert.
/// </summary>
public sealed class AcknowledgeRiskAlertHandler(
    [FromKeyedServices("microfinance:riskalerts")] IRepository<RiskAlert> repository,
    ILogger<AcknowledgeRiskAlertHandler> logger)
    : IRequestHandler<AcknowledgeRiskAlertCommand, AcknowledgeRiskAlertResponse>
{
    public async Task<AcknowledgeRiskAlertResponse> Handle(AcknowledgeRiskAlertCommand request, CancellationToken cancellationToken)
    {
        var riskAlert = await repository.GetByIdAsync(request.RiskAlertId, cancellationToken);

        if (riskAlert is null)
        {
            throw new NotFoundException($"Risk alert with ID {request.RiskAlertId} not found.");
        }

        riskAlert.Acknowledge(request.UserId);
        await repository.UpdateAsync(riskAlert, cancellationToken);

        logger.LogInformation("Risk alert {RiskAlertId} acknowledged by user {UserId}",
            request.RiskAlertId, request.UserId);

        return new AcknowledgeRiskAlertResponse(
            riskAlert.Id,
            riskAlert.Status,
            riskAlert.AcknowledgedAt!.Value);
    }
}
