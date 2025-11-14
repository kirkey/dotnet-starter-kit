namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Search.v1;

using FSH.Starter.WebApi.HumanResources.Application.Taxes.Specifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Handler for searching tax brackets.
/// </summary>
public sealed class SearchTaxBracketsHandler(
    [FromKeyedServices("hr:taxbrackets")] IReadRepository<TaxBracket> repository)
    : IRequestHandler<SearchTaxBracketsRequest, PagedList<TaxBracketDto>>
{
    public async Task<PagedList<TaxBracketDto>> Handle(
        SearchTaxBracketsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchTaxBracketsSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var dtos = items.Select(t => new TaxBracketDto(
            t.Id,
            t.TaxType,
            t.Year,
            t.MinIncome,
            t.MaxIncome,
            t.Rate,
            t.FilingStatus)).ToList();

        return new PagedList<TaxBracketDto>(
            dtos,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}

