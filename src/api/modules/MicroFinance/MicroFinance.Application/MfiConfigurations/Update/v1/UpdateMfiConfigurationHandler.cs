using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Update.v1;

public sealed class UpdateMfiConfigurationHandler(
    ILogger<UpdateMfiConfigurationHandler> logger,
    [FromKeyedServices("microfinance:mficonfigurations")] IRepository<MfiConfiguration> repository)
    : IRequestHandler<UpdateMfiConfigurationCommand, UpdateMfiConfigurationResponse>
{
    public async Task<UpdateMfiConfigurationResponse> Handle(UpdateMfiConfigurationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var config = await repository.FirstOrDefaultAsync(new MfiConfigurationByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"MFI configuration with id {request.Id} not found");

        config.UpdateValue(request.Value);
        await repository.UpdateAsync(config, cancellationToken);

        logger.LogInformation("MFI configuration {Key} updated", config.Key);
        return new UpdateMfiConfigurationResponse(config.Id, config.Key, config.Value);
    }
}
