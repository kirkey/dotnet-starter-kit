using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Search.v1;

public sealed class SearchMfiConfigurationsHandler(
    [FromKeyedServices("microfinance:mficonfigurations")] IReadRepository<MfiConfiguration> repository)
    : IRequestHandler<SearchMfiConfigurationsCommand, PagedList<MfiConfigurationSummaryResponse>>
{
    public async Task<PagedList<MfiConfigurationSummaryResponse>> Handle(
        SearchMfiConfigurationsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchMfiConfigurationsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<MfiConfigurationSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
