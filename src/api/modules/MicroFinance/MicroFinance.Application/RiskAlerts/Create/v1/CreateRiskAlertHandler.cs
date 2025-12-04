using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Create.v1;

/// <summary>
/// Handler for creating a new risk alert.
/// </summary>
public sealed class CreateRiskAlertHandler(
    [FromKeyedServices("microfinance:riskalerts")] IRepository<RiskAlert> repository,
    ILogger<CreateRiskAlertHandler> logger)
    : IRequestHandler<CreateRiskAlertCommand, CreateRiskAlertResponse>
{
    public async Task<CreateRiskAlertResponse> Handle(CreateRiskAlertCommand request, CancellationToken cancellationToken)
    {
        var riskAlert = RiskAlert.Create(
            request.AlertNumber,
            request.Title,
            request.Severity,
            request.Source,
            request.RiskCategoryId,
            request.RiskIndicatorId,
            request.Description,
            request.ThresholdValue,
            request.ActualValue,
            request.BranchId,
            request.LoanId,
            request.MemberId,
            request.DueDate);

        await repository.AddAsync(riskAlert, cancellationToken);

        logger.LogInformation("Risk alert {AlertNumber} created - Title: {Title}, Severity: {Severity}",
            request.AlertNumber, request.Title, request.Severity);

        return new CreateRiskAlertResponse(
            riskAlert.Id,
            riskAlert.AlertNumber,
            riskAlert.Title,
            riskAlert.Severity,
            riskAlert.Status);
    }
}
