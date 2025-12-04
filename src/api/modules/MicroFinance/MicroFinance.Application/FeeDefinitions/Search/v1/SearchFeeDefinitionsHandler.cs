using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Search.v1;

public sealed class SearchFeeDefinitionsHandler(
    [FromKeyedServices("microfinance:feedefinitions")] IReadRepository<FeeDefinition> repository)
    : IRequestHandler<SearchFeeDefinitionsCommand, PagedList<FeeDefinitionResponse>>
{
    public async Task<PagedList<FeeDefinitionResponse>> Handle(SearchFeeDefinitionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchFeeDefinitionsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<FeeDefinitionResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
