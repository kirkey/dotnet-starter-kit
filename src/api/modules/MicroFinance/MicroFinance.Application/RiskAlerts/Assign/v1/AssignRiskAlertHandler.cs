using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Assign.v1;

/// <summary>
/// Handler for assigning a risk alert.
/// </summary>
public sealed class AssignRiskAlertHandler(
    [FromKeyedServices("microfinance:riskalerts")] IRepository<RiskAlert> repository,
    ILogger<AssignRiskAlertHandler> logger)
    : IRequestHandler<AssignRiskAlertCommand, AssignRiskAlertResponse>
{
    public async Task<AssignRiskAlertResponse> Handle(AssignRiskAlertCommand request, CancellationToken cancellationToken)
    {
        var riskAlert = await repository.GetByIdAsync(request.RiskAlertId, cancellationToken);

        if (riskAlert is null)
        {
            throw new NotFoundException($"Risk alert with ID {request.RiskAlertId} not found.");
        }

        riskAlert.Assign(request.UserId);
        await repository.UpdateAsync(riskAlert, cancellationToken);

        logger.LogInformation("Risk alert {RiskAlertId} assigned to user {UserId}",
            request.RiskAlertId, request.UserId);

        return new AssignRiskAlertResponse(
            riskAlert.Id,
            riskAlert.AssignedToUserId!.Value,
            riskAlert.Status);
    }
}
