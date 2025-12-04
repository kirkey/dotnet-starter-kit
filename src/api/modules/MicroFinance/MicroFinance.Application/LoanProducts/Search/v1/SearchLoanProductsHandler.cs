using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Search.v1;

public sealed class SearchLoanProductsHandler(
    [FromKeyedServices("microfinance:loanproducts")] IReadRepository<LoanProduct> repository)
    : IRequestHandler<SearchLoanProductsCommand, PagedList<LoanProductResponse>>
{
    public async Task<PagedList<LoanProductResponse>> Handle(SearchLoanProductsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchLoanProductsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<LoanProductResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
