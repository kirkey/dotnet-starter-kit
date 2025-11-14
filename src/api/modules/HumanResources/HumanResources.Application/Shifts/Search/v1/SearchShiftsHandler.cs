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

        var responses = items.Select(shift => new ShiftResponse
        {
            Id = shift.Id,
            ShiftName = shift.ShiftName,
            StartTime = shift.StartTime,
            EndTime = shift.EndTime,
            IsOvernight = shift.IsOvernight,
            BreakDurationMinutes = shift.BreakDurationMinutes,
            WorkingHours = shift.WorkingHours,
            Description = shift.Description,
            IsActive = shift.IsActive
        }).ToList();

        return new PagedList<ShiftResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}

