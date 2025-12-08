using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Close.v1;

public sealed record CloseAmlAlertCommand(DefaultIdType Id) : IRequest<CloseAmlAlertResponse>;
