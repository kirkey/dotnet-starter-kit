using FSH.Starter.WebApi.HumanResources.Application.Shifts.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Search.v1;

public class SearchShiftsRequest : PaginationFilter, IRequest<PagedList<ShiftResponse>>
{
    public string? SearchString { get; set; }
    public bool? IsActive { get; set; }
}

