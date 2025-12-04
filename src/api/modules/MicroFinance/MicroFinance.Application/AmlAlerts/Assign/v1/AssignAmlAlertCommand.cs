using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Assign.v1;

public sealed record AssignAmlAlertCommand(
    Guid Id,
    Guid AssignedToId) : IRequest<AssignAmlAlertResponse>;
