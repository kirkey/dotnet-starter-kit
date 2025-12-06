using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Search.v1;

public class SearchTellerSessionsCommand : PaginationFilter, IRequest<PagedList<TellerSessionResponse>>
{
    public string? Status { get; set; }
    public Guid? TellerId { get; set; }
    public Guid? BranchId { get; set; }
    public DateOnly? SessionDate { get; set; }
    public DateOnly? SessionDateFrom { get; set; }
    public DateOnly? SessionDateTo { get; set; }
}
