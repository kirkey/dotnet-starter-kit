using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Search.v1;

public class SearchLoanSchedulesSpecs : EntitiesByPaginationFilterSpec<LoanSchedule, LoanScheduleResponse>
{
    public SearchLoanSchedulesSpecs(SearchLoanSchedulesCommand command)
        : base(command) =>
        Query
            .OrderBy(ls => ls.DueDate, !command.HasOrderBy())
            .ThenBy(ls => ls.InstallmentNumber)
            .Where(ls => ls.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(ls => ls.IsPaid == command.IsPaid!.Value, command.IsPaid.HasValue)
            .Where(ls => ls.DueDate >= command.DueDateFrom!.Value, command.DueDateFrom.HasValue)
            .Where(ls => ls.DueDate <= command.DueDateTo!.Value, command.DueDateTo.HasValue);
}
