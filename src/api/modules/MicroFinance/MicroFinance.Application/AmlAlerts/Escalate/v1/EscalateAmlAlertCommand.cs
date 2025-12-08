using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Escalate.v1;

public sealed record EscalateAmlAlertCommand(
    DefaultIdType Id,
    string Reason) : IRequest<EscalateAmlAlertResponse>;
