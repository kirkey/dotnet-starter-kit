using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Search.v1;

public sealed class SearchLoanGuarantorsHandler(
    [FromKeyedServices("microfinance:loanguarantors")] IReadRepository<LoanGuarantor> repository)
    : IRequestHandler<SearchLoanGuarantorsCommand, PagedList<LoanGuarantorResponse>>
{
    public async Task<PagedList<LoanGuarantorResponse>> Handle(SearchLoanGuarantorsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchLoanGuarantorsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<LoanGuarantorResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
