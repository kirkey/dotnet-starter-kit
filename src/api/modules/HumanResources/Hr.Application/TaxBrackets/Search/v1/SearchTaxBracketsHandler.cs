namespace FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Search.v1;

using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Get.v1;
using Specifications;

/// <summary>
/// Handler for searching tax brackets with filters and pagination.
/// </summary>
public sealed class SearchTaxBracketsHandler(
    [FromKeyedServices("hr:taxbrackets")] IReadRepository<TaxBracket> repository)
    : IRequestHandler<SearchTaxBracketsRequest, PagedList<TaxBracketResponse>>
{
    public async Task<PagedList<TaxBracketResponse>> Handle(
        SearchTaxBracketsRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchTaxBracketsSpec(request);
        var brackets = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = brackets.Select(bracket => new TaxBracketResponse(
            bracket.Id,
            bracket.TaxType,
            bracket.Year,
            bracket.MinIncome,
            bracket.MaxIncome,
            bracket.Rate,
            bracket.FilingStatus,
            bracket.Description
        )).ToList();

        return new PagedList<TaxBracketResponse>(
            responses,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}

