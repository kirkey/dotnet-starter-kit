using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Search.v1;

public class SearchCustomerCasesSpecs : EntitiesByPaginationFilterSpec<CustomerCase, CustomerCaseSummaryResponse>
{
    public SearchCustomerCasesSpecs(SearchCustomerCasesCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.OpenedAt, !command.HasOrderBy())
            .Where(x => x.CaseNumber == command.CaseNumber, !string.IsNullOrWhiteSpace(command.CaseNumber))
            .Where(x => x.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(x => x.Category == command.Category, !string.IsNullOrWhiteSpace(command.Category))
            .Where(x => x.Priority == command.Priority, !string.IsNullOrWhiteSpace(command.Priority))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.Channel == command.Channel, !string.IsNullOrWhiteSpace(command.Channel))
            .Where(x => x.AssignedToId == command.AssignedToId!.Value, command.AssignedToId.HasValue)
            .Where(x => x.OpenedAt >= command.OpenedFrom!.Value.ToDateTime(TimeOnly.MinValue), command.OpenedFrom.HasValue)
            .Where(x => x.OpenedAt <= command.OpenedTo!.Value.ToDateTime(TimeOnly.MaxValue), command.OpenedTo.HasValue)
            .Where(x => x.SlaBreached == command.SlaBreached!.Value, command.SlaBreached.HasValue);
}
