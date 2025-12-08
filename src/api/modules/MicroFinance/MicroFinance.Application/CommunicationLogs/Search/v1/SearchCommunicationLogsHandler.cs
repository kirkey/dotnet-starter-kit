using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Search.v1;

public sealed class SearchCommunicationLogsHandler(
    [FromKeyedServices("microfinance:communicationlogs")] IReadRepository<CommunicationLog> repository)
    : IRequestHandler<SearchCommunicationLogsCommand, PagedList<CommunicationLogSummaryResponse>>
{
    public async Task<PagedList<CommunicationLogSummaryResponse>> Handle(
        SearchCommunicationLogsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCommunicationLogsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CommunicationLogSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
