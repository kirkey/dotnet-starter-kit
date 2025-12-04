using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Search.v1;

public class SearchLoanGuarantorsSpecs : EntitiesByPaginationFilterSpec<LoanGuarantor, LoanGuarantorResponse>
{
    public SearchLoanGuarantorsSpecs(SearchLoanGuarantorsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(lg => lg.GuaranteeDate, !command.HasOrderBy())
            .Where(lg => lg.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(lg => lg.GuarantorMemberId == command.GuarantorMemberId!.Value, command.GuarantorMemberId.HasValue)
            .Where(lg => lg.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status));
}
