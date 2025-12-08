using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Assign.v1;

public sealed record AssignAmlAlertCommand(
    DefaultIdType Id,
    DefaultIdType AssignedToId) : IRequest<AssignAmlAlertResponse>;
