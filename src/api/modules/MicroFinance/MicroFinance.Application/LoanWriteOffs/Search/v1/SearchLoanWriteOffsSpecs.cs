using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Search.v1;

/// <summary>
/// Specification for searching loan write-offs.
/// </summary>
public class SearchLoanWriteOffsSpecs : EntitiesByPaginationFilterSpec<LoanWriteOff, LoanWriteOffSummaryResponse>
{
    public SearchLoanWriteOffsSpecs(SearchLoanWriteOffsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(w => w.RequestDate, !command.HasOrderBy())
            .Where(w => w.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(w => w.WriteOffNumber == command.WriteOffNumber, !string.IsNullOrWhiteSpace(command.WriteOffNumber))
            .Where(w => w.WriteOffType == command.WriteOffType, !string.IsNullOrWhiteSpace(command.WriteOffType))
            .Where(w => w.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(w => w.RequestDate >= command.RequestDateFrom!.Value, command.RequestDateFrom.HasValue)
            .Where(w => w.RequestDate <= command.RequestDateTo!.Value, command.RequestDateTo.HasValue)
            .Where(w => w.WriteOffDate >= command.WriteOffDateFrom!.Value, command.WriteOffDateFrom.HasValue)
            .Where(w => w.WriteOffDate <= command.WriteOffDateTo!.Value, command.WriteOffDateTo.HasValue)
            .Where(w => w.TotalWriteOff >= command.MinTotalWriteOff!.Value, command.MinTotalWriteOff.HasValue)
            .Where(w => w.TotalWriteOff <= command.MaxTotalWriteOff!.Value, command.MaxTotalWriteOff.HasValue);
}
