using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Search.v1;

public class SearchAgentBankingsCommand : PaginationFilter, IRequest<PagedList<AgentBankingResponse>>
{
    public string? Status { get; set; }
    public string? Tier { get; set; }
    public Guid? BranchId { get; set; }
    public string? AgentCode { get; set; }
    public string? BusinessName { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? IsKycVerified { get; set; }
}
