using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Create.v1;

public sealed record CreateAmlAlertCommand(
    string AlertCode,
    string AlertType,
    string Severity,
    string TriggerRule,
    string Description,
    Guid? MemberId = null,
    Guid? TransactionId = null,
    decimal? TransactionAmount = null) : IRequest<CreateAmlAlertResponse>;
