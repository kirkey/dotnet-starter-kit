using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Create.v1;

public sealed class CreateAmlAlertHandler(
    [FromKeyedServices("microfinance:amlalerts")] IRepository<AmlAlert> repository,
    ILogger<CreateAmlAlertHandler> logger)
    : IRequestHandler<CreateAmlAlertCommand, CreateAmlAlertResponse>
{
    public async Task<CreateAmlAlertResponse> Handle(CreateAmlAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = AmlAlert.Create(
            request.AlertCode,
            request.AlertType,
            request.Severity,
            request.TriggerRule,
            request.Description);

        if (request.MemberId.HasValue)
            alert.ForMember(request.MemberId.Value);

        if (request.TransactionId.HasValue && request.TransactionAmount.HasValue)
            alert.ForTransaction(request.TransactionId.Value, request.TransactionAmount.Value);

        await repository.AddAsync(alert, cancellationToken);
        logger.LogInformation("AML alert {AlertCode} created with ID {Id}", alert.AlertCode, alert.Id);

        return new CreateAmlAlertResponse(alert.Id, alert.AlertCode, alert.AlertType, alert.Severity, alert.Status);
    }
}
