using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Update.v1;

public sealed record UpdateMfiConfigurationCommand(Guid Id, string Value) : IRequest<UpdateMfiConfigurationResponse>;
