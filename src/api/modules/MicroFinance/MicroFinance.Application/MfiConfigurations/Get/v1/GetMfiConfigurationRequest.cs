using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Get.v1;

public sealed record GetMfiConfigurationRequest(Guid Id) : IRequest<MfiConfigurationResponse>;
