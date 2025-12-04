using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Search.v1;

public sealed class SearchLoanCollateralsHandler(
    [FromKeyedServices("microfinance:loancollaterals")] IReadRepository<LoanCollateral> repository)
    : IRequestHandler<SearchLoanCollateralsCommand, PagedList<LoanCollateralResponse>>
{
    public async Task<PagedList<LoanCollateralResponse>> Handle(SearchLoanCollateralsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchLoanCollateralsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<LoanCollateralResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
