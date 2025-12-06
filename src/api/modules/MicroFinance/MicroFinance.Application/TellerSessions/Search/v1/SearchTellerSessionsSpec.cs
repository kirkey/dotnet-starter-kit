using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Search.v1;

public class SearchTellerSessionsSpec : EntitiesByPaginationFilterSpec<TellerSession, TellerSessionResponse>
{
    public SearchTellerSessionsSpec(SearchTellerSessionsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(s => s.SessionDate, !command.HasOrderBy())
            .ThenByDescending(s => s.StartTime, !command.HasOrderBy())
            .Where(s => s.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(s => s.TellerUserId == command.TellerId!.Value, command.TellerId.HasValue)
            .Where(s => s.BranchId == command.BranchId!.Value, command.BranchId.HasValue)
            .Where(s => s.SessionDate == command.SessionDate!.Value, command.SessionDate.HasValue)
            .Where(s => s.SessionDate >= command.SessionDateFrom!.Value, command.SessionDateFrom.HasValue)
            .Where(s => s.SessionDate <= command.SessionDateTo!.Value, command.SessionDateTo.HasValue);
}
