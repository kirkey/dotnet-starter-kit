using FSH.Starter.WebApi.HumanResources.Application.Holidays.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Holidays.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Search.v1;

public sealed class SearchHolidaysHandler(
    [FromKeyedServices("hr:holidays")] IReadRepository<Holiday> repository)
    : IRequestHandler<SearchHolidaysRequest, PagedList<HolidayResponse>>
{
    public async Task<PagedList<HolidayResponse>> Handle(
        SearchHolidaysRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchHolidaysSpec(request);

        var items = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        return new PagedList<HolidayResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

