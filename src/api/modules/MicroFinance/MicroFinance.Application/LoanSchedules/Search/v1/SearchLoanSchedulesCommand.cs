using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Search.v1;

public class SearchLoanSchedulesCommand : PaginationFilter, IRequest<PagedList<LoanScheduleResponse>>
{
    public DefaultIdType? LoanId { get; set; }
    public bool? IsPaid { get; set; }
    public DateOnly? DueDateFrom { get; set; }
    public DateOnly? DueDateTo { get; set; }
}
