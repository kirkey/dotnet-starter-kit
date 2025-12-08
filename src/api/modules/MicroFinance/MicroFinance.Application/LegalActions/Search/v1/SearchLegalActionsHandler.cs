// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/LegalActions/Search/v1/SearchLegalActionsHandler.cs
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Search.v1;

/// <summary>
/// Handler for searching legal actions.
/// </summary>
public sealed class SearchLegalActionsHandler(
    [FromKeyedServices("microfinance:legalactions")] IReadRepository<LegalAction> repository)
    : IRequestHandler<SearchLegalActionsCommand, PagedList<LegalActionResponse>>
{
    public async Task<PagedList<LegalActionResponse>> Handle(SearchLegalActionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchLegalActionsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<LegalActionResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

