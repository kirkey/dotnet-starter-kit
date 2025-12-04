using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Get.v1;

public sealed class GetMfiConfigurationHandler(
    [FromKeyedServices("microfinance:mficonfigurations")] IReadRepository<MfiConfiguration> repository)
    : IRequestHandler<GetMfiConfigurationRequest, MfiConfigurationResponse>
{
    public async Task<MfiConfigurationResponse> Handle(GetMfiConfigurationRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var config = await repository.FirstOrDefaultAsync(new MfiConfigurationByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"MFI configuration with id {request.Id} not found");

        return new MfiConfigurationResponse(
            config.Id,
            config.Key,
            config.Value,
            config.Category,
            config.DataType,
            config.Description,
            config.IsEncrypted,
            config.IsEditable,
            config.RequiresRestart,
            config.DefaultValue,
            config.ValidationRules,
            config.BranchId,
            config.DisplayOrder,
            config.CreatedOn);
    }
}
