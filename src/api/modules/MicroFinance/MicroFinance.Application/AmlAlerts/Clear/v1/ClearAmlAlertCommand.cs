using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Clear.v1;

public sealed record ClearAmlAlertCommand(
    Guid Id,
    Guid ResolvedById,
    string Notes) : IRequest<ClearAmlAlertResponse>;
