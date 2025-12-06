using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Confirm.v1;

public sealed record ConfirmAmlAlertCommand(
    Guid Id,
    Guid ResolvedById,
    string Notes) : IRequest<ConfirmAmlAlertResponse>;
