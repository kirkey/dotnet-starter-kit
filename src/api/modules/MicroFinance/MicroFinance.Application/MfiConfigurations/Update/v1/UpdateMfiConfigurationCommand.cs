using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Update.v1;

public sealed record UpdateMfiConfigurationCommand(DefaultIdType Id, string Value) : IRequest<UpdateMfiConfigurationResponse>;
