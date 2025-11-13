using FSH.Starter.WebApi.HumanResources.Application.Shifts.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Shifts.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Search.v1;

public sealed class SearchShiftsHandler(
    [FromKeyedServices("hr:shifts")] IReadRepository<Shift> repository)
    : IRequestHandler<SearchShiftsRequest, PagedList<ShiftResponse>>
{
    public async Task<PagedList<ShiftResponse>> Handle(
        SearchShiftsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchShiftsSpec(request);

        var items = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        return new PagedList<ShiftResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

