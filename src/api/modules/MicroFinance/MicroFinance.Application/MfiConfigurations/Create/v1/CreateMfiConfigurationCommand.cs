using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Create.v1;

public sealed record CreateMfiConfigurationCommand(
    string Key,
    string Value,
    string Category,
    string DataType = "String",
    string? Description = null,
    bool IsEditable = true,
    string? DefaultValue = null) : IRequest<CreateMfiConfigurationResponse>;
