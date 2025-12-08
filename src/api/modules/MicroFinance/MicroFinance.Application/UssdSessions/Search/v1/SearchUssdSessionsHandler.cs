// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/UssdSessions/Search/v1/SearchUssdSessionsHandler.cs
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Search.v1;

/// <summary>
/// Handler for searching USSD sessions.
/// </summary>
public sealed class SearchUssdSessionsHandler(
    [FromKeyedServices("microfinance:ussdsessions")] IReadRepository<UssdSession> repository)
    : IRequestHandler<SearchUssdSessionsCommand, PagedList<UssdSessionResponse>>
{
    public async Task<PagedList<UssdSessionResponse>> Handle(SearchUssdSessionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchUssdSessionsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<UssdSessionResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

