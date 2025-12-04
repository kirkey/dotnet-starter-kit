using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Resolve.v1;

/// <summary>
/// Handler for resolving a risk alert.
/// </summary>
public sealed class ResolveRiskAlertHandler(
    [FromKeyedServices("microfinance:riskalerts")] IRepository<RiskAlert> repository,
    ILogger<ResolveRiskAlertHandler> logger)
    : IRequestHandler<ResolveRiskAlertCommand, ResolveRiskAlertResponse>
{
    public async Task<ResolveRiskAlertResponse> Handle(ResolveRiskAlertCommand request, CancellationToken cancellationToken)
    {
        var riskAlert = await repository.GetByIdAsync(request.RiskAlertId, cancellationToken);

        if (riskAlert is null)
        {
            throw new NotFoundException($"Risk alert with ID {request.RiskAlertId} not found.");
        }

        riskAlert.Resolve(request.UserId, request.Resolution);
        await repository.UpdateAsync(riskAlert, cancellationToken);

        logger.LogInformation("Risk alert {RiskAlertId} resolved by user {UserId}",
            request.RiskAlertId, request.UserId);

        return new ResolveRiskAlertResponse(
            riskAlert.Id,
            riskAlert.Status,
            riskAlert.ResolvedAt!.Value);
    }
}
