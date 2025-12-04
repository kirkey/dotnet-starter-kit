using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Escalate.v1;

/// <summary>
/// Handler for escalating a risk alert.
/// </summary>
public sealed class EscalateRiskAlertHandler(
    [FromKeyedServices("microfinance:riskalerts")] IRepository<RiskAlert> repository,
    ILogger<EscalateRiskAlertHandler> logger)
    : IRequestHandler<EscalateRiskAlertCommand, EscalateRiskAlertResponse>
{
    public async Task<EscalateRiskAlertResponse> Handle(EscalateRiskAlertCommand request, CancellationToken cancellationToken)
    {
        var riskAlert = await repository.GetByIdAsync(request.RiskAlertId, cancellationToken);

        if (riskAlert is null)
        {
            throw new NotFoundException($"Risk alert with ID {request.RiskAlertId} not found.");
        }

        riskAlert.Escalate();
        await repository.UpdateAsync(riskAlert, cancellationToken);

        logger.LogInformation("Risk alert {RiskAlertId} escalated to level {Level} with severity {Severity}",
            request.RiskAlertId, riskAlert.EscalationLevel, riskAlert.Severity);

        return new EscalateRiskAlertResponse(
            riskAlert.Id,
            riskAlert.EscalationLevel,
            riskAlert.Severity);
    }
}
