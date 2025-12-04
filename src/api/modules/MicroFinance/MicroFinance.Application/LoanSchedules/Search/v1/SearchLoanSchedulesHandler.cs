using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Search.v1;

public sealed class SearchLoanSchedulesHandler(
    [FromKeyedServices("microfinance:loanschedules")] IReadRepository<LoanSchedule> repository)
    : IRequestHandler<SearchLoanSchedulesCommand, PagedList<LoanScheduleResponse>>
{
    public async Task<PagedList<LoanScheduleResponse>> Handle(SearchLoanSchedulesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchLoanSchedulesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<LoanScheduleResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
