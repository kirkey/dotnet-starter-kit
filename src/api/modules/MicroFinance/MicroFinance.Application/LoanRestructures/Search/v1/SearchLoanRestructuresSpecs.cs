using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Search.v1;

/// <summary>
/// Specification for searching loan restructures.
/// </summary>
public class SearchLoanRestructuresSpecs : EntitiesByPaginationFilterSpec<LoanRestructure, LoanRestructureSummaryResponse>
{
    public SearchLoanRestructuresSpecs(SearchLoanRestructuresCommand command)
        : base(command) =>
        Query
            .OrderByDescending(r => r.RequestDate, !command.HasOrderBy())
            .Where(r => r.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(r => r.RestructureNumber == command.RestructureNumber, !string.IsNullOrWhiteSpace(command.RestructureNumber))
            .Where(r => r.RestructureType == command.RestructureType, !string.IsNullOrWhiteSpace(command.RestructureType))
            .Where(r => r.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(r => r.RequestDate >= command.RequestDateFrom!.Value, command.RequestDateFrom.HasValue)
            .Where(r => r.RequestDate <= command.RequestDateTo!.Value, command.RequestDateTo.HasValue)
            .Where(r => r.EffectiveDate >= command.EffectiveDateFrom!.Value, command.EffectiveDateFrom.HasValue)
            .Where(r => r.EffectiveDate <= command.EffectiveDateTo!.Value, command.EffectiveDateTo.HasValue);
}
