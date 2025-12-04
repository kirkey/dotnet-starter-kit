using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Escalate.v1;

public sealed record EscalateAmlAlertCommand(
    Guid Id,
    string Reason) : IRequest<EscalateAmlAlertResponse>;
