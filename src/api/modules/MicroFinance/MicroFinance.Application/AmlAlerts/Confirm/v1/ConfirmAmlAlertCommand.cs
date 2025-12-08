using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Confirm.v1;

public sealed record ConfirmAmlAlertCommand(
    DefaultIdType Id,
    DefaultIdType ResolvedById,
    string Notes) : IRequest<ConfirmAmlAlertResponse>;
