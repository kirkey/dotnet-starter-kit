using Accounting.Application.TaxCodes.Responses;

namespace Accounting.Application.TaxCodes.Search.v1;

public sealed class SearchTaxCodesHandler(
    IReadRepository<TaxCode> repository)
    : IRequestHandler<SearchTaxCodesCommand, PagedList<TaxCodeResponse>>
{
    public async Task<PagedList<TaxCodeResponse>> Handle(SearchTaxCodesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchTaxCodesSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        return new PagedList<TaxCodeResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
