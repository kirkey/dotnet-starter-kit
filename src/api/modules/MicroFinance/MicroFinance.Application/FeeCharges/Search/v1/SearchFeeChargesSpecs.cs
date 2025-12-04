using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Search.v1;

public class SearchFeeChargesSpecs : EntitiesByPaginationFilterSpec<FeeCharge, FeeChargeResponse>
{
    public SearchFeeChargesSpecs(SearchFeeChargesCommand command)
        : base(command) =>
        Query
            .OrderByDescending(fc => fc.ChargeDate, !command.HasOrderBy())
            .Where(fc => fc.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(fc => fc.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(fc => fc.SavingsAccountId == command.SavingsAccountId!.Value, command.SavingsAccountId.HasValue)
            .Where(fc => fc.FeeDefinitionId == command.FeeDefinitionId!.Value, command.FeeDefinitionId.HasValue)
            .Where(fc => fc.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(fc => fc.ChargeDate >= command.ChargeDateFrom!.Value, command.ChargeDateFrom.HasValue)
            .Where(fc => fc.ChargeDate <= command.ChargeDateTo!.Value, command.ChargeDateTo.HasValue);
}
