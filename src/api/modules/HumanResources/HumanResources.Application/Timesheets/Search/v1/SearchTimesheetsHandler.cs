using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Search.v1;

public sealed class SearchTimesheetsHandler(
    [FromKeyedServices("hr:timesheets")] IReadRepository<Domain.Entities.Timesheet> repository)
    : IRequestHandler<SearchTimesheetsRequest, PagedList<TimesheetResponse>>
{
    public async Task<PagedList<TimesheetResponse>> Handle(
        SearchTimesheetsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchTimesheetsSpec(request);

        var items = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        return new PagedList<TimesheetResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

