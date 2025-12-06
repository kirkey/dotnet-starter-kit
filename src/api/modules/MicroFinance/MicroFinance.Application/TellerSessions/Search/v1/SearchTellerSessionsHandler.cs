using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Search.v1;

public sealed class SearchTellerSessionsHandler(
    [FromKeyedServices("microfinance:tellersessions")] IReadRepository<TellerSession> repository)
    : IRequestHandler<SearchTellerSessionsCommand, PagedList<TellerSessionResponse>>
{
    public async Task<PagedList<TellerSessionResponse>> Handle(SearchTellerSessionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchTellerSessionsSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<TellerSessionResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
