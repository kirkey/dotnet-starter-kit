using FSH.Starter.WebApi.HumanResources.Application.Shifts.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Shifts.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Specifications;

public class SearchShiftsSpec : EntitiesByPaginationFilterSpec<Shift, ShiftResponse>
{
    public SearchShiftsSpec(SearchShiftsRequest request)
        : base(request) =>
        Query
            .Where(s => s.ShiftName.Contains(request.SearchString) || s.Description!.Contains(request.SearchString), !string.IsNullOrWhiteSpace(request.SearchString))
            .Where(s => s.IsActive == request.IsActive, request.IsActive.HasValue)
            .OrderBy(s => s.StartTime, !request.HasOrderBy());
}

