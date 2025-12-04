using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Search.v1;

public class SearchFixedDepositsSpecs : EntitiesByPaginationFilterSpec<FixedDeposit, FixedDepositResponse>
{
    public SearchFixedDepositsSpecs(SearchFixedDepositsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(fd => fd.DepositDate, !command.HasOrderBy())
            .Where(fd => fd.CertificateNumber.Contains(command.CertificateNumber!), !string.IsNullOrWhiteSpace(command.CertificateNumber))
            .Where(fd => fd.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(fd => fd.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(fd => fd.DepositDate >= command.DepositDateFrom!.Value, command.DepositDateFrom.HasValue)
            .Where(fd => fd.DepositDate <= command.DepositDateTo!.Value, command.DepositDateTo.HasValue)
            .Where(fd => fd.MaturityDate >= command.MaturityDateFrom!.Value, command.MaturityDateFrom.HasValue)
            .Where(fd => fd.MaturityDate <= command.MaturityDateTo!.Value, command.MaturityDateTo.HasValue)
            .Where(fd => fd.PrincipalAmount >= command.MinPrincipal!.Value, command.MinPrincipal.HasValue)
            .Where(fd => fd.PrincipalAmount <= command.MaxPrincipal!.Value, command.MaxPrincipal.HasValue);
}
