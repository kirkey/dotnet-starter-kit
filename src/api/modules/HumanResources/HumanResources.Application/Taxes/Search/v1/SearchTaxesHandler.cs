using FSH.Starter.WebApi.HumanResources.Application.Taxes.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Taxes.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Search.v1;

/// <summary>
/// Handler for searching tax brackets.
/// </summary>
public sealed class SearchTaxesHandler(
    [FromKeyedServices("hr:taxes")] IReadRepository<TaxBracket> repository)
    : IRequestHandler<SearchTaxesRequest, PagedList<TaxResponse>>
{
    public async Task<PagedList<TaxResponse>> Handle(
        SearchTaxesRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchTaxesSpec(request);
        var taxes = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = taxes.Select(MapToResponse).ToList();

        return new PagedList<TaxResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }

    private static TaxResponse MapToResponse(TaxBracket tax)
    {
        return new TaxResponse
        {
            Id = tax.Id,
            TaxType = tax.TaxType,
            Year = tax.Year,
            MinIncome = tax.MinIncome,
            MaxIncome = tax.MaxIncome,
            Rate = tax.Rate,
            FilingStatus = tax.FilingStatus,
            Description = tax.Description
        };
    }
}

