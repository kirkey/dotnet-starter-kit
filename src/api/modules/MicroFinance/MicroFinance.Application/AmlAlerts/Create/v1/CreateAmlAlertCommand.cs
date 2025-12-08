using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Create.v1;

public sealed record CreateAmlAlertCommand(
    string AlertCode,
    string AlertType,
    string Severity,
    string TriggerRule,
    string Description,
    DefaultIdType? MemberId = null,
    DefaultIdType? TransactionId = null,
    decimal? TransactionAmount = null) : IRequest<CreateAmlAlertResponse>;
