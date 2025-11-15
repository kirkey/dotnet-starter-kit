using FSH.Starter.WebApi.HumanResources.Application.Holidays.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Holidays.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Specifications;

public class HolidayByIdSpec : Specification<Holiday>
{
    public HolidayByIdSpec(DefaultIdType id)
    {
        Query.Where(h => h.Id == id);
    }
}

public class SearchHolidaysSpec : EntitiesByPaginationFilterSpec<Holiday, HolidayResponse>
{
    public SearchHolidaysSpec(SearchHolidaysRequest request)
        : base(request) =>
        Query
            .Where(h => h.HolidayName.Contains(request.SearchString!), !string.IsNullOrWhiteSpace(request.SearchString))
            .Where(h => h.HolidayDate >= request.StartDate && h.HolidayDate <= request.EndDate, 
                   request is { StartDate: not null, EndDate: not null })
            .Where(h => h.IsPaid == request.IsPaid, request.IsPaid.HasValue)
            .Where(h => h.IsActive == request.IsActive, request.IsActive.HasValue)
            .OrderByDescending(h => h.HolidayDate, !request.HasOrderBy());
}

