using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Search.v1;

public class SearchAgentBankingsSpec : EntitiesByPaginationFilterSpec<AgentBanking, AgentBankingResponse>
{
    public SearchAgentBankingsSpec(SearchAgentBankingsCommand command)
        : base(command) =>
        Query
            .OrderBy(a => a.BusinessName, !command.HasOrderBy())
            .Where(a => a.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(a => a.Tier == command.Tier, !string.IsNullOrWhiteSpace(command.Tier))
            .Where(a => a.BranchId == command.BranchId!.Value, command.BranchId.HasValue)
            .Where(a => a.AgentCode.Contains(command.AgentCode!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.AgentCode))
            .Where(a => a.BusinessName.Contains(command.BusinessName!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.BusinessName))
            .Where(a => a.PhoneNumber.Contains(command.PhoneNumber!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.PhoneNumber))
            .Where(a => a.IsKycVerified == command.IsKycVerified!.Value, command.IsKycVerified.HasValue);
}
