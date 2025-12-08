using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Clear.v1;

public sealed record ClearAmlAlertCommand(
    DefaultIdType Id,
    DefaultIdType ResolvedById,
    string Notes) : IRequest<ClearAmlAlertResponse>;
