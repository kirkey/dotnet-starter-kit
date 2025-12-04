using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Create.v1;

public sealed class CreateMfiConfigurationHandler(
    ILogger<CreateMfiConfigurationHandler> logger,
    [FromKeyedServices("microfinance:mficonfigurations")] IRepository<MfiConfiguration> repository)
    : IRequestHandler<CreateMfiConfigurationCommand, CreateMfiConfigurationResponse>
{
    public async Task<CreateMfiConfigurationResponse> Handle(CreateMfiConfigurationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var config = MfiConfiguration.Create(
            request.Key,
            request.Value,
            request.Category,
            request.DataType,
            request.Description,
            request.IsEditable,
            request.DefaultValue);

        await repository.AddAsync(config, cancellationToken);
        logger.LogInformation("MFI configuration {Key} created with ID {Id}", config.Key, config.Id);

        return new CreateMfiConfigurationResponse(config.Id, config.Key);
    }
}
