using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Search.v1;

public sealed class SearchCommunicationTemplatesHandler(
    [FromKeyedServices("microfinance:communicationtemplates")] IReadRepository<CommunicationTemplate> repository)
    : IRequestHandler<SearchCommunicationTemplatesCommand, PagedList<CommunicationTemplateSummaryResponse>>
{
    public async Task<PagedList<CommunicationTemplateSummaryResponse>> Handle(
        SearchCommunicationTemplatesCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCommunicationTemplatesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CommunicationTemplateSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
