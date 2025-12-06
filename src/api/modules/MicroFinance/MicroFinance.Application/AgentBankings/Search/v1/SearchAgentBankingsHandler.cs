using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Search.v1;

public sealed class SearchAgentBankingsHandler(
    [FromKeyedServices("microfinance:agentbankings")] IReadRepository<AgentBanking> repository)
    : IRequestHandler<SearchAgentBankingsCommand, PagedList<AgentBankingResponse>>
{
    public async Task<PagedList<AgentBankingResponse>> Handle(SearchAgentBankingsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchAgentBankingsSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<AgentBankingResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
