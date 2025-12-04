using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Get.v1;

public sealed record GetAmlAlertRequest(Guid Id) : IRequest<AmlAlertResponse>;
